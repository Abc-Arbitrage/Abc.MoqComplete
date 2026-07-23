using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.16.1")]
    public class ItIsAnyProviderListTests : CodeCompletionTestBase
    {
#pragma warning disable CS0618 // Type or member is obsolete
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
#pragma warning restore CS0618 // Type or member is obsolete

        protected override string RelativeTestDataPath => "ItIsAnyCompletion";
        
        [TestCase("itIsAnyCompletionList")]
        [TestCase("itIsAnyVerifyCompletionList")]
        public void should_fill_with_overloads(string src) => DoOneTest(src);
    }
}

