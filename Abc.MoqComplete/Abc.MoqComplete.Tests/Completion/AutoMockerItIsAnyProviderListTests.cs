using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
	[TestNetCore21("Moq.AutoMock/1.2.0.120")]
	public class AutoMockerItIsAnyProviderListTests : CodeCompletionTestBase
	{
#pragma warning disable CS0618 // Type or member is obsolete
		protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
#pragma warning restore CS0618 // Type or member is obsolete

		protected override string RelativeTestDataPath => "AutoMockerItIsAnyCompletion";

		[TestCase("itIsAnyCompletionList")]
		public void should_fill_with_overloads(string src)
		{
			DoOneTest(src);
		}
	}
}