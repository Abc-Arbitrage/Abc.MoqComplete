using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services
{
    [SolutionComponent]
    public class MoqMethodIdentifier : BaseMethodIdentifier, IMoqMethodIdentifier
    {
        public bool IsMoqSetupMethod(IInvocationExpression invocationExpression)
            => IsMethod(invocationExpression,
                        declaredElement => IsMethodString(declaredElement,
                                                          "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> T]] expression)",
                                                          "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> T, TResult -> TResult]] expression)"));
        
        public bool IsMoqVerifyMethod(IInvocationExpression invocationExpression)
            => IsMethod(invocationExpression,
                        declaredElement => IsMethodString(declaredElement,
                                                          "Method:Moq.Mock`1.Verify(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> T]] expression)",
                                                          "Method:Moq.Mock`1.Verify(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> T, TResult -> TResult]] expression)"));

        public bool IsMoqReturnMethod(IInvocationExpression invocationExpression) 
            => IsMethod(invocationExpression,
                        declaredElement => IsMethodStartingWithString(declaredElement,
                                                                      "Method:Moq.Language.IReturns`2.Returns",
                                                                      "Method:Moq.Language.IReturns`1.Returns"));

        public bool IsMoqCallbackMethod(IInvocationExpression expression) 
            => IsMethod(expression,
                        declaredElement => IsMethodStartingWithString(declaredElement,
                                                                      "Method:Moq.Language.ICallback.Callback(System.Action",
                                                                      "Method:Moq.Language.ICallback`2.Callback(System.Action"));
    }
}