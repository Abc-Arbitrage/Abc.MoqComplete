using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using MoqComplete.CompletionProvider;
using NUnit.Framework;

namespace MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class ItIsAnyProviderActionTests : CodeCompletionTestBase
    {
        private ItIsAnyParameterProvider _itIsAnyProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "ItIsAnyCompletion";

        [SetUp]
        public void SetUp()
        {
            // /!\ Mandatory otherwise the completion is not done
            _itIsAnyProvider = new ItIsAnyParameterProvider();
        }

        [TestCase("itIsAnyCompletionFull")]
        [TestCase("itIsAnyCompletionParam")]
        [TestCase("itIsAnyVerifyCompletionFull")]
        public void should_fill_with_it_isAny(string testSrc) => DoOneTest(testSrc);
    }
}

