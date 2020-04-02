using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace Abc.MoqComplete.CodeAnalysis
{
	[StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
	public sealed class AutoMockerSuspiciousCallbackWarning : BaseCallbackWarning
	{
		public AutoMockerSuspiciousCallbackWarning(DocumentRange documentRange)
			: base("AutoMocker suspicious Callback method call: Generic types do not match", documentRange)
		{
		}
	}
}