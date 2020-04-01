using System;
using Moq;
using Moq.AutoMock;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void Do<T>(T item) where T : struct;
    }
    
    public class Test1
    {
        public void METHOD()
        {
            var _mocker = new AutoMocker();
            var i = 0;
            _mocker.Setup<ITestInterface>(x => x.Do(It.IsAny<int>())).Callback<int>(item => i++);
            Console.WriteLine(i);
        }
    }
}