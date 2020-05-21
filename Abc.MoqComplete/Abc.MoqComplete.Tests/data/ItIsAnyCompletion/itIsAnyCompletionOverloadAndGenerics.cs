// ${COMPLETE_ITEM:It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()}
using Moq;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface<T>
    {
        decimal GetGrossAmountInEuro(decimal price, T quantity, int securityId, int currencyId, decimal spotRateInEuro);
        decimal GetGrossAmountInEuro(string message);
        decimal GetGrossAmountInEuro(int trade);
        
        void Build(int temp);
    }

    public class Test1
    {
        public void METHOD()
        {
            var mock = new Mock<ITestInterface<decimal>>();
            mock.Setup(x=>x.GetGrossAmountInEuro(it{caret}))
        }
    }
}