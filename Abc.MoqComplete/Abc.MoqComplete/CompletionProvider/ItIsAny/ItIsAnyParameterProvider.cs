using Abc.MoqComplete.Services;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CompletionProvider.ItIsAny
{
	[Language(typeof(CSharpLanguage))]
	public sealed class ItIsAnyParameterProvider : BaseItIsAnyParameterProvider
	{
        protected override bool IsSetupMethod(IMoqMethodIdentifier identifier, IInvocationExpression methodInvocation)
		{
			return identifier.IsMoqSetupMethod(methodInvocation);
		}
	}
}