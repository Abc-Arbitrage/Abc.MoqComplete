using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace Abc.MoqComplete.CodeAnalysis
{
	[StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
	public sealed class SuspiciousCallbackWarning : BaseCallbackWarning
	{
		public SuspiciousCallbackWarning(DocumentRange documentRange)
			: base("Suspicious Callback method call: Generic types do not match", documentRange)
		{
		}
	}
}