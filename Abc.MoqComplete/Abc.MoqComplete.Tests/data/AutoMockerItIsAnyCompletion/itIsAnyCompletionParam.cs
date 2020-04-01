// ${COMPLETE_ITEM:It.IsAny<int>()}
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void BuildSomething(int i, string toto, bool ok);
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
            var _mocker = new AutoMocker();
            temp.Setup<ITestInterface>(x => x.BuildSomething(It{caret}))
        }
    }
}