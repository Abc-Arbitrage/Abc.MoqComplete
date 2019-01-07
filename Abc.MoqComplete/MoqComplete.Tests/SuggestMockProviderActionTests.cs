using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using MoqComplete.CompletionProvider;
using NUnit.Framework;

namespace MoqComplete.Tests
{
    [TestNetCore21("Moq/4.10.1")]
    public class SuggestMockProviderActionTests : CodeCompletionTestBase
    {
        private SuggestMockProvider _suggestMockProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "MockCompletion";

        [SetUp]
        public void SetUp()
        {
            _suggestMockProvider = new SuggestMockProvider();
        }

        [TestCase("mockCompletionParam")]
        [TestCase("mockSuggestionParam")]
        public void should_fill_with_mock_object(string testSrc) => DoOneTest(testSrc);
    }
}
