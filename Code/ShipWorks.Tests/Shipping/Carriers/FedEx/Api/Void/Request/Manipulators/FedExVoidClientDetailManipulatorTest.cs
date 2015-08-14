using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    [TestClass]
    public class FedExVoidClientDetailManipulatorTest
    {
        private FedExVoidClientDetailManipulator testObject;

        private Mock<CarrierRequest> voidCarrierRequest;
        private DeleteShipmentRequest nativeRequest;

        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", MeterNumber = "67890"};

            nativeRequest = new DeleteShipmentRequest { ClientDetail = new ClientDetail() };
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            voidCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExVoidClientDetailManipulator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(voidCarrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVoidRequest_AndIsNotVoidRequest_Test()
        {
            // Setup the native request to be an unexpected type
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ShipmentReply());

            testObject.Manipulate(voidCarrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForVoid_Test()
        {
            testObject.Manipulate(voidCarrierRequest.Object);

            voidCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForVoid_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(voidCarrierRequest.Object);

            ClientDetail detail = ((DeleteShipmentRequest)voidCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForVoid_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(voidCarrierRequest.Object);

            ClientDetail detail = ((DeleteShipmentRequest)voidCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }
    }
}
