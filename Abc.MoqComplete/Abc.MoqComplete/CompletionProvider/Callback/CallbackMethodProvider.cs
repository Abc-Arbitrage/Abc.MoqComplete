using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace Abc.MoqComplete.CompletionProvider.Callback
{
	[Language(typeof(CSharpLanguage))]
	public class CallbackMethodProvider : BaseCallbackMethodProvider<IMockedMethodProvider>
	{
    }
}