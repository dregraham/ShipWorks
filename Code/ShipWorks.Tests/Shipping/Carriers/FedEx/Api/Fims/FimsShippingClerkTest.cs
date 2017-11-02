using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    public class FimsShippingClerkTest
    {
        private FimsShippingClerk testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipmentEntity;

        public FimsShippingClerkTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.FedEx.SmartPostHubID = "5571";
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExFimsMailView;
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity());

            testObject = mock.Create<FimsShippingClerk>();
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenFimsUsernameIsBlank()
        {
            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.Contains("FedEX FIMS Username is missing", ex.Message);
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenFimsPasswordIsBlank()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(r => r.GetShippingSettings())
                .Returns(new ShippingSettingsEntity { FedExFimsUsername = "foo" });

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));

            Assert.Contains("FedEX FIMS Password is missing", ex.Message);
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenServiceIsNotFims()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));

            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments require selecting a FIMS service type".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenShipCountryIsUs()
        {
            shipmentEntity.ShipCountryCode = "US";

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments cannot be shipped domestically".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenCustomsItemsIsEmpty()
        {
            shipmentEntity.CustomsItems.Clear();
            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments require customs information to be entered".ToUpperInvariant()));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenPackageCountIsGreaterThan1()
        {
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());

            Exception ex = Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
            Assert.True(ex.Message.ToUpperInvariant().Contains("FedEX FIMS shipments allow only 1 package".ToUpperInvariant()));
        }

        [Fact]
        public void GetRates_ReturnsEmptyRateGroup()
        {
            RateGroup rateGroup = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.Empty(rateGroup.Rates);
        }
    }
}
