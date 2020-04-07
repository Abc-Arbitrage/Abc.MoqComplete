using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services.MethodProvider
{
	[SolutionComponent]
	public class MoqMockedMethodProvider : BaseMethodProvider, IMoqMockedMethodProvider
	{
		private readonly IMoqMethodIdentifier _methodIdentifier;

		public MoqMockedMethodProvider(IMoqMethodIdentifier methodIdentifier)
		{
			_methodIdentifier = methodIdentifier;
		}

		protected override bool IsSetupMethod(IInvocationExpression invocationExpression)
		{
			return _methodIdentifier.IsMoqSetupMethod(invocationExpression);
		}
	}
}