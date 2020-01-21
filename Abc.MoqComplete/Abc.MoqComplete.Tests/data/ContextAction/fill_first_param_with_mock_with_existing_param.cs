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
        private Mock<Test1.ITest2> myTest2;
        private Mock<Test1.ITest3> myTest3;

        [Test]
        public void METHOD()
        {
            myTest2 = new Mock<Test1.ITest2>();
            myTest3 = new Mock<Test1.ITest3>();
            var t = new Test1({caret}, myTest2.Object, myTest3.Object );
        }
    }
}