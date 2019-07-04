using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;

namespace Abc.MoqComplete.Extensions
{
    public static class LookupItemExtensions
    {
        public readonly static string MoqCompleteOrderString = "#MoqComplete";

        public static void SetTopPriority(this ILookupItem lookupItem)
        {
            lookupItem.PlaceTop(0);
            lookupItem.WithHighSelectionPriority();
            lookupItem.Placement.OrderString = MoqCompleteOrderString;
        }
    }
}