using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services.MethodProvider
{
	[SolutionComponent(Instantiation.ContainerAsyncPrimaryThread)]
	public class AutoMockerMockedMethodProvider : BaseMethodProvider, IAutoMockerMockedMethodProvider
	{
		private readonly IMoqMethodIdentifier _methodIdentifier;

		public AutoMockerMockedMethodProvider(IMoqMethodIdentifier methodIdentifier)
		{
			_methodIdentifier = methodIdentifier;
		}

		protected override bool IsSetupMethod(IInvocationExpression invocationExpression)
		{
			return _methodIdentifier.IsAutoMockerSetupMethod(invocationExpression);
		}
	}
}