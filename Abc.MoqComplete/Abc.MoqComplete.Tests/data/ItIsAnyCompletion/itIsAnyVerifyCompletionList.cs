// ${COMPLETE_ITEM:It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()}
using Moq;
using NUnit.Framework;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void Publish(long sequence);
        void Publish(long lo, long hi);
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
            Mock<ITestInterface> temp = new Mock<ITestInterface>();
			temp.Verify(x => x.Publish(it{caret}))
        }
    }
}