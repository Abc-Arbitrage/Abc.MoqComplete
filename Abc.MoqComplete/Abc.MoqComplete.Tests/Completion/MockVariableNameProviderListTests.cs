using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class MockVariableNameProviderListTests : CodeCompletionTestBase
    {
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
        protected override string RelativeTestDataPath => "MockVariableNameCompletion";

        [TestCase("compeleteFields")]
        [TestCase("compeleteMethodParameter")]
        public void should_fill_with_it_isAny(string testSrc) => DoOneTest(testSrc);
    }
}
