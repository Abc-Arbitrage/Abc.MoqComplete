using Abc.MoqComplete.ContextActions.FillWithMock;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.16.1")]
    public class FillParamWithMockLocalVariableContextActionTests : ContextActionBypassIsAvailable<FillParamWithMockLocalVariableContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("local_variable_fill_first_param_with_mock")]
        [TestCase("local_variable_fill_second_param_with_mock")]
        [TestCase("local_variable_fill_second_parameter")]
        public void should_test_execution(string name) => DoOneTest(name);
    }
}