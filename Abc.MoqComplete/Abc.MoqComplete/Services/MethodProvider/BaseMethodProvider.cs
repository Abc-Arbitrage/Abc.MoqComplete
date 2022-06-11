using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.Services.MethodProvider
{
    public abstract class BaseMethodProvider
    {
        protected abstract bool IsSetupMethod(IInvocationExpression invocationExpression);

        public IMethod GetMockedMethodFromSetupMethod(IInvocationExpression invocationExpression)
        {
            var mockMethodInvocationExpression = GetMockedMethodInvocation(invocationExpression);

            if (mockMethodInvocationExpression?.Reference == null)
            {
                return null;
            }

            var targetMethodResolveResult = mockMethodInvocationExpression.Reference.Resolve();

            if (targetMethodResolveResult.ResolveErrorType == ResolveErrorType.OK)
            {
                return targetMethodResolveResult.DeclaredElement as IMethod;
            }

            return null;
        }

        public TreeNodeCollection<ICSharpArgument>? GetMockedMethodParametersFromSetupMethod(IInvocationExpression invocationExpression)
        {
            var mockMethodInvocationExpression = GetMockedMethodInvocation(invocationExpression);

            return mockMethodInvocationExpression?.ArgumentList?.Arguments;
        }

        public IEnumerable<string> GetMockedMethodParameterTypesString(IInvocationExpression invocation)
        {
            var mockedMethod = GetMockedMethodFromSetupMethod(invocation);
            var methodInvocation = GetMockedMethodInvocation(invocation);
            var substitution = methodInvocation?.Reference?.Resolve().Substitution;

            return mockedMethod.Parameters.Select(p =>
            {
                if (substitution == null)
                {
                    return p.Type.GetPresentableName(CSharpLanguage.Instance);
                }

                return substitution.Apply(p.Type).GetPresentableName(CSharpLanguage.Instance);
            });
        }

        private IInvocationExpression GetMockedMethodInvocation(IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || !IsSetupMethod(invocationExpression))
            {
                return null;
            }

            var methodArguments = invocationExpression.ArgumentList.Arguments;

            if (methodArguments.Count != 1)
            {
                return null;
            }

            var lambdaExpression = methodArguments[0].Value as ILambdaExpression;

            return lambdaExpression?.BodyExpression as IInvocationExpression;
        }
    }
}