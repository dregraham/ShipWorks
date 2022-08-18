using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Tests.Stores.Platforms.Amazon
{
    public class AmazonUtilityTest
    {
        private AmazonOrderEntity orderEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private readonly ShipmentEntity shipmentEntity;

        public AmazonUtilityTest()
        {
            orderEntity = new AmazonOrderEntity { OrderNumber = 123456 };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int) PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int) ShipmentTypeCode.Usps, Postal = postalShipmentEntity };
        }

        [Fact]
        public void GetCarrierName_ReturnsUsps_WhenUspsAndFirstClass()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Usps;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("USPS", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsUsps_WhenEndiciaAndFirstClass()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("USPS", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsDhl_WhenEndiciaAndDhl()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("DHL eCommerce", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsDhl_WhenUspsAndDhl()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("DHL eCommerce", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsConsolidator_WhenEndiciaAndConsolidator()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int) PostalServiceType.ConsolidatorDomestic;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("Consolidator", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsOtherCarrierDesc_WhenOther()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("Some other carrier", carrierName);
        }

        [Fact]
        public void GetCarrierName_Returns_WhenDhlEcommerce()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.DhlEcommerce;
            shipmentEntity.DhlEcommerce = new DhlEcommerceShipmentEntity();

            string carrierName = AmazonUtility.GetCarrierName(shipmentEntity, ShipmentTypeCode.DhlEcommerce);

            Assert.Equal("DHL", carrierName);
        }
    }
}
