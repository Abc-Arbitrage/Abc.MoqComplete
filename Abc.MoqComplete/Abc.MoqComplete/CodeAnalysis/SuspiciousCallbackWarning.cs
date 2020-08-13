using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace Abc.MoqComplete.CodeAnalysis
{
    [StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
	public class SuspiciousCallbackWarning : IHighlighting
	{
		private readonly DocumentRange _documentRange;

		public SuspiciousCallbackWarning(string toolTip, DocumentRange documentRange)
		{
			_documentRange = documentRange;
			ToolTip = toolTip;
		}

		public bool IsValid() => true;

		public DocumentRange CalculateRange() => _documentRange;

		public string ToolTip { get; }

		public string ErrorStripeToolTip { get; }
	}
}