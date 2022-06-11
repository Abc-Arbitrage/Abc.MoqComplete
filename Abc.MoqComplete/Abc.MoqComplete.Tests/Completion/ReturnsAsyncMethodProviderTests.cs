using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.18.1")]
    public class ReturnsAsyncMethodProviderTests: CodeCompletionTestBase
    {
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "ReturnsAsyncCompletion";

        [TestCase("returnsAsyncCompletion")]
        [TestCase("returnsAsyncCompletion_afterCallback")]
        [TestCase("returnsAsyncCompletion_generic")]
        [TestCase("ValueTask_returnsAsyncCompletion")]
        [TestCase("ValueTask_returnsAsyncCompletion_afterCallback")]
        [TestCase("ValueTask_returnsAsyncCompletion_generic")]
        public void should_fill_with_callback(string testSrc) => DoOneTest(testSrc);
    }
}
