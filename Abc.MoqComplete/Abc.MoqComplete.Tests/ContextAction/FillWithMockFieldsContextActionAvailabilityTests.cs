using Abc.MoqComplete.ContextActions.FillWithMock;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.10.1")]
    public class FillWithMockFieldsContextActionAvailabilityTests : ContextActionAvailabilityTestBase<FillWithMockFieldsContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("fill_with_mock_fields_available_action")]
        [TestCase("fill_with_mock_fields_unavailable_action")]
        public void should_test_availability(string name)
        {
            DoOneTest(name);
        }
    }
}
