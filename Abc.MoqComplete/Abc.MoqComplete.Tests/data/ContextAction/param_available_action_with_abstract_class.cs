using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    class Test1
    {
        public Test1(AbstractTest test)
        {

        }
    }

    public abstract class AbstractTest
    {
        abstract void M(ITest test, ITest test2, int test3);
    }

    [TestFixture]
    public class TestClass
    {
        [Test]
        public void METHOD()
        {
            var t = new Test1({on});
        }
    }
}