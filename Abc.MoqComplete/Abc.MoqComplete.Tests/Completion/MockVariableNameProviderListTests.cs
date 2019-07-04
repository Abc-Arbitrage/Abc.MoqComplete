using Abc.MoqComplete.CompletionProvider;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class MockVariableNameProviderListTests : CodeCompletionTestBase
    {
        private MockVariableNameProvider _returnsProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
        protected override string RelativeTestDataPath => "MockVariableNameCompletion";

        [SetUp]
        public void SetUp()
        {
            // /!\ Mandatory otherwise the completion is not done
            _returnsProvider = new MockVariableNameProvider();
        }

        [TestCase("compeleteFields")]
        [TestCase("compeleteMethodParameter")]
        public void should_fill_with_it_isAny(string testSrc) => DoOneTest(testSrc);
    }
}
