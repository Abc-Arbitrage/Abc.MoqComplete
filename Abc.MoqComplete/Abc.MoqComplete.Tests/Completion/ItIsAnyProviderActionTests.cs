using Abc.MoqComplete.CompletionProvider;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class ItIsAnyProviderActionTests : CodeCompletionTestBase
    {
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.Action;
        protected override string RelativeTestDataPath => "ItIsAnyCompletion";

        [TestCase("itIsAnyCompletionFull")]
        [TestCase("itIsAnyCompletionParam")]
        [TestCase("itIsAnyCompletionGeneric")]
        [TestCase("itIsAnyCompletionOverload")]
        [TestCase("itIsAnyCompletionOverloadAndGenerics")]
        [TestCase("itIsAnyVerifyCompletionFull")]
        public void should_fill_with_it_isAny(string testSrc) => DoOneTest(testSrc);
    }
}

