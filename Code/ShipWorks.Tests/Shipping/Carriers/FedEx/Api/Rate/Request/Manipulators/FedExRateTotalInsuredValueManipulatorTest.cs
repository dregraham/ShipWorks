using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateTotalInsuredValueManipulatorTest
    {
        private FedExRateTotalInsuredValueManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExRateTotalInsuredValueManipulatorTest()
        {
            // Create a RateRequest type and set the properties the manipulator is interested in
            nativeRequest = new RateRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            EntityCollection<FedExPackageEntity> packages = new EntityCollection<FedExPackageEntity>();
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal)100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal)100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal)100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });

            testObject = new FedExRateTotalInsuredValueManipulator(new FedExSettings(settingsRepository.Object));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.Equal("USD", totalInsuredValue.Currency);
        }

        [Fact]
        public void Manipulate_SetsAmount_Test()
        {
            // // Add some packages for the sum test
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.02m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.03m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            const decimal expectedTotal = 300.06m;

            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.Equal(expectedTotal, totalInsuredValue.Amount);
        }

        [Fact]
        public void Manipulate_SetsAmountSpecified_Test()
        {
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m });

            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.True(totalInsuredValue.AmountSpecified);
        }
    }
}
