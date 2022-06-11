using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class CallbackMethodProviderTests: CodeCompletionTestBase
    {
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "CallbackCompletion";

        [TestCase("callbackCompletion")]
        [TestCase("callbackCompletion_afterReturn")]
        [TestCase("callbackCompletion_afterReturnAsync")]
        [TestCase("callbackCompletion_afterReturnAsyncMultiple")]
        [TestCase("callbackCompletion_afterReturnAsyncMultiple2")]
        [TestCase("callbackCompletion_generic")]
        public void should_fill_with_callback(string testSrc) => DoOneTest(testSrc);
    }
}
