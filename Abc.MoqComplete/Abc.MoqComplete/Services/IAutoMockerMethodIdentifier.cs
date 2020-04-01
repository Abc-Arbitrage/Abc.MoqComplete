using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services
{
    public interface IAutoMockerMethodIdentifier
    {
        bool IsAutoMockerSetupMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsAutoMockerVerifyMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsAutoMockerReturnMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsAutoMockerCallbackMethod([CanBeNull]  IInvocationExpression invocationExpression);
    }
}