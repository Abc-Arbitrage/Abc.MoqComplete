using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    class Test1
    {
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

        [Test]
        public void METHOD()
        {
            var t = new {off}Test1{off}({off});
        }
    }
}