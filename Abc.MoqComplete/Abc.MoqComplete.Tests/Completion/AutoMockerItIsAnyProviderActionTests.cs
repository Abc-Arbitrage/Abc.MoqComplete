using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
	[TestNetCore21("Moq.AutoMock/1.2.0.120")]
	public class AutoMockerItIsAnyProviderActionTests : CodeCompletionTestBase
	{
		protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;

		protected override string RelativeTestDataPath => "AutoMockerItIsAnyCompletion";

		[TestCase("itIsAnyCompletionFull")]
		[TestCase("itIsAnyCompletionParam")]
		[TestCase("itIsAnyCompletionGeneric")]
        [TestCase("itIsAnyCompletionOverload")]
        [TestCase("itIsAnyCompletionOverloadAndGenerics")]
		[TestCase("itIsAnyVerifyCompletionFull")]
		public void should_fill_with_it_isAny(string testSrc)
		{
			DoOneTest(testSrc);
		}
	}
}