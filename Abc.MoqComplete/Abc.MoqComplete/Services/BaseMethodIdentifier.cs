using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;

namespace Abc.MoqComplete.Services
{
    public abstract class BaseMethodIdentifier
    {
        protected bool IsMethod([CanBeNull] IInvocationExpression invocationExpression,
            Func<IDeclaredElement, bool> methodFilter)
        {
            if (invocationExpression?.Reference == null)
            {
                return false;
            }

            var resolveResult = invocationExpression.Reference.Resolve();
            if (resolveResult.ResolveErrorType == ResolveErrorType.MULTIPLE_CANDIDATES)
            {
                return resolveResult.Result.Candidates.Any(methodFilter);
            }

            return methodFilter(resolveResult.DeclaredElement);
        }

        protected bool IsMethodString([CanBeNull] IDeclaredElement declaredElement, params string[] methodName)
        {
            var declaredElementAsString = declaredElement.ConvertToString();
            return methodName.Contains(declaredElementAsString);
        }

        protected bool IsMethodStartingWithString([CanBeNull] IDeclaredElement declaredElement,
            params string[] methodName)
        {
            var methodString = declaredElement.ConvertToString();
            return methodString != null && methodName.Any(m => methodString.StartsWith(m));
        }
    }
}