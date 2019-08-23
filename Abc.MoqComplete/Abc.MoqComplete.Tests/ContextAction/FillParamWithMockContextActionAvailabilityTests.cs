using Abc.MoqComplete.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.10.1")]
    public class FillParamWithMockContextActionAvailabilityTests : ContextActionAvailabilityTestBase<FillParamWithMockContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("param_available_action_empty_constructor")]
        [TestCase("param_available_action_one_parameter")]
        [TestCase("param_available_action_two_parameter")]
        [TestCase("param_unavailable_action_three_parameter")]
        [TestCase("param_unavailable_action_constructor_with_no_parameter")]
        public void should_test_availability(string name)
        {
            DoOneTest(name);
        }
    }
}
