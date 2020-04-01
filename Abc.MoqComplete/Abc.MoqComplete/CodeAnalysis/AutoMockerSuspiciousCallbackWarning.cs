using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.GutterMarksGroup)]
	public class AutoMockerSuspiciousCallbackWarning : IHighlighting
	{
		private readonly DocumentRange _documentRange;

		public AutoMockerSuspiciousCallbackWarning(IInvocationExpression element, DocumentRange documentRange)
		{
			_documentRange = documentRange;
		}

		public bool IsValid()
		{
			return true;
		}

		public DocumentRange CalculateRange()
		{
			return _documentRange;
		}

		public string ToolTip => "AutoMocker suspicious Callback method call: Generic types do not match";

		public string ErrorStripeToolTip { get; }
	}
}