using System;
using Moq;
using Moq.AutoMock;

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
            var _mocker = new AutoMocker();
            var i = 0;
            _mocker.Setup<ITestInterface>(x => x.Do(It.IsAny<TheClass>())).Callback<int>(_ => i++);
            Console.WriteLine(i);
        }
    }
}