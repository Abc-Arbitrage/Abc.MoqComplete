// ${COMPLETE_ITEM:It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()}
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void BuildSomething(int theInt, string theString, bool theBool);
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
			var _mocker = new AutoMocker();
            _mocker.Setup<ITestInterface>(x => x.BuildSomething(It{caret}));
        }
    }
}