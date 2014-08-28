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

        [TestInitialize]
        public void Initialize()
        {
            postageWebClient = new Mock<IPostageWebClient>();
            postageWebClient.Setup(p => p.GetBalance()).Returns(42.42);
            postageWebClient.Setup(p => p.Purchase(It.IsAny<double>()));
            postageWebClient.Setup(p => p.ShipmentTypeCode).Returns(ShipmentTypeCode.Express1Endicia);
            postageWebClient.Setup(p => p.AccountIdentifier).Returns(accountIdentifier);

            tangoWebClient = new Mock<ITangoWebClient>();
            tangoWebClient.Setup(t => t.LogPostageEvent(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<string>()));

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
            testObject.Purchase(7);

            tangoWebClient.Verify(t => t.LogPostageEvent(42.42, 7, ShipmentTypeCode.Express1Endicia, accountIdentifier), Times.Once);
        }

        [TestMethod]
        public void Value_LogsPostageEventToTango_Test()
        {
            double balance = testObject.Value;

            tangoWebClient.Verify(t => t.LogPostageEvent(42.42, 0, ShipmentTypeCode.Express1Endicia, accountIdentifier), Times.Once);
        }

        [TestMethod]
        public void Value_ValueIsCorrect_Test()
        {
            double balance = testObject.Value;

            Assert.AreEqual(42.42, balance);
        }
    }
}