using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class PostageBalanceTests
    {
        private AutoMock mock;
        private PostageBalance testObject;

        public PostageBalanceTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<PostageBalance>();
        }

        [Fact]
        public void Purchase_CallsPostageWebClientGetBalance()
        {
            testObject.Purchase(5);

            mock.Mock<IPostageWebClient>().Verify(p => p.GetBalance(), Times.Once);
        }

        [Fact]
        public void Purchase_CallsPurchaseWithAmount()
        {
            testObject.Purchase(6);

            mock.Mock<IPostageWebClient>().Verify(p => p.Purchase(6), Times.Once);
        }

        [Fact]
        public void Value_ValueIsCorrect()
        {
            mock.Mock<IPostageWebClient>().Setup(x => x.GetBalance()).Returns(42.42M);

            Assert.Equal(42.42M, testObject.Value);
        }
    }
}