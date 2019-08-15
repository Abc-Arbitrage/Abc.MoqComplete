using System.Linq;
using Abc.MoqComplete.Extensions;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace Abc.MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public sealed class SlowPriorityAdjuster : ISlowCodeCompletionItemsProvider
    {
        public object IsAvailable(ISpecificCodeCompletionContext context) 
            => true;

        public bool AddLookupItems(ISpecificCodeCompletionContext context, IItemsCollector collector, object data) 
            => true;

        public void TransformItems(ISpecificCodeCompletionContext context, IItemsCollector collector, object data)
        {
            PriorityAdjuster.PutMoqCompleteAtTop(collector);
        }

        public EvaluationMode SupportedEvaluationMode => EvaluationMode.All;
    }

    [Language(typeof(CSharpLanguage))]
    public sealed class PriorityAdjuster : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
            => true;

        protected override void TransformItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            PutMoqCompleteAtTop(collector);
        }

        public static void PutMoqCompleteAtTop(IItemsCollector collector)
        {
            var moqComplete = collector.Items.Where(x => x.Placement.OrderString == LookupItemExtensions.MoqCompleteOrderString).ToList();
            var otherItems = collector.Items.Where(x => x.Placement.OrderString != LookupItemExtensions.MoqCompleteOrderString).ToList();

            if (moqComplete.Any())
            {
                otherItems.ForEach(x =>
                {
                    var rank = (byte)(x.Placement.Rank == 255 ? 255 : x.Placement.Rank + 1);
                    x.WithLowSelectionPriority();
                    x.Placement.Rank = rank;
                });
            }
        }

        public override CompletionMode SupportedCompletionMode => CompletionMode.All;

        public override EvaluationMode SupportedEvaluationMode => EvaluationMode.All;
    }
}