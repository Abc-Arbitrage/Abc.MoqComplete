using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace Abc.MoqComplete.CodeAnalysis
{
	public abstract class BaseCallbackWarning : IHighlighting
	{
		private readonly DocumentRange _documentRange;

		protected BaseCallbackWarning(string toolTip, DocumentRange documentRange)
		{
			_documentRange = documentRange;
			ToolTip = toolTip;
		}

		public bool IsValid() => true;

		public DocumentRange CalculateRange() => _documentRange;

		/// <inheritdoc />
		public string ToolTip { get; }

		public string ErrorStripeToolTip { get; }
	}
}