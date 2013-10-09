﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Void.Request
{
    [TestClass]
    public class FedExVoidRequestTest
    {
        private FedExVoidRequest testObject;
        
        private Mock<IFedExServiceGateway> fedExService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;
        
        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;
        
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity();
            account = new FedExAccountEntity {AccountNumber = "1234", MeterNumber = "45453"};

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.Void(It.IsAny<DeleteShipmentRequest>())).Returns(new ShipmentReply());

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateShipResponse(It.IsAny<ShipmentReply>(), It.IsAny<CarrierRequest>(), shipmentEntity)).Returns(carrierResponse.Object);

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            // Create some mocked manipulators for testing purposes
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                firstManipulator.Object,
                secondManipulator.Object
            };


            testObject = new FedExVoidRequest(manipulators, shipmentEntity, fedExService.Object, responseFactory.Object, account);
        }


        [TestMethod]
        public void CarrierAccountEntity_IsNotNull_Test()
        {
            Assert.IsNotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [TestMethod]
        public void CarrierAccountEntity_ReturnsAccountProvidedInConstructor_Test()
        {
            Assert.AreEqual(account, testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [TestMethod]
        public void Submit_DelegatesToManipulators_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToFedExService_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify that the ship method was called using the test object's native request
            fedExService.Verify(s => s.Void(testObject.NativeRequest as DeleteShipmentRequest), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToResponseFactory_WhenCreatingVoidResponse_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the ship response is created via the response factory using the test object's shipment entity
            responseFactory.Verify(f => f.CreateVoidResponse(It.IsAny<ShipmentReply>(), testObject), Times.Once());
        }
    }
}
