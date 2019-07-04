using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {
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
        public void Method(Mock<Test1.ITest> {caret}
    }
}