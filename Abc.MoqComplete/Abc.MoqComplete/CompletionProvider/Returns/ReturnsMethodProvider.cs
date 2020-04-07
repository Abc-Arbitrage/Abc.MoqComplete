using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace Abc.MoqComplete.CompletionProvider.Returns
{
	[Language(typeof(CSharpLanguage))]
	public class ReturnsMethodProvider : BaseReturnsMethodProvider<IMockedMethodProvider>
	{
    }
}