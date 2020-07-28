using System;
using System.Collections.Generic;
using System.Linq;
using Abc.MoqComplete.ContextActions.Services;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.UI.Controls.BulbMenu.Anchors;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.FillWithMock
{
    [ContextAction(Group = "C#", Name = "With local variables", Description = "Fill with mock using local variables", Priority = short.MinValue)]
    public sealed class FillWithMockLocalVariableContextAction : ContextActionBase
    {
        [NotNull]
        private static readonly InvisibleAnchor _anchor = FillWithMockFieldsContextAction.Anchor.CreateNext(true);
        private readonly ICSharpContextActionDataProvider _dataProvider;
        private ICsharpMemberProvider _csharpMemberProvider;
        public override string Text => "With local variables";

        public FillWithMockLocalVariableContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            _csharpMemberProvider = ComponentResolver.GetComponent<ICsharpMemberProvider>(_dataProvider);
            return cache.HasKey(AnchorKey.FillWithMockContextActionKey);
        }

        public override IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextActionIntentions(_anchor, BulbThemedIcons.ContextAction.Id);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var block = _dataProvider.GetSelectedElement<IBlock>();
            var selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            if (!(selectedElement.TypeReference.Resolve()?.DeclaredElement is IClass c))
                return null;

            var constructor = c.Constructors.OrderByDescending(x => x.Parameters.Count).FirstOrDefault(x => !x.IsParameterless);
            if (constructor == null)
                return null;

            var parameters = _csharpMemberProvider.GetConstructorParameters(constructor.ToString()).ToArray();

            for (int i = 0; i < constructor.Parameters.Count; i++)
            {
                var shortName = constructor.Parameters[i].ShortName;
                var localVariableStatement = _dataProvider.ElementFactory.CreateStatement("var $0 = new Mock<$1>();", (object)shortName, (object)parameters[i]);

                block.AddStatementBefore(localVariableStatement, selectedElement.GetContainingStatement());
                var argument = _dataProvider.ElementFactory.CreateArgument(ParameterKind.VALUE, _dataProvider.ElementFactory.CreateExpression($"{shortName}.Object"));
                selectedElement.AddArgumentBefore(argument, null);
            }
            return null;
        }
    }
}
