// ${COMPLETE_ITEM:It.IsAny<ITest2>(), It.IsAny<double>(), It.IsAny<string>()}
using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {       
        public interface ITest<T>
        {
            void Coco(ITest2 test, T test2, string test3);

        }
        public interface ITest2 : ITest<double>
        {
            
        }
    }

    [TestFixture]
    public class TestClass
    {

        [Test]
        public void METHOD()
        {
            var m = new Mock<Test1.ITest2>();
            m.Setup(x=>x.Coco(It{caret}));
        }
    }
}