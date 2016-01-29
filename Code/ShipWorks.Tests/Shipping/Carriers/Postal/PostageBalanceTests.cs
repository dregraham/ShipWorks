using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class PostageBalanceTests
    {
        private PostageBalance testObject;
        private Mock<IPostageWebClient> postageWebClient;
        private Mock<ITangoWebClient> tangoWebClient;
        private const string accountIdentifier = "blahblahblah";
        private const decimal balance = (decimal)42.42;

        public PostageBalanceTests()
        {
            postageWebClient = new Mock<IPostageWebClient>();
            postageWebClient.Setup(p => p.GetBalance()).Returns(balance);
            postageWebClient.Setup(p => p.Purchase(It.IsAny<decimal>()));
            postageWebClient.Setup(p => p.ShipmentTypeCode).Returns(ShipmentTypeCode.Express1Endicia);
            postageWebClient.Setup(p => p.AccountIdentifier).Returns(accountIdentifier);

            tangoWebClient = new Mock<ITangoWebClient>();
            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>()));

            testObject = new PostageBalance(postageWebClient.Object, tangoWebClient.Object);
        }

        [Fact]
        public void Purchase_CallsPostageWebClientGetBalance()
        {
            testObject.Purchase(5);

            postageWebClient.Verify(p => p.GetBalance(), Times.Once);
        }

        [Fact]
        public void Purchase_CallsPurchaseWithAmount()
        {
            testObject.Purchase(6);

            postageWebClient.Verify(p => p.Purchase(6), Times.Once);
        }

        [Fact]
        public void Purchase_LogsPostageEventToTango()
        {
            Task result = testObject.Purchase(7);
            result.Wait(TimeSpan.FromSeconds(5));

            tangoWebClient.Verify(t => t.LogPostageEvent(balance, 7, ShipmentTypeCode.Express1Endicia, accountIdentifier), Times.Once);
        }

        [Fact]
        public void Purchase_PurchasePostageDespiteTangoError()
        {
            testObject.Purchase(7);

            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>())).Throws(new TangoException());

            postageWebClient.Verify(p => p.Purchase(7), Times.Once);
        }

        [Fact]
        public void Value_LogsPostageEventToTango()
        {
            decimal testValue = testObject.Value;

            int index = 0;

            while (true)
            {
                try
                {
                    index++;
                    tangoWebClient.Verify(t => t.LogPostageEvent(balance, 0, ShipmentTypeCode.Express1Endicia, accountIdentifier), Times.Once);
                    return;
                }
                catch (Exception)
                {
                    if (index > 3)
                    {
                        throw;   
                    }
                    
                    Thread.Sleep(200);
                }
            }
            
        }

        [Fact]
        public void Value_ValueIsCorrect()
        {
            Assert.Equal(balance, testObject.Value);
        }

        [Fact]
        public void Value_ValueIsCorrectDespiteLoggingError()
        {
            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>())).Throws(new TangoException());

            Assert.Equal(balance, testObject.Value);
        }
    }
}