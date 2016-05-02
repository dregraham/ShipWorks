using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Sears;
using Xunit;

namespace ShipWorks.Tests.Stores.Sears
{
    public class SearsUtilityTest
    {
        [Fact]
        public void GetShipmentCarrierCode_ReturnsOTH_WhenCustomerPickup()
        {
            ShipmentEntity shipment = GetSearsShipment(true, ShipmentTypeCode.FedEx);

            string carrierCode = SearsUtility.GetShipmentCarrierCode(shipment);

            Assert.Equal("OTH", carrierCode);
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsSMRT_WhenSmartPost()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.SmartPost
            };

            Assert.Equal("SMRT", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsFXFT_WhenFreight()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FedEx1DayFreight
            };

            Assert.Equal("FXFT", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsFDE_WhenFedEx()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FedExGround
            };

            Assert.Equal("FDE", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsUPSM_WhenMailInnovations()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsMailInnovationsExpedited
            };

            Assert.Equal("UPSM", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsUPS_WhenUPS()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsGround
            };

            Assert.Equal("UPS", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsUSPS_WhenUsps()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Usps);

            Assert.Equal("USPS", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsUPSI_WhenIParcel()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.iParcel);

            Assert.Equal("UPSI", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentCarrierCode_ReturnsOTH_WhenOther()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Other);

            Assert.Equal("OTH", SearsUtility.GetShipmentCarrierCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsPICKUP_WhenCustomerPickup()
        {
            ShipmentEntity shipment = GetSearsShipment(true, ShipmentTypeCode.Other);

            Assert.Equal("PICKUP", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsNextDay_WhenFedExOvernight()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FirstOvernight
            };

            Assert.Equal("Next Day", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsSecondDay_WhenFedEx2Day()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FedEx2Day
            };

            Assert.Equal("Second Day", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsLTL_WhenFedExFreight()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FedEx1DayFreight
            };

            Assert.Equal("LTL", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsSmartPost_WhenFedExSmartPost()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.SmartPost
            };

            Assert.Equal("Smart Post", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsGround_WhenFedExGround()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.FedEx);

            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.FedExGround
            };

            Assert.Equal("Ground", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsParcel_WhenUpsMiService()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsMailInnovationsExpedited
            };

            Assert.Equal("Parcel", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsParcel_WhenUpsWorldShipMiService()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsWorldShip);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsMailInnovationsExpedited
            };

            Assert.Equal("Parcel", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsSurePost_WhenUpsSurePost()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsSurePostBoundPrintedMatter
            };

            Assert.Equal("SurePost", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsNextDaySaver_WhenUpsNextDayAirSaver()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsNextDayAirSaver
            };

            Assert.Equal("Next Day Saver", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsNextDay_WhenUpsNextDayAir()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsNextDayAir
            };

            Assert.Equal("Next Day", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsSecondDay_WhenUpsSecondDayAir()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.Ups2DayAir
            };

            Assert.Equal("Second Day", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsGround_WhenUpsGround()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.UpsOnLineTools);

            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsGround
            };

            Assert.Equal("Ground", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsParcelPost_WhenParcelSelect()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Usps);

            shipment.Postal = new PostalShipmentEntity
            {
                Service = (int) PostalServiceType.ParcelSelect
            };

            Assert.Equal("Parcel Post", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsFirstClass_WhenFirstClass()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Usps);

            shipment.Postal = new PostalShipmentEntity
            {
                Service = (int) PostalServiceType.FirstClass
            };

            Assert.Equal("First Class", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsStandardMail_WhenStandardPost()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Usps);

            shipment.Postal = new PostalShipmentEntity
            {
                Service = (int)PostalServiceType.StandardPost
            };

            Assert.Equal("Standard Mail", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsPriority_WhenPriorityMail()
        {
            ShipmentEntity shipment = GetSearsShipment(false, ShipmentTypeCode.Usps);

            shipment.Postal = new PostalShipmentEntity
            {
                Service = (int)PostalServiceType.PriorityMail
            };

            Assert.Equal("Priority", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsParcel_WhenIParcel()
        {
            var shipment = GetSearsShipment(false, ShipmentTypeCode.iParcel);

            Assert.Equal("Parcel", SearsUtility.GetShipmentServiceCode(shipment));
        }

        [Fact]
        public void GetShipmentServiceCode_ReturnsPRIORITY_WhenOther()
        {
            var shipment = GetSearsShipment(false, ShipmentTypeCode.Other);

            Assert.Equal("PRIORITY", SearsUtility.GetShipmentServiceCode(shipment));
        }

        private static ShipmentEntity GetSearsShipment(bool customerPickup, ShipmentTypeCode shipmentTypeCode)
        {
            return new ShipmentEntity
            {
                ShipmentType = (int) shipmentTypeCode,
                Order = new SearsOrderEntity
                {
                    CustomerPickup = customerPickup
                }
            };
        }
    }
}