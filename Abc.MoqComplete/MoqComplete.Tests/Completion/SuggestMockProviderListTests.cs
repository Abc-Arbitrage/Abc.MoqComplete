using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using MoqComplete.CompletionProvider;
using NUnit.Framework;

namespace MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class SuggestMockProviderListTests : CodeCompletionTestBase
    {
        private SuggestMockProvider _suggestMockProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
        protected override string RelativeTestDataPath => "MockCompletion";

        [SetUp]
        public void SetUp()
        {
            _suggestMockProvider = new SuggestMockProvider();
        }

        [TestCase("mockCompletionList")]
        public void should_list_mock(string testSrc) => DoOneTest(testSrc);
    }
}
