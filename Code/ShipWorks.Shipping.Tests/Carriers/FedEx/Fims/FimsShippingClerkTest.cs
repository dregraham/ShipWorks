using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Fims
{
    public class FimsShippingClerkTest
    {
        [Fact]
        public void Ship_DelegatesToLabelRepository_ClearReferences()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var settings = new ShippingSettingsEntity() {FedExFimsUsername = "U", FedExFimsPassword = "P"};
                var settingsRepository = mock.Mock<ICarrierSettingsRepository>();
                settingsRepository.Setup(s => s.GetShippingSettings()).Returns(settings);

                var shipment = new ShipmentEntity();
                shipment.FedEx = new FedExShipmentEntity();
                shipment.FedEx.Packages.Add(new FedExPackageEntity());
                shipment.FedEx.Service = (int) FedExServiceType.FedExFimsStandard;
                shipment.ShipCountryCode = "CA";
                shipment.CustomsItems.Add(new ShipmentCustomsItemEntity());

                var labelRepository = mock.Mock<IFimsLabelRepository>();

                var testObject = mock.Create<FimsShippingClerk>();

                testObject.Ship(shipment);

                labelRepository.Verify(r => r.ClearReferences(It.IsAny<ShipmentEntity>()));
            }
        }

        [Fact]
        public void Ship_DelegatesToWebClient_Ship()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var settings = new ShippingSettingsEntity() { FedExFimsUsername = "U", FedExFimsPassword = "P" };
                var settingsRepository = mock.Mock<ICarrierSettingsRepository>();
                settingsRepository.Setup(s => s.GetShippingSettings()).Returns(settings);

                var shipment = new ShipmentEntity();
                shipment.FedEx = new FedExShipmentEntity();
                shipment.FedEx.Packages.Add(new FedExPackageEntity());
                shipment.FedEx.Service = (int)FedExServiceType.FedExFimsStandard;
                shipment.ShipCountryCode = "CA";
                shipment.CustomsItems.Add(new ShipmentCustomsItemEntity());

                var webClient = mock.Mock<IFimsWebClient>();

                var testObject = mock.Create<FimsShippingClerk>();

                testObject.Ship(shipment);

                webClient.Verify(r => r.Ship(It.IsAny<IFimsShipRequest>()));
            }
        }

        [Fact]
        public void Track_AddsTrackingNumberToTrackingLink()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var shipment = new ShipmentEntity();
                shipment.FedEx = new FedExShipmentEntity();
                shipment.FedEx.Packages.Add(new FedExPackageEntity() {TrackingNumber = "12345"});
                shipment.FedEx.Service = (int)FedExServiceType.FedExFimsStandard;
                shipment.ShipCountryCode = "CA";
                shipment.CustomsItems.Add(new ShipmentCustomsItemEntity());

                var testObject = mock.Create<FimsShippingClerk>();

                var trackingResult = testObject.Track(shipment);
                string trackUrl = "Click <a href=\"http://mailviewrecipient.fedex.com/recip_package_summary.aspx?PostalID=12345\" style='background-color: transparent; color:Blue;'>here</a> to view tracking information.";
                Assert.Equal(trackUrl, trackingResult.Summary);
            }
        }
    }
}