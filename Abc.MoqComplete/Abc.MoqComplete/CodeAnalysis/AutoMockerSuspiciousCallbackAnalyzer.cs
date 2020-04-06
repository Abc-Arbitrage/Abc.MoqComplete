using System.Linq;
using Abc.MoqComplete.Services;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Conversions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] { typeof(AutoMockerSuspiciousCallbackWarning) })]
	public class AutoMockerSuspiciousCallbackAnalyzer : BaseCallbackAnalyzer
	{
		/// <inheritdoc />
		protected override TreeNodeCollection<ICSharpArgument>? GetArguments(ISolution solution, IInvocationExpression methodInvocation)
		{
			var mockedMethodProvider = solution.GetComponent<IAutoMockerMockedMethodProvider>();

			return mockedMethodProvider.GetMockedMethodParametersFromSetupMethod(methodInvocation);
		}

		/// <inheritdoc />
		protected override void AddHighlighting(IHighlightingConsumer consumer, DocumentRange range)
		{
			consumer.AddHighlighting(new AutoMockerSuspiciousCallbackWarning(range));
		}
	}
}