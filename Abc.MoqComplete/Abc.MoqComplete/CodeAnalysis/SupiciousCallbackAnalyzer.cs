using Abc.MoqComplete.Services;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] { typeof(SuspiciousCallbackWarning) })]
	public class SuspiciousCallbackAnalyzer : BaseCallbackAnalyzer
    {
		protected override TreeNodeCollection<ICSharpArgument>? GetArguments(ISolution solution, IInvocationExpression methodInvocation)
		{
			var mockedMethodProvider = solution.GetComponent<IMockedMethodProvider>();

			return mockedMethodProvider.GetMockedMethodParametersFromSetupMethod(methodInvocation);
		}

        protected override string WarningText => "Suspicious Callback method call: Generic types do not match";
    }
}