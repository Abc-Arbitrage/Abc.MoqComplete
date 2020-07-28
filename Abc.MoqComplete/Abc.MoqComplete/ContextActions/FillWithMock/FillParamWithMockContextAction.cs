using System;
using System.Linq;
using Abc.MoqComplete.ContextActions.Services;
using Abc.MoqComplete.Services;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.FillWithMock
{
    [ContextAction(Group = "C#", Name = "Fill parameter with Mock", Description = "Fills the current parameter with mock", Priority = short.MinValue + 1)]
    public class FillParamWithMockContextAction : ContextActionBase
    {
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private IObjectCreationExpression _selectedElement;
        private IClassLikeDeclaration _classDeclaration;
        private IClassBody _classBody;
        private IBlock _block;
        private IConstructor _constructor;
        private ICsharpMemberProvider _csharpMemberProvider;

        public FillParamWithMockContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = ComponentResolver.GetComponent<ITestProjectProvider>(_dataProvider);

            if (!testProjectProvider.IsTestProject(_dataProvider.PsiModule))
                return false;

            _csharpMemberProvider = ComponentResolver.GetComponent<ICsharpMemberProvider>(_dataProvider);
            _selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            _block = _dataProvider.GetSelectedElement<IBlock>();
            _classBody = _dataProvider.GetSelectedElement<IClassBody>();
            _classDeclaration = _classBody?.GetContainingTypeDeclaration() as IClassLikeDeclaration;

            if (_classDeclaration == null || _block == null || _selectedElement == null)
                return false;

            if (!(_selectedElement.TypeReference?.Resolve().DeclaredElement is IClass c))
                return false;

            var parameterCount = _selectedElement.ArgumentList?.Arguments.Count(x => x.Kind != ParameterKind.UNKNOWN);
            _constructor = c.Constructors.ToArray().FirstOrDefault(x => !x.IsParameterless && x.Parameters.Count > parameterCount);
            if (_constructor == null)
                return false;

            var previousTokenType = _dataProvider.TokenBeforeCaret?.NodeType as ITokenNodeType;
            var nextTokenType = _dataProvider.TokenAfterCaret?.NodeType as ITokenNodeType;
            if (previousTokenType == null || nextTokenType == null)
                return false;

            if (previousTokenType.TokenRepresentation == " ")
                previousTokenType = _dataProvider.PsiFile.FindTokenAt(_dataProvider.TokenBeforeCaret.GetTreeStartOffset() - 1)?.NodeType as ITokenNodeType;

            if (nextTokenType.TokenRepresentation == " ")
                nextTokenType = _dataProvider.PsiFile.FindTokenAt(_dataProvider.TokenBeforeCaret.GetTreeEndOffset() + 1)?.NodeType as ITokenNodeType;

            if (previousTokenType == null || nextTokenType == null)
                return false;

            if (previousTokenType.TokenRepresentation == "(")
                return nextTokenType.TokenRepresentation == ")" || nextTokenType.TokenRepresentation == ",";

            if (previousTokenType.TokenRepresentation == ",")
                return !nextTokenType.IsIdentifier;

            return false;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var argumentList = _selectedElement.ArgumentList;
            var parameters = _csharpMemberProvider.GetConstructorParameters(_constructor.ToString()).ToArray();
            var mockFieldsByType = _csharpMemberProvider.GetClassFields(_classBody, _selectedElement.Language);
            var parameterNumber = GetCurrentParameterNumber();
            var shortName = _constructor.Parameters[parameterNumber].ShortName;
            var currentParam = parameters[parameterNumber];
            var typeString = _csharpMemberProvider.GetGenericMock(currentParam);

            if (!mockFieldsByType.TryGetValue(typeString, out var name))
            {
                var mockType = TypeFactory.CreateTypeByCLRName(typeString, _dataProvider.PsiModule);
                var field = _dataProvider.ElementFactory.CreateTypeMemberDeclaration("private $0 $1;", (object)mockType, (object)shortName);
                var options = new SuggestionOptions(defaultName: shortName);
                name = _dataProvider.PsiServices.Naming.Suggestion.GetDerivedName(field.DeclaredElement, NamedElementKinds.PrivateInstanceFields, ScopeKind.Common, _selectedElement.Language, options, _dataProvider.SourceFile);
                field.SetName(name);
                _classDeclaration.AddClassMemberDeclaration((IClassMemberDeclaration)field);

                var statement = _dataProvider.ElementFactory.CreateStatement("$0 = new Mock<$1>();", (object)name, (object)currentParam);
                _block.AddStatementBefore(statement, _selectedElement.GetContainingStatement());
            }

            var argument = _dataProvider.ElementFactory.CreateArgument(ParameterKind.VALUE, _dataProvider.ElementFactory.CreateExpression($"{name}.Object"));
            ICSharpArgument arg;
            var shouldRemoveEndComma = true;

            if (argumentList.Arguments.Count <= 1)
                arg = _selectedElement.AddArgumentAfter(argument, null);
            else if (parameterNumber != 0)
                arg = _selectedElement.AddArgumentAfter(argument, argumentList.Arguments[parameterNumber - 1]);
            else
            {
                arg = _selectedElement.AddArgumentBefore(argument, argumentList.Arguments[1]);
                shouldRemoveEndComma = false;
            }

            var argumentRange = arg.GetDocumentRange();

            // Remove last comma Hack!
            return textControl =>
            {
                TextRange range;
                
                if (shouldRemoveEndComma)
                    range = new TextRange(argumentRange.EndOffset.Offset, argumentRange.EndOffset.Offset + 1);
                else
                    range = new TextRange(argumentRange.StartOffset.Offset - 2, argumentRange.StartOffset.Offset);

                var text = textControl.Document.GetText(range);
                
                if (text.Contains(","))
                    textControl.Document.DeleteText(range);
            };
        }

        private int GetCurrentParameterNumber()
        {
            var delimiterPositions = _selectedElement.Delimiters.Select(x => x.GetNavigationRange().StartOffset.Offset).ToArray();
            var currentPosition = _dataProvider.DocumentSelection.StartOffset.Offset;

            var parameterNumber = 0;
            while (parameterNumber < delimiterPositions.Length && currentPosition > delimiterPositions[parameterNumber])
                parameterNumber++;

            return parameterNumber;
        }

        public override string Text => "Fill current parameter with Mock";
    }
}
