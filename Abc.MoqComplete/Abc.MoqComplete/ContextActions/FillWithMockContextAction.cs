using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using JetBrains.TextControl;
using JetBrains.Util;
using IBlock = JetBrains.ReSharper.Psi.CSharp.Tree.IBlock;
using IClassBody = JetBrains.ReSharper.Psi.CSharp.Tree.IClassBody;
using IClassLikeDeclaration = JetBrains.ReSharper.Psi.CSharp.Tree.IClassLikeDeclaration;
using IObjectCreationExpression = JetBrains.ReSharper.Psi.CSharp.Tree.IObjectCreationExpression;

namespace Abc.MoqComplete.ContextActions
{
    [ContextAction(Group = "C#", Name = "Fill with Mock", Description = "Fills the constructor with mocks", Priority = short.MinValue)]
    public class FillWithMockContextAction : ContextActionBase
    {
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private IObjectCreationExpression _selectedElement;
        private IParameterProvider _parameterProvider;

        public FillWithMockContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = ComponentResolver.GetComponent<ITestProjectProvider>(_dataProvider);
            _selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            _parameterProvider = ComponentResolver.GetComponent<IParameterProvider>(_dataProvider);

            return testProjectProvider.IsTestProject(_dataProvider.PsiModule) && _selectedElement != null && _selectedElement.Arguments.Count == 0;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var block = _dataProvider.GetSelectedElement<IBlock>();
            var classBody = _dataProvider.GetSelectedElement<IClassBody>();
            var classDeclaration = classBody?.GetContainingTypeDeclaration() as IClassLikeDeclaration;

            if (classDeclaration == null || block == null)
                return null;

            if (!(_selectedElement.TypeReference.Resolve()?.DeclaredElement is IClass c))
                return null;

            var constructor = c.Constructors.ToArray().OrderByDescending(x => x.Parameters.Count).FirstOrDefault(x => !x.IsParameterless);
            if (constructor == null)
                return null;

            var parameters = _parameterProvider.GetParameters(constructor.ToString()).ToArray();
            var naming = _dataProvider.PsiServices.Naming;
            var mockFieldsByType = GetFields(classBody);

            for (int i = 0; i < constructor.Parameters.Count; i++)
            {
                var shortName = constructor.Parameters[i].ShortName;
                var typeString = GetGenericMock(parameters[i]);
                
                if (!mockFieldsByType.TryGetValue(typeString, out var name))
                {
                    var mockType = TypeFactory.CreateTypeByCLRName(typeString, _dataProvider.PsiModule);
                    var field = _dataProvider.ElementFactory.CreateTypeMemberDeclaration("private $0 $1;", (object)mockType, (object)shortName);
                    var options = new SuggestionOptions(defaultName: shortName);
                    name = naming.Suggestion.GetDerivedName(field.DeclaredElement, NamedElementKinds.PrivateInstanceFields, ScopeKind.Common, _selectedElement.Language,
                                                                options, _dataProvider.SourceFile);
                    field.SetName(name);
                    classDeclaration.AddClassMemberDeclaration((IClassMemberDeclaration)field);

                    var statement = _dataProvider.ElementFactory.CreateStatement("$0 = new Mock<$1>();", (object)name, (object)parameters[i]);
                    block.AddStatementBefore(statement, _selectedElement.GetContainingStatement());
                }

                var argument = _dataProvider.ElementFactory.CreateArgument(ParameterKind.VALUE, _dataProvider.ElementFactory.CreateExpression($"{name}.Object"));
                _selectedElement.AddArgumentBefore(argument, null);
            }

            return null;
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

        public override string Text => "Fill with Mocks";
    }
}
