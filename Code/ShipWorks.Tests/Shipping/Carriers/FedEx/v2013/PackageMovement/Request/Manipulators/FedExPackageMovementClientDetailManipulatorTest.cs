using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.PackageMovement.Request.Manipulators
{
    [TestClass]
    public class FedExPackageMovementClientDetailManipulatorTest
    {
        private FedExPackageMovementClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<CarrierRequest> carrierRequest;

        private PostalCodeInquiryRequest nativeRequest;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", MeterNumber = "67890"};

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);


            nativeRequest = new PostalCodeInquiryRequest { ClientDetail = new ClientDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExPackageMovementClientDetailManipulator(settingsRepository.Object);
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ServiceAvailabilityRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_DelegatesToRequestForFedExAccount_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            ClientDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(carrierRequest.Object);

            ClientDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }
    }
}
