// ${COMPLETE_ITEM:ReturnsAsync((int i, string toto, bool ok) => )}
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        Task BuildSomething(int i, string toto, bool ok);
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
            Mock<ITestInterface> temp = new Mock<ITestInterface>();
            temp.Setup(x => x.BuildSomething(It.IsAny<int>(),It.IsAny<string>(), It.IsAny<bool>()))
                 .R{caret}
        }
    }
}