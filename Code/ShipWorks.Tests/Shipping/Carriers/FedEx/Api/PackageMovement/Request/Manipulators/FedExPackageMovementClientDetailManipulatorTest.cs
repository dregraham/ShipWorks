using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators
{
    public class FedExPackageMovementClientDetailManipulatorTest
    {
        private FedExPackageMovementClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<CarrierRequest> carrierRequest;

        private PostalCodeInquiryRequest nativeRequest;
        private FedExAccountEntity account;

        public FedExPackageMovementClientDetailManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);


            nativeRequest = new PostalCodeInquiryRequest { ClientDetail = new ClientDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExPackageMovementClientDetailManipulator(settingsRepository.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ServiceAvailabilityRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount()
        {
            testObject.Manipulate(carrierRequest.Object);

            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            ClientDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(carrierRequest.Object);

            ClientDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }
    }
}
