using System.Linq;
using Abc.MoqComplete.ContextActions.Services;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction.Services
{
    [TestFixture]
    public class ParameterProviderTests
    {
        private CsharpMemberProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new CsharpMemberProvider();
        }

        [Test]
        public void should_parse_generic_parameters()
        {
            var constructorString =
                "CSharpConstructor:CurrencyChoiceHasSignificantSpreadRequirement(Func<CurrencyChoiceCorpAction,IValueWithSource>,ICurrencyAmountConverter,ICorporateActionRepository)";
            var expected = new[] { "Func<CurrencyChoiceCorpAction,IValueWithSource>", "ICurrencyAmountConverter", "ICorporateActionRepository" };

            var parameters = _provider.GetConstructorParameters(constructorString).ToArray();

            Assert.That(parameters, Is.EquivalentTo(expected));
        }

        [Test]
        public void should_parse_non_generic_parameters()
        {
            var constructorString =
                "CSharpConstructor:TestConstructor(ICurrencyAmountConverter,ICorporateActionRepository)";
            var expected = new[] { "ICurrencyAmountConverter", "ICorporateActionRepository" };

            var parameters = _provider.GetConstructorParameters(constructorString).ToArray();

            Assert.That(parameters, Is.EquivalentTo(expected));
        }
    }
}
