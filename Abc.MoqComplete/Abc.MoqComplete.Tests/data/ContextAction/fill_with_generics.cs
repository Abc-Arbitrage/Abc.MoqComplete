using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {
        public Test1(Func<ITest, ITest2> test, ITest2 test2)
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