using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    class Test1
    {
        public Test1(ITest test)
        {

        }
        public Test1(ITest test, ITest2 test2)
        {

        }
        public Test1(ITest test, ITest2 test2, ITest test3)
        {

        }
    }

    public interface ITest
    {
        void Coco(ITest test, ITest test2, int test3);
    }
    public interface ITest2
    {
    }

    [TestFixture]
    public class TestClass
    {
        private Mock<ITest> myTest;
        private Mock<ITest2> myTest2;
        private Mock<ITest> myTest3;

        [Test]
        public void METHOD()
        {
            myTest = new Mock<ITest>();
            myTest2 = new Mock<ITest2>();
            myTest3 = new Mock<ITest>();
            var t = new Test1{off}({off}myTest.Object{off},{off} myTest2.Object{off},{off} myTest3.Object{off});
        }
    }
}