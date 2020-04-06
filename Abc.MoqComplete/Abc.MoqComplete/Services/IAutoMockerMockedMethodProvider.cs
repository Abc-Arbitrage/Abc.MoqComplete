using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.Services
{
	public interface IAutoMockerMockedMethodProvider
	{
		IMethod GetMockedMethodFromSetupMethod([CanBeNull] IInvocationExpression invocationExpression);

		TreeNodeCollection<ICSharpArgument>? GetMockedMethodParametersFromSetupMethod(IInvocationExpression invocationExpression);

		IEnumerable<string> GetMockedMethodParameterTypes(IInvocationExpression invocationExpression);
	}
}