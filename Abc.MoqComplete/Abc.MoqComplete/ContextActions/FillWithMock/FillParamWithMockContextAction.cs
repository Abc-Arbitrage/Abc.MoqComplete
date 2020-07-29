using System;
using System.Collections.Generic;
using System.Linq;
using Abc.MoqComplete.ContextActions.Services;
using Abc.MoqComplete.Services;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.UI.Controls.BulbMenu.Anchors;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
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
        private ICsharpParameterProvider _parameterProvider;
        [NotNull]
        private static readonly IAnchor _anchor = new SubmenuAnchor(IntentionsAnchors.ContextActionsAnchor, SubmenuBehavior.Executable);
        [NotNull]
        public static readonly InvisibleAnchor Anchor = new InvisibleAnchor(_anchor);
        private int _parameterNumber;

        public FillParamWithMockContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextActionIntentions(Anchor);
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = ComponentResolver.GetComponent<ITestProjectProvider>(_dataProvider);

            if (!testProjectProvider.IsTestProject(_dataProvider.PsiModule))
                return false;

            _parameterProvider = ComponentResolver.GetComponent<ICsharpParameterProvider>(_dataProvider);
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

            var isAvailable = false;
            if (previousTokenType.TokenRepresentation == "(")
                isAvailable = nextTokenType.TokenRepresentation == ")" || nextTokenType.TokenRepresentation == ",";

            else if (previousTokenType.TokenRepresentation == ",")
                isAvailable = !nextTokenType.IsIdentifier;

            if (!isAvailable)
                return false;

            _parameterNumber = _parameterProvider.GetCurrentParameterNumber(_selectedElement, _dataProvider);
            var parameter = _constructor.Parameters[_parameterNumber];

            if (!_csharpMemberProvider.IsAbstractOrInterface(parameter))
                return false;

            cache.PutKey(AnchorKey.FillParamWithMockContextActionKey);

            return true;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var argumentList = _selectedElement.ArgumentList;
            var parameters = _csharpMemberProvider.GetConstructorParameters(_constructor.ToString()).ToArray();
            var mockFieldsByType = _csharpMemberProvider.GetClassFields(_classBody, _selectedElement.Language);
            var shortName = _constructor.Parameters[_parameterNumber].ShortName;
            var currentParam = parameters[_parameterNumber];
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

            return _parameterProvider.FillCurrentParameterWithMock(name, argumentList, _selectedElement, _parameterNumber, _dataProvider);
        }

        public override string Text => "Fill current parameter with Mock";
    }
}
