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
		private Mock<Test1.ITest> foo;
		
        [Test]
        public void METHOD({off})
        {
            var t = new{off} Te{off}st1(foo.Object{off});
        }
    }
}