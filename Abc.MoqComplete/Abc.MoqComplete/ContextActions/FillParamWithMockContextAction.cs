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
using JetBrains.TextControl;
using JetBrains.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abc.MoqComplete.ContextActions.Services;

namespace Abc.MoqComplete.ContextActions
{
    [ContextAction(Group = "C#", Name = "Fill parameter with Mock", Description = "Fills the current parameter with mock", Priority = short.MinValue + 1)]
    public class FillParamWithMockContextAction : ContextActionBase
    {
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private IObjectCreationExpression _selectedElement;
        private int _parameterNumber;
        private IClassLikeDeclaration _classDeclaration;
        private IClassBody _classBody;
        private IBlock _block;
        private IConstructor _constructor;
        private IParameterProvider _parameterProvider;

        public FillParamWithMockContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = ComponentResolver.GetComponent<ITestProjectProvider>(_dataProvider);
            _parameterProvider = ComponentResolver.GetComponent<IParameterProvider>(_dataProvider);
            _selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            _block = _dataProvider.GetSelectedElement<IBlock>();
            _classBody = _dataProvider.GetSelectedElement<IClassBody>();
            _classDeclaration = _classBody?.GetContainingTypeDeclaration() as IClassLikeDeclaration;

            if (_classDeclaration == null || _block == null || _selectedElement == null)
                return false;

            if (!(_selectedElement.TypeReference?.Resolve().DeclaredElement is IClass c))
                return false;

            _parameterNumber = _selectedElement.ArgumentList.Arguments.Count(x => x.Kind != ParameterKind.UNKNOWN);
            _constructor = c.Constructors.ToArray().FirstOrDefault(x => !x.IsParameterless && x.Parameters.Count > _parameterNumber);
            if (_constructor == null)
                return false;

            return testProjectProvider.IsTestProject(_dataProvider.PsiModule);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var argumentList = _selectedElement.ArgumentList;
            var parameters = _parameterProvider.GetParameters(_constructor.ToString()).ToArray();
            var mockFieldsByType = GetFields(_classBody);
            var shortName = _constructor.Parameters[_parameterNumber].ShortName;
            var currentParam = parameters[_parameterNumber];
            var typeString = GetGenericMock(currentParam);

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
            var previousArgument = _parameterNumber != 0 ? argumentList.Arguments[_parameterNumber - 1] : null;
            var arg = _selectedElement.AddArgumentAfter(argument, previousArgument);
            var argumentRange = arg.GetDocumentRange();
            
            // Remove last comma Hack!
            return textControl =>
            {
                var range = new TextRange(argumentRange.EndOffset.Offset, argumentRange.EndOffset.Offset + 1);
                if (textControl.Document.GetText(range) == ",")
                    textControl.Document.DeleteText(range);
            };
        }

        private static string GetGenericMock(string typeStr) => $"Moq.Mock<{typeStr}>";

        private Dictionary<string, string> GetFields(IClassBody classBody)
        {
            var dic = new Dictionary<string, string>();
            var fields = classBody.FieldDeclarations.Select(x => x.TypeUsage.FirstChild as IReferenceName).Where(x => x != null && x.ShortName == "Mock").ToArray();

            foreach (var referenceName in fields)
            {
                var types = referenceName.TypeArguments.Select(x => x.GetPresentableName(_selectedElement.Language, DeclaredElementPresenterTextStyles.Empty).Text);
                var strType = string.Join(",", types);
                var mockType = GetGenericMock(strType);
                var field = (IFieldDeclaration)referenceName.Parent.NextSibling.NextSibling;

                if (!dic.ContainsKey(mockType))
                    dic.Add(mockType, field.DeclaredName);
            }

            return dic;
        }
        
        public override string Text => "Fill current parameter with Mock";
    }
}
