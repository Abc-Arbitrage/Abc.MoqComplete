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
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.FillWithMock
{
    [ContextAction(Group = "C#", Name = "Fill with Mock", Description = "Fills the constructor with mocks")]
    public class FillWithMockFieldsContextAction : ContextActionBase
    {
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private IObjectCreationExpression _selectedElement;
        private ICsharpMemberProvider _csharpMemberProvider;

        [NotNull]
        private static readonly IAnchor _anchor = new SubmenuAnchor(IntentionsAnchors.ContextActionsAnchor, SubmenuBehavior.Executable);
        [NotNull]
        public static readonly InvisibleAnchor Anchor = new InvisibleAnchor(_anchor);

        public override IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextActionIntentions(Anchor);
        }

        public FillWithMockFieldsContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var testProjectProvider = ComponentResolver.GetComponent<ITestProjectProvider>(_dataProvider);
            _selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            _csharpMemberProvider = ComponentResolver.GetComponent<ICsharpMemberProvider>(_dataProvider);

            var isAvailable = testProjectProvider.IsTestProject(_dataProvider.PsiModule) && _selectedElement != null && _selectedElement.Arguments.Count == 0;
            if (isAvailable)
                cache.PutKey(AnchorKey.FillWithMockContextActionKey);

            return isAvailable;
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

            var constructor = c.Constructors.OrderByDescending(x => x.Parameters.Count).FirstOrDefault(x => !x.IsParameterless);
            if (constructor == null)
                return null;

            var parameters = _csharpMemberProvider.GetConstructorParameters(constructor.ToString()).ToArray();
            var naming = _dataProvider.PsiServices.Naming;
            var mockFieldsByType = _csharpMemberProvider.GetClassFields(classBody, _selectedElement.Language);

            for (int i = 0; i < constructor.Parameters.Count; i++)
            {
                var shortName = constructor.Parameters[i].ShortName;
                var typeString = _csharpMemberProvider.GetGenericMock(parameters[i]);
                
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

        public override string Text => "Fill with Mocks";
    }
}
