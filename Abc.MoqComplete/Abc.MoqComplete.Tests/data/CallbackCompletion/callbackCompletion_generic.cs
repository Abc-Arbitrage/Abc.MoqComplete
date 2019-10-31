// ${COMPLETE_ITEM:Callback<ITest2,double,string>((test,test2,test3) => {})}
using NUnit.Framework;
using Moq;

namespace ConsoleApp1.Tests
{
    public interface ITest<T, U>
    {
        int Coco(ITest2 test, T test2, U test3);

    }
    public interface ITest2 : ITest<double, string>
    {
        
    }

    [TestFixture]
    public class TestClass
    {

        [Test]
        public void METHOD()
        {
            var m = new Mock<ITest2>();
            m.Setup(x=>x.Coco(It.IsAny<ITest2>(), It.IsAny<double>(), It.IsAny<string>()))
             .{caret}

        }
    }
}