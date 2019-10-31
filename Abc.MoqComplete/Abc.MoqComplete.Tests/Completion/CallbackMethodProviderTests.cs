using Abc.MoqComplete.CompletionProvider;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class CallbackMethodProviderTests: CodeCompletionTestBase
    {
        private CallbackMethodProvider _callbackProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "CallbackCompletion";

        [SetUp]
        public void SetUp()
        {
            // /!\ Mandatory otherwise the completion is not done
            _callbackProvider = new CallbackMethodProvider();
        }

        [TestCase("callbackCompletion")]
        [TestCase("callbackCompletion_afterReturn")]
        [TestCase("callbackCompletion_generic")]
        public void should_fill_with_callback(string testSrc) => DoOneTest(testSrc);
    }
}
