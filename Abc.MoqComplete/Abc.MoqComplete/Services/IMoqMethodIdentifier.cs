﻿using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services
{
    public interface IMoqMethodIdentifier
    {
		bool IsAutoMockerSetupMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsMoqSetupMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsMoqVerifyMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsMoqReturnMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsMoqReturnAsyncMethod([CanBeNull]  IInvocationExpression invocationExpression);
        bool IsMoqCallbackMethod([CanBeNull]  IInvocationExpression invocationExpression);
    }
}
