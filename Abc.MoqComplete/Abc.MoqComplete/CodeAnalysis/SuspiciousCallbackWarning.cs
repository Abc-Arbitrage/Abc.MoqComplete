using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
	public sealed class SuspiciousCallbackWarning : BaseCallbackWarning
	{
		public SuspiciousCallbackWarning(IInvocationExpression element, DocumentRange documentRange)
			: base("Suspicious Callback method call: Generic types do not match", documentRange)
		{
		}
	}
}