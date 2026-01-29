using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;

namespace Abc.MoqComplete.Services
{
	[SolutionComponent(Instantiation.ContainerAsyncPrimaryThread)]
	public class MoqMethodIdentifier : IMoqMethodIdentifier
	{
		/// <inheritdoc />
		public bool IsAutoMockerSetupMethod(IInvocationExpression invocationExpression)
			=> IsMethod(invocationExpression,
				declaredElement => IsMethodStartingWithString(declaredElement,
					"Method:Moq.AutoMock.AutoMocker.Setup"));

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

		public bool IsMoqReturnAsyncMethod(IInvocationExpression invocationExpression)
			=> IsMethod(invocationExpression,
				declaredElement => IsMethodStartingWithString(declaredElement,
					"Method:Moq.GeneratedReturnsExtensions.ReturnsAsync(Moq.Language.IReturns`2",
					"Method:Moq.ReturnsExtensions.ReturnsAsync(Moq.Language.IReturns`2"));

		public bool IsMoqCallbackMethod(IInvocationExpression expression)
			=> IsMethod(expression,
				declaredElement => IsMethodStartingWithString(declaredElement,
					"Method:Moq.Language.ICallback.Callback(System.Action",
					"Method:Moq.Language.ICallback`2.Callback(System.Action"));

		private bool IsMethod([CanBeNull] IInvocationExpression invocationExpression,
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

		private bool IsMethodString([CanBeNull] IDeclaredElement declaredElement, params string[] methodName)
		{
			var declaredElementAsString = declaredElement.ConvertToString();

			return methodName.Contains(declaredElementAsString);
		}

		private bool IsMethodStartingWithString([CanBeNull] IDeclaredElement declaredElement,
												params string[] methodName)
		{
			var methodString = declaredElement.ConvertToString();

			return methodString != null && methodName.Any(m => methodString.StartsWith(m));
		}
	}
}