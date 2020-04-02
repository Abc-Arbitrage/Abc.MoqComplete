using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
	public sealed class AutoMockerSuspiciousCallbackWarning : BaseCallbackWarning
	{
		public AutoMockerSuspiciousCallbackWarning(IInvocationExpression element, DocumentRange documentRange)
			: base("AutoMocker suspicious Callback method call: Generic types do not match", documentRange)
		{
		}
	}
}