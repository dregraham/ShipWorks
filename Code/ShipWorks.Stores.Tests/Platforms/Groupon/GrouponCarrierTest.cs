using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Groupon;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Groupon
{
    public class GrouponCarrierTest
    {
        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenCarrierIsUsps()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.Postal = new PostalShipmentEntity();
            shipment.Postal.Service = (int) PostalServiceType.PriorityMail;
            shipment.ShipmentType = (int) ShipmentTypeCode.Usps;

            Assert.Equal("usps", GrouponCarrier.GetCarrierCode(shipment));
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenCarrierIsDhl()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.Postal = new PostalShipmentEntity();
            shipment.Postal.Service = (int) PostalServiceType.DhlParcelPlusExpedited;
            shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;

            Assert.Equal("dhl", GrouponCarrier.GetCarrierCode(shipment));
        }

        [Fact]
        public void GetCarrierCode_ReturnsFedex_WhenCarrierIsFedex()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.FedEx = new FedExShipmentEntity();
            shipment.FedEx.Service = (int) FedExServiceType.FedEx2Day;
            shipment.ShipmentType = (int) ShipmentTypeCode.FedEx;

            Assert.Equal("fedex", GrouponCarrier.GetCarrierCode(shipment));
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenCarrierIsUpsAndServiceIsNotMI()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.Ups = new UpsShipmentEntity();
            shipment.Ups.Service = (int) UpsServiceType.Ups2DayAir;
            shipment.ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools;

            Assert.Equal("ups", GrouponCarrier.GetCarrierCode(shipment));
        }

        [Fact]
        public void GetCarrierCode_ReturnsUpsmi_WhenCarrierIsUpsAndServiceIsMI()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.Ups = new UpsShipmentEntity();
            shipment.Ups.Service = (int) UpsServiceType.UpsMailInnovationsExpedited;
            shipment.ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools;

            Assert.Equal("upsmi", GrouponCarrier.GetCarrierCode(shipment));
        }

        [Fact]
        public void GetCarrierCode_ReturnsOtherCarrierName_WhenCarrierIsOther()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.Other = new OtherShipmentEntity();
            shipment.Other.Carrier = "Other Carrier";
            shipment.ShipmentType = (int) ShipmentTypeCode.Other;

            Assert.Equal("Other Carrier", GrouponCarrier.GetCarrierCode(shipment));
        }
    }
}