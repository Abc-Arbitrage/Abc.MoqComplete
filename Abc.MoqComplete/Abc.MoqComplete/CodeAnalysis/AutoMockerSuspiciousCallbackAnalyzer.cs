using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	[ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] { typeof(SuspiciousCallbackWarning) })]
	public class AutoMockerSuspiciousCallbackAnalyzer : BaseCallbackAnalyzer<IAutoMockerMockedMethodProvider>
	{
        protected override string WarningText => "AutoMocker suspicious Callback method call: Generic types do not match";
    }
}