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
        public Test1(ITest test, ITest2 test2, int test3)
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

        [Test]
        public void METHOD()
        {
            myTest = new Mock<ITest>();
            myTest2 = new Mock<ITest2>();
            var t = new Test1(myTest.Object{on}, myTest2.Object{on},{on});
        }
    }
}