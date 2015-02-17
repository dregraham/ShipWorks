using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    [TestClass]
    public class PostageBalanceTests
    {
        private PostageBalance testObject;
        private Mock<IPostageWebClient> postageWebClient;
        private Mock<ITangoWebClient> tangoWebClient;
        private const string accountIdentifier = "blahblahblah";
        private const decimal balance = (decimal)42.42;

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void Purchase_CallsPostageWebClientGetBalance_Test()
        {
            testObject.Purchase(5);

            postageWebClient.Verify(p => p.GetBalance(), Times.Once);
        }

        [TestMethod]
        public void Purchase_CallsPurchaseWithAmount_Test()
        {
            testObject.Purchase(6);

            postageWebClient.Verify(p => p.Purchase(6), Times.Once);
        }

        [TestMethod]
        public void Purchase_LogsPostageEventToTango_Test()
        {
            Task result = testObject.Purchase(7);
            result.Wait(TimeSpan.FromSeconds(5));

            tangoWebClient.Verify(t => t.LogPostageEvent(balance, 7, ShipmentTypeCode.Express1Endicia, accountIdentifier), Times.Once);
        }

        [TestMethod]
        public void Purchase_PurchasePostageDespiteTangoError_Test()
        {
            testObject.Purchase(7);

            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>())).Throws(new TangoException());

            postageWebClient.Verify(p => p.Purchase(7), Times.Once);
        }

        [TestMethod]
        public void Value_LogsPostageEventToTango_Test()
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

        [TestMethod]
        public void Value_ValueIsCorrect_Test()
        {
            Assert.AreEqual(balance, testObject.Value);
        }

        [TestMethod]
        public void Value_ValueIsCorrectDespiteLoggingError_Test()
        {
            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>())).Throws(new TangoException());

            Assert.AreEqual(balance, testObject.Value);
        }
    }
}