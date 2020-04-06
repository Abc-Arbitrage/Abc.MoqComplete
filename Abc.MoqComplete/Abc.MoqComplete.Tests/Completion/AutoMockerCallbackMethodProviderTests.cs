using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
	[TestNetCore21("Moq.AutoMock/1.2.0.120")]
	public class AutoMockerCallbackMethodProviderTests: CodeCompletionTestBase
	{
		protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
		protected override string RelativeTestDataPath => "AutoMockerCallbackCompletion";

		[TestCase("callbackCompletion")]
		[TestCase("callbackCompletion_afterReturn")]
		[TestCase("callbackCompletion_generic")]
		public void should_fill_with_callback(string testSrc) => DoOneTest(testSrc);
	}
}