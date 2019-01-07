using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;
using System.Linq;

namespace MoqComplete.Extensions
{
    internal static class MoqExtensions
    {
        [CanBeNull]
        public static IMethod GetMockedMethodFromSetupMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || !IsMoqSetupMethod(invocationExpression))
                return null;

            var methodArguments = invocationExpression.ArgumentList.Arguments;
            if (methodArguments.Count != 1)
                return null;

            var lambdaExpression = methodArguments[0].Value as ILambdaExpression;
            if (lambdaExpression == null)
                return null;

            var mockMethodInvocationExpression = lambdaExpression.BodyExpression as IInvocationExpression;
            if (mockMethodInvocationExpression == null || mockMethodInvocationExpression.Reference == null)
                return null;

            var targetMethodResolveResult = mockMethodInvocationExpression.Reference.Resolve();
            if (targetMethodResolveResult.ResolveErrorType == ResolveErrorType.OK)
            {
                return targetMethodResolveResult.DeclaredElement as IMethod;
            }

            return null;
        }

        public static bool IsMoqSetupMethod([CanBeNull] IDeclaredElement declaredElement)
        {
            var declaredElementAsString = declaredElement.ConvertToString();
            if (declaredElementAsString == "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> T]] expression)")
                return true;
            if (declaredElementAsString == "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> T, TResult -> TResult]] expression)")
                return true;
            return false;
        }

        public static bool IsMoqSetupMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return false;

            var resolveResult = invocationExpression.Reference.Resolve();
            if (resolveResult.ResolveErrorType == ResolveErrorType.MULTIPLE_CANDIDATES)
                return resolveResult.Result.Candidates.Any(IsMoqSetupMethod);

            return IsMoqSetupMethod(resolveResult.DeclaredElement);
        }
    }
}
