using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace Abc.MoqComplete.CompletionProvider
{
	[Language(typeof(CSharpLanguage))]
	public class AutoMockerReturnsMethodProvider : BaseReturnsMethodProvider<IAutoMockerMockedMethodProvider>
	{
    }
}