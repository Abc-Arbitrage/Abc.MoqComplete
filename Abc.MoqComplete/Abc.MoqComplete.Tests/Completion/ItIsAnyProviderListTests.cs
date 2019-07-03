using Abc.MoqComplete.CompletionProvider;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.Completion
{
    [TestNetCore21("Moq/4.10.1")]
    public class ItIsAnyProviderListTests : CodeCompletionTestBase
    {
        private ItIsAnyParameterProvider _itIsAnyProvider;
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
        protected override string RelativeTestDataPath => "ItIsAnyCompletion";

        [SetUp]
        public void SetUp()
        {
            // /!\ Mandatory otherwise the completion is not done
            _itIsAnyProvider = new ItIsAnyParameterProvider();
        }
        
        [TestCase("itIsAnyCompletionList")]
        public void should_fill_with_overloads(string src) => DoOneTest(src);
    }
}

