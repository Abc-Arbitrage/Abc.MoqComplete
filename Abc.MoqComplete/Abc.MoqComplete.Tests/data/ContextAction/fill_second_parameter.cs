using NUnit.Framework;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {
        public Test1(ITest test, ITest2 test2, ITest3 test3)
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
		
        public interface ITest3
        {
        }
    }

    [TestFixture]
    public class TestClass
    {
        private Mock<Test1.ITest> myTest;
        private Mock<Test1.ITest3> myTest3;

        [Test]
        public void METHOD()
        {
            myTest = new Mock<Test1.ITest>();
            myTest3 = new Mock<Test1.ITest3>();
            var t = new Test1(myTest.Object,  {caret}, myTest3.Object);
        }
    }
}