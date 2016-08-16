using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExTotalInsuredValueManipulatorTest
    {
        private FedExTotalInsuredValueManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private object nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExTotalInsuredValueManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            };

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            SetUpNewCarrierRequest();

            testObject = new FedExTotalInsuredValueManipulator(new FedExSettings(settingsRepository.Object));
        }

        /// <summary>
        /// Create a new carrier request with defaults
        /// </summary>
        private void SetUpNewCarrierRequest()
        {
            // Add some packages for the sum test
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.02m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.03m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);
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
            nativeRequest = null;
            SetUpNewCarrierRequest();

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            nativeRequest = new object();
            SetUpNewCarrierRequest();

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            ((ProcessShipmentRequest)nativeRequest).RequestedShipment = null;
            SetUpNewCarrierRequest();

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(((ProcessShipmentRequest)nativeRequest).RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD()
        {
            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.Equal("USD", totalInsuredValue.Currency);
        }

        [Fact]
        public void Manipulate_SetsAmount()
        {
            const decimal expectedTotal = 300.06m;

            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.Equal(expectedTotal, totalInsuredValue.Amount);
        }
    }
}
