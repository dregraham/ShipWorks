using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using log4net;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    
    public class FimsShippingClerkTest
    {
        private FimsShippingClerk testObject;

        private Mock<IFimsWebClient> webClient;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<ILog> log;

        private Mock<IFimsShipResponse> shipResponse;
        private Mock<IFimsLabelRepository> labelRepository;
        private ShipmentEntity shipmentEntity;
        private ShippingSettingsEntity shippingSettings;

        public FimsShippingClerkTest()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.Info(It.IsAny<string>()));
            log.Setup(l => l.Error(It.IsAny<string>()));

            shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExFimsUsername = "Success";
            shippingSettings.FedExFimsPassword = "Password";

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccounts()).Returns
                (
                    new List<FedExAccountEntity>()
                    {
                        new FedExAccountEntity() {MeterNumber = "123"},
                        new FedExAccountEntity() {MeterNumber = "456"},
                        new FedExAccountEntity() {MeterNumber = "789"}
                    }
                );

            // Return a FedEx account that has been migrated
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { MeterNumber = "123" });
            settingsRepository.Setup(r => r.UseTestServer).Returns(false);
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            shipResponse = new Mock<IFimsShipResponse>();

            labelRepository = new Mock<IFimsLabelRepository>();
            labelRepository.Setup(f => f.ClearReferences(It.IsAny<ShipmentEntity>()));

            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.FedEx.SmartPostHubID = "5571";
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExFims;
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity());

            webClient = new Mock<IFimsWebClient>();
            webClient.Setup(w => w.Ship(It.IsAny<IFimsShipRequest>())).Returns(shipResponse.Object);

            // Force our test object to perform version capture when called.
            testObject = new FimsShippingClerk(webClient.Object, labelRepository.Object, settingsRepository.Object, log.Object);
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenFimsUsernameIsBlank_Test()
        {
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExFimsUsername = string.Empty;
            shippingSettings.FedExFimsPassword = "asdf";

            // Create the shipment and setup the repository to return a null account for this test
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS Username is missing".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenFimsPasswordIsBlank_Test()
        {
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExFimsUsername = "asdf";
            shippingSettings.FedExFimsPassword = string.Empty;

            // Create the shipment and setup the repository to return a null account for this test
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));

            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS Password is missing".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenServiceIsNotFims_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
            
            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments require selecting a FIMS service type".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenShipCountryIsUs_Test()
        {
            shipmentEntity.ShipCountryCode = "US";

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments cannot be shipped domestically".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenCustomsItemsIsEmpty_Test()
        {
            shipmentEntity.CustomsItems.Clear();
            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments require customs information to be entered".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenPackageCountIsGreaterThan1_Test()
        {
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments allow only 1 package".ToUpperInvariant()));
        }


        #region GetRates Tests

        [Fact]
        public void GetRates_ReturnsEmptyRateGroup_Test()
        {
            RateGroup rateGroup = testObject.GetRates(shipmentEntity);

            Assert.True(!rateGroup.Rates.Any());
        }
        #endregion

    }
}
