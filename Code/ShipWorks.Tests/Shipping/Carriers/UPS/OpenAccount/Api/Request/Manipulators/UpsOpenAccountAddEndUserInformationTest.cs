﻿using System.Collections.Generic;
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

        public UpsOpenAccountAddEndUserInformationTest()
        {
            networkUtility = new Mock<INetworkUtility>();

            testObject = new UpsOpenAccountAddEndUserInformation(networkUtility.Object);


            openAccountRequest = new OpenAccountRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, openAccountRequest);

            request = mockRequest.Object;
        }

        [Fact]
        public void Manipulate_IPAddressIsSet()
        {
            string ipAddress = "192.168.42.1";

            networkUtility.Setup(n => n.GetIPAddress()).Returns(ipAddress);

            testObject.Manipulate(request);

            Assert.Equal(ipAddress, openAccountRequest.EndUserInformation.EndUserIPAddress);
        }

        [Fact]
        public void Manipulate_ThrowsUpsOpenAccountException()
        {
            networkUtility.Setup(n => n.GetIPAddress()).Throws(new NetworkException("oops"));

            Assert.Throws<UpsOpenAccountException>(() => testObject.Manipulate(request));
        }
    }
}