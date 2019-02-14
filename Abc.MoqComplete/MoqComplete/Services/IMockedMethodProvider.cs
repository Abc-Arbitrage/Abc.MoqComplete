using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MoqComplete.Services
{
    public interface IMockedMethodProvider
    {
        IMethod GetMockedMethodFromSetupMethod([CanBeNull] IInvocationExpression invocationExpression);
    }
}
