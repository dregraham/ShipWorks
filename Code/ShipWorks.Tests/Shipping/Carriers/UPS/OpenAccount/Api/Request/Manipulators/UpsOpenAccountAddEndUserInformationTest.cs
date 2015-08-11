using System.Collections.Generic;
using Interapptive.Shared.Net;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators
{
    public class UpsOpenAccountAddEndUserInformationTest
    {
        private Mock<INetworkUtility> networkUtility;
        private UpsOpenAccountAddEndUserInformation testObject;
        private CarrierRequest request;
        OpenAccountRequest openAccountRequest;

        [TestInitialize]
        public void Initialize()
        {
            networkUtility = new Mock<INetworkUtility>();

            testObject = new UpsOpenAccountAddEndUserInformation(networkUtility.Object);


            openAccountRequest = new OpenAccountRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, openAccountRequest);

            request = mockRequest.Object;
        }

        [Fact]
        public void Manipulate_IPAddressIsSet_Test()
        {
            string ipAddress = "192.168.42.1";

            networkUtility.Setup(n => n.GetIPAddress()).Returns(ipAddress);

            testObject.Manipulate(request);

            Assert.AreEqual(ipAddress, openAccountRequest.EndUserInformation.EndUserIPAddress);
        }

        [Fact]
        [ExpectedException(typeof(UpsOpenAccountException))]
        public void Manipulate_ThrowsUpsOpenAccountException_Test()
        {
            networkUtility.Setup(n => n.GetIPAddress()).Throws(new NetworkException("oops"));

            testObject.Manipulate(request);
        }
    }
}