using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using MoqComplete.ContextActions;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.10.1")]
    public class FillWithMockContextActionTests : ContextActionExecuteTestBase<FillWithMockContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("fill_with_mock")]
        public void should_test_availability(string name)
        {
            DoOneTest(name);
        }
    }
}
