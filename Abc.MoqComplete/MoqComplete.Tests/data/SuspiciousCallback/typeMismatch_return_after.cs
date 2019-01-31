using System;
using Moq;

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
            var mock = new Mock<ITestInterface>();
            var count = 0;
            mock.Setup(x => x.BuildSomething(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
				.Callback<int, string, string>((i, s, arg3) => count += i)
                .Returns(0);
            Console.WriteLine(count);
        }
    }
}