using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
    [StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
    public class SuspiciousCallbackWarning : IHighlighting
    {
        private readonly DocumentRange _documentRange;

        public SuspiciousCallbackWarning(IInvocationExpression element, DocumentRange documentRange)
        {
            _documentRange = documentRange;
        }

        public bool IsValid() => true;

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip => "Suspicious Callback method call: Generic types do not match";
        public string ErrorStripeToolTip { get; }
    }
}
