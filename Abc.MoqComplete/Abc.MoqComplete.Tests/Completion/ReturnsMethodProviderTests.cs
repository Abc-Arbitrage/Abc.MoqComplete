using Abc.MoqComplete.CompletionProvider;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class ReturnsMethodProviderTests: CodeCompletionTestBase
    {
        private CallbackMethodProvider _returnsProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "ReturnsCompletion";

        [SetUp]
        public void SetUp()
        {
            // /!\ Mandatory otherwise the completion is not done
            _returnsProvider = new CallbackMethodProvider();
        }

        [TestCase("returnsCompletion")]
        [TestCase("returnsCompletion_afterCallback")]
        [TestCase("returnsCompletion_generic")]
        public void should_fill_with_callback(string testSrc) => DoOneTest(testSrc);
    }
}
