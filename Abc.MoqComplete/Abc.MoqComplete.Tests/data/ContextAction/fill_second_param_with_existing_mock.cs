using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {
        public Test1(ITest test, ITest2 test2)
        {

        }
        
        public Test1(ITest test)
        {

        }

        public interface ITest
        {
            void Coco(ITest test, ITest test2, string test3);
        }
        public interface ITest2
        {
        }
    }

    [TestFixture]
    public class TestClass
    {
        private Mock<Test1.ITest> myTest;
        private Mock<Test1.ITest2> myTest2;

        [Test]
        public void METHOD()
        {
            myTest = new Mock<Test1.ITest>();
            myTest2 = new Mock<Test1.ITest2>();
            var t = new Test1(myTest.Object{caret});
        }
    }
}
