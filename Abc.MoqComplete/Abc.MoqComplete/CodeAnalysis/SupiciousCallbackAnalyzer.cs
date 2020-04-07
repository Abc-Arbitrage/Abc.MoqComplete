using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] { typeof(SuspiciousCallbackWarning) })]
	public class SuspiciousCallbackAnalyzer : BaseCallbackAnalyzer<IMockedMethodProvider>
    {
        protected override string WarningText => "Suspicious Callback method call: Generic types do not match";
    }
}