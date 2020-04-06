using Abc.MoqComplete.Services;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.CompletionProvider
{
	[Language(typeof(CSharpLanguage))]
	public class AutoMockerItIsAnyParameterProvider : BaseItIsAnyParameterProvider
	{
		/// <inheritdoc />
		protected override bool IsSetupMethod(IMoqMethodIdentifier identifier, IInvocationExpression methodInvocation)
		{
			return identifier.IsAutoMockerSetupMethod(methodInvocation);
		}
	}
}