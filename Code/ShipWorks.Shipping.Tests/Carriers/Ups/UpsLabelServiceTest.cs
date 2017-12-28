using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsLabelServiceTest
    {
        readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly UpsLabelService testObject;

        public UpsLabelServiceTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<UpsLabelService>();
            shipment = new ShipmentEntity()
            {
                InsuranceProvider = (int) InsuranceProvider.Carrier,
                Ups = new UpsShipmentEntity()
                {
                    Service = (int) UpsServiceType.UpsSurePost1LbOrGreater,
                    Packages =
                    {
                        new UpsPackageEntity()
                        {
                            Insurance = true,
                            InsuranceValue = 1
                        }
                    }
                }
            };
        }

        [Fact]
        public async Task UpsLabelServiceCreate_ThrowsCarrierException_WithSurePostDeclaredValue()
        {
            CarrierException ex = await Assert.ThrowsAsync<CarrierException>(() => testObject.Create(shipment));
            Assert.Equal("UPS declared value is not supported for SurePost shipments. For insurance coverage, go to Shipping Settings and enable ShipWorks Insurance for this carrier.", ex.Message);
        }

        [Fact]
        public async Task UpsLabelServiceCreate_ThrowsInvalidPackageDimensionsException_WithZeroDimensions()
        {
            shipment.Ups.Packages.Select(p => p.InsuranceValue = 0).ToList();
            InvalidPackageDimensionsException ex = await Assert.ThrowsAsync<InvalidPackageDimensionsException>(() => testObject.Create(shipment));
            Assert.Equal("Package 1 has invalid dimensions.\r\nPackage dimensions must be greater than 0 and not 1x1x1.  ", ex.Message);
        }

        [Fact]
        public void UpsLabelServiceCreate_ClearsReturnContents_ForMiShipments()
        {
            shipment.Ups.Service = (int) UpsServiceType.UpsMailInnovationsExpedited;
            foreach (UpsPackageEntity package in shipment.Ups.Packages)
            {
                package.DimsHeight = 7;
                package.DimsLength = 7;
                package.DimsWidth = 7;
            }
            shipment.Ups.ReturnContents = "Something not supported by Mi";

            testObject.Create(shipment);

            Assert.Equal(string.Empty, shipment.Ups.ReturnContents);
        }
    }
}