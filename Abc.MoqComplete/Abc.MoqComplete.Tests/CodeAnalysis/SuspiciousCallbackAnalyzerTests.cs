using Abc.MoqComplete.CodeAnalysis;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.CodeAnalysis
{
    [TestNetCore21("Moq/4.10.1")]
    public class SuspiciousCallbackAnalyzerTests : CSharpHighlightingTestBase
    {
        protected override string RelativeTestDataPath => "SuspiciousCallback";
        
        [TestCase("typeMismatch_return_before")]
        [TestCase("typeCountMismatch_return_before")]
        [TestCase("typeMismatch_return_after")]
        [TestCase("typeCountMismatch_return_after")]
        [TestCase("typeMismatch_generic")]
        public void should_detect_suspicious_callback(string testSrc) => DoOneTest(testSrc);

        [TestCase("no_types")]
        [TestCase("generic_types")]
        [TestCase("implementation_types")]
        public void should_not_detect_suspicious_callback(string testSrc) => DoOneTest(testSrc);
    }
}
