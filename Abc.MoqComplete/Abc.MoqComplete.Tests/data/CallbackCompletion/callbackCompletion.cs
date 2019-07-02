// ${COMPLETE_ITEM:Callback<int,string,bool>((i,toto,ok) => {})}
using Moq;
using NUnit.Framework;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void BuildSomething(int i, string toto, bool ok);
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
            Mock<ITestInterface> temp = new Mock<ITestInterface>();
            temp.Setup(x => x.BuildSomething(It.IsAny<int>(),It.IsAny<string>(), It.IsAny<bool>()))
                 .c{caret}
        }
    }
}