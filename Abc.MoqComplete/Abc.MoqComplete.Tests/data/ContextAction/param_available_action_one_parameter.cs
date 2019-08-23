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
    {{off}
        private Mock<ITest> myTest;{off}

        [Test]
        public void METHOD()
        {
            myTest = new Mock<ITest>();{off}
            var t = new Test1(myTest.Object{on},{on});
        }
    }
}