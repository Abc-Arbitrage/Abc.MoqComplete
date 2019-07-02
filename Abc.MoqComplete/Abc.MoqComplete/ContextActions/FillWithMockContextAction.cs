using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using MoqComplete.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoqComplete.ContextActions
{
    [ContextAction(Group = "C#", Name = "Fill with Mock", Description = "Fills the constructor with mocks", Priority = short.MinValue)]
    public class FillWithMockContextAction : ContextActionBase
    {
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private IObjectCreationExpression _selectedElement;

        public FillWithMockContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = GetComponent<ITestProjectProvider>();
            _selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);

            return testProjectProvider.IsTestProject(_dataProvider.PsiModule) && _selectedElement != null;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var node = _selectedElement.Parent;
            while (node != null && !(node is IClassBody))
                node = node.Parent;

            var declaration = ((IClassBody)node)?.GetContainingTypeDeclaration() as IClassLikeDeclaration;

            if (declaration == null)
                return null;

            if (!(_selectedElement.TypeReference.Resolve()?.DeclaredElement is IClass c))
                return null;

            var constructor = c.Constructors.ToArray().OrderByDescending(x => x.Parameters.Count).FirstOrDefault(x => !x.IsParameterless);

            if (constructor == null)
                return null;

            var parameters = GetParameters(constructor.ToString()).ToArray();
            var naming = _dataProvider.PsiServices.Naming;

            for (int i = 0; i < constructor.Parameters.Count; i++)
            {
                var shortName = constructor.Parameters[i].ShortName;
                var mockType = TypeFactory.CreateTypeByCLRName($"Moq.Mock<{parameters[i]}>", _dataProvider.PsiModule);
                var field = _dataProvider.ElementFactory.CreateTypeMemberDeclaration("private readonly $0 $1 = new $0();", (object)mockType, (object)shortName);
                var options = new SuggestionOptions(defaultName: shortName);
                var name = naming.Suggestion.GetDerivedName(field.DeclaredElement, NamedElementKinds.PrivateInstanceFields, ScopeKind.Common, _selectedElement.Language, options, _dataProvider.SourceFile);
                field.SetName(name);
                declaration.AddClassMemberDeclaration((IClassMemberDeclaration)field);

                var argument = _dataProvider.ElementFactory.CreateArgument(ParameterKind.VALUE, _dataProvider.ElementFactory.CreateExpression($"{name}.Object"));
                _selectedElement.AddArgumentBefore(argument, null);
            }

            return null;
        }

        private IEnumerable<string> GetParameters(string constructorString)
        {
            var index = 0;

            while (constructorString[index] != '(')
                index++;

            var sb = new StringBuilder();
            while (constructorString[index] != ')')
            {
                index++;
                if (constructorString[index] == ',' || constructorString[index] == ')')
                {
                    yield return sb.ToString();
                    sb.Clear();
                }
                else
                    sb.Append(constructorString[index]);
            }
        }

        private T GetComponent<T>()
            where T : class
            => _dataProvider.PsiModule.GetSolution().GetComponent<T>();

        public override string Text => "Fill with Mock";
    }
}
