using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public interface IGeneric<T>
    {

    }

    public interface ITest2
    {
    }

    public class Test2
    {
        public Test2(IGeneric<int> test, ITest2 test2)
        {
        }   
    }

    [TestFixture]
    public class TestClass
    {

        [Test]
        public void METHOD()
        {
            var t = new Test2({caret});
        }
    }
}
