﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Request
{
    [TestClass]
    public class UpsOpenAccountRequestTest
    {
        private UpsOpenAccountRequest testObject;

        private Mock<IUpsServiceGateway> upsService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;


        [TestInitialize]
        public void Initialize()
        {
            upsService = new Mock<IUpsServiceGateway>();
            upsService.Setup(s => s.OpenAccount(It.IsAny<OpenAccountRequest>())).Returns(new OpenAccountResponse());

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateSubscriptionResponse(It.IsAny<OpenAccountResponse>(), It.IsAny<CarrierRequest>())).Returns(carrierResponse.Object);

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            testObject = new UpsOpenAccountRequest(upsService.Object, responseFactory.Object, new OpenAccountRequest());
        }

        [TestMethod]
        public void CarrierAccountEntity_IsNull_Test()
        {
            Assert.IsNull(testObject.CarrierAccountEntity as UpsAccountEntity);
        }        

        [TestMethod]
        public void Submit_DelegatesToUpsOpenAccountService_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify that the subscribe method was called using the test object's native request
            upsService.Verify(s => s.OpenAccount(testObject.NativeRequest as OpenAccountRequest), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToResponseFactory_WhenCreatingResponse_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            testObject.Submit();

            // Verify the response is created via the response factory using the test object
            responseFactory.Verify(f => f.CreateSubscriptionResponse(It.IsAny<OpenAccountResponse>(), testObject), Times.Once());
        }

    }
}
