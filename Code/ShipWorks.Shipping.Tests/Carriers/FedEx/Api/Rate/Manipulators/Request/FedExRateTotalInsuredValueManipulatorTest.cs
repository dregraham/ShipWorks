using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateTotalInsuredValueManipulatorTest
    {
        private FedExRateTotalInsuredValueManipulator testObject;
        private RateRequest rateRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<IFedExSettingsRepository> settingsRepository;

        public FedExRateTotalInsuredValueManipulatorTest()
        {
            // Create a RateRequest type and set the properties the manipulator is interested in
            rateRequest = new RateRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            EntityCollection<FedExPackageEntity> packages = new EntityCollection<FedExPackageEntity>();
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal) 100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal) 100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            packages.Add(new FedExPackageEntity() { DeclaredValue = (decimal) 100.29, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            };

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<IFedExSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });
            settingsRepository.Setup(r => r.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });

            testObject = new FedExRateTotalInsuredValueManipulator(settingsRepository.Object);
        }

        [Fact]
        public void Shouldapply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(null, FedExRateRequestOptions.None));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenRateRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(shipmentEntity,  null));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            rateRequest.RequestedShipment = null;

            testObject.Manipulate(shipmentEntity, rateRequest);

            // The requested shipment property should be created now
            Assert.NotNull(rateRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD()
        {
            testObject.Manipulate(shipmentEntity, rateRequest);

            Money totalInsuredValue = rateRequest.RequestedShipment.TotalInsuredValue;
            Assert.Equal("USD", totalInsuredValue.Currency);
        }

        [Fact]
        public void Manipulate_SetsAmount()
        {
            // // Add some packages for the sum test
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.02m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.03m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            const decimal expectedTotal = 300.06m;

            testObject.Manipulate(shipmentEntity, rateRequest);

            Money totalInsuredValue = rateRequest.RequestedShipment.TotalInsuredValue;
            Assert.Equal(expectedTotal, totalInsuredValue.Amount);
        }

        [Fact]
        public void Manipulate_SetsAmountSpecified()
        {
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m });

            testObject.Manipulate(shipmentEntity, rateRequest);

            Money totalInsuredValue = rateRequest.RequestedShipment.TotalInsuredValue;
            Assert.True(totalInsuredValue.AmountSpecified);
        }
    }
}
