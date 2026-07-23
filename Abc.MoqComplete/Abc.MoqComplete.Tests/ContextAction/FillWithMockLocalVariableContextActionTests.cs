using Abc.MoqComplete.ContextActions.FillWithMock;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.16.1")]
    public class FillWithMockLocalVariableContextActionTests : ContextActionBypassIsAvailable<FillWithMockLocalVariableContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("local_variable_fill_with_mock")]
        [TestCase("local_variable_fill_with_generics")]
        public void should_test_execution(string name) => DoOneTest(name);
    }
}
