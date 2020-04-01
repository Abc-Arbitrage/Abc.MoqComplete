using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
	[TestNetCore21("Moq.AutoMock/1.2.0.120")]
	public class AutoMockerItIsAnyProviderListTests : CodeCompletionTestBase
	{
		protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;

		protected override string RelativeTestDataPath => "ItIsAnyCompletion";

		[TestCase("itIsAnyCompletionList")]
		public void should_fill_with_overloads(string src)
		{
			DoOneTest(src);
		}
	}
}