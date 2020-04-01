using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.Services
{
	[SolutionComponent]
	public class AutoMockerMethodIdentifier : BaseMethodIdentifier, IAutoMockerMethodIdentifier
	{
		/// <inheritdoc />
		public bool IsAutoMockerSetupMethod(IInvocationExpression invocationExpression)
		{
			return IsMethod(invocationExpression,
				declaredElement => IsMethodString(declaredElement,
					"Method:Moq.AutoMock.AutoMocker.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> TService, TResult -> TReturn]] setup)",
					"Method:Moq.AutoMock.AutoMocker.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> TService]] setup)"));
		}

		/// <inheritdoc />
		public bool IsAutoMockerVerifyMethod(IInvocationExpression invocationExpression)
		{
			return IsMethod(invocationExpression,
				declaredElement => IsMethodString(declaredElement,
					"Method:Moq.Mock`1.Verify(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> T]] expression)",
					"Method:Moq.Mock`1.Verify(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> T, TResult -> TResult]] expression)"));
		}

		/// <inheritdoc />
		public bool IsAutoMockerReturnMethod(IInvocationExpression invocationExpression)
		{
			return IsMethod(invocationExpression,
				declaredElement => IsMethodStartingWithString(declaredElement,
					"Method:Moq.Language.IReturns`2.Returns",
					"Method:Moq.Language.IReturns`1.Returns"));
		}

		/// <inheritdoc />
		public bool IsAutoMockerCallbackMethod(IInvocationExpression invocationExpression)
		{
			return IsMethod(invocationExpression,
				declaredElement => IsMethodStartingWithString(declaredElement,
					"Method:Moq.Language.ICallback.Callback(System.Action",
					"Method:Moq.Language.ICallback`2.Callback(System.Action"));
		}
	}
}