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
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.FillWithMock
{
    [ContextAction(GroupType = typeof(CSharpContextActions), Name = "With local variables", Description = "Fill parameter with mock using local variables", Priority = short.MinValue)]
    public class FillParamWithMockLocalVariableContextAction : ContextActionBase
    {
        [NotNull]
        private static readonly InvisibleAnchor _anchor = FillParamWithMockContextAction.Anchor.CreateNext(true);
        private readonly ICSharpContextActionDataProvider _dataProvider;
        public override string Text => "With local variables";

        public FillParamWithMockLocalVariableContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return cache.HasKey(AnchorKey.FillParamWithMockContextActionKey);
        }

        public override IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextActionIntentions(_anchor, BulbThemedIcons.ContextAction.Id);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var csharpMemberProvider = ComponentResolver.GetComponent<ICsharpMemberProvider>(_dataProvider);
            var selectedElement = _dataProvider.GetSelectedElement<IObjectCreationExpression>(false, false);
            var argumentList = selectedElement.ArgumentList;
            var @class = (IClass)selectedElement.TypeReference?.Resolve().DeclaredElement;
            var parameterCount = selectedElement.ArgumentList?.Arguments.Count(x => x.Kind != ParameterKind.UNKNOWN);
            var constructor = @class.Constructors.ToArray().FirstOrDefault(x => !x.IsParameterless && x.Parameters.Count > parameterCount);
            var parameters = csharpMemberProvider.GetConstructorParameters(constructor).ToArray();
            var parameterNumber = csharpMemberProvider.GetCurrentParameterNumber(selectedElement, _dataProvider);
            var shortName = constructor.Parameters[parameterNumber].ShortName;
            var currentParam = parameters[parameterNumber];
            var block = _dataProvider.GetSelectedElement<IBlock>();
            var localVariableStatement = _dataProvider.ElementFactory.CreateStatement("var $0 = new Mock<$1>();", (object)shortName, currentParam);
            
            block.AddStatementBefore(localVariableStatement, selectedElement.GetContainingStatement());

            return csharpMemberProvider.FillCurrentParameterWithMock(shortName, argumentList, selectedElement, parameterNumber, _dataProvider);
        }
    }
}
