using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void Do<T>(T item) where T : class;
    }
    
	class TheClass
    {

    }
	
    public class Test1
    {
        public void METHOD()
        {
            var temp = new Mock<ITestInterface>();
            var i = 0;
            temp.Setup(x => x.Do(It.IsAny<TheClass>())).Callback<int>(item => i++);
            Console.WriteLine(i);
        }
    }
}