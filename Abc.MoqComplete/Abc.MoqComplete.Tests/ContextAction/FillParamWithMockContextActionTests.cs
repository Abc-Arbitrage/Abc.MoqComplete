using Abc.MoqComplete.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    [TestNetCore21("Moq/4.10.1")]
    public class FillParamWithMockContextActionTests : ContextActionExecuteTestBase<FillParamWithMockContextAction>
    {
        protected override string RelativeTestDataPath => "ContextAction";
        protected override string ExtraPath => "";

        [TestCase("fill_first_param_with_mock")]
        [TestCase("fill_first_param_with_existing_mock")]
        [TestCase("fill_second_param_with_mock")]
        [TestCase("fill_second_param_with_existing_mock")]
        public void should_test_execution(string name) => DoOneTest(name);
    }
}