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
        [Test]
        public void METHOD()
        {
            var t = new Test1({caret});
        }
    }
}