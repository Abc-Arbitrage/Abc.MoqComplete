using System;
using Moq;
using Moq.AutoMock;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        int BuildSomething(int theInt, string theString, bool theBool);
    }

    public class Test1
    {
       public void METHOD()
        {
            var _mocker = new AutoMocker();
            var count = 0;
            _mocker.Setup<ITestInterface, int>(x => x.BuildSomething(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
				.Callback<int, string>((i, s, arg3) => count += i)
                .Returns(0);
            Console.WriteLine(count);
        }
    }
}