using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.Volusion;

namespace ShipWorks.Tests.Stores.Volusion
{
    /// <summary>
    /// Summary description for VolusionWebClientTest
    /// </summary>
    public class VolusionWebClientTest
    {
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;

        public VolusionWebClientTest()
        {
            orderEntity = new NeweggOrderEntity { OrderNumber = 123456 };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
        }

        [Fact]
        public void GetVolusionGateway_ReturnsDhl_WhenEndiciaAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = VolusionWebClient.GetVolusionGateway(shipmentEntity);

            Assert.Equal("DHL", carrierCode);
        }

        [Fact]
        public void GetVolusionGateway_ReturnsDhl_WhenUspsAndDhlServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = VolusionWebClient.GetVolusionGateway(shipmentEntity);

            Assert.Equal("DHL", carrierCode);
        }

        [Fact]
        public void GetVolusionGateway_ReturnsOther_WhenEndiciaAnConsolidatorServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = VolusionWebClient.GetVolusionGateway(shipmentEntity);

            Assert.Equal("OTHER", carrierCode);
        }

        [Fact]
        public void GetVolusionGateway_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = VolusionWebClient.GetVolusionGateway(shipmentEntity);

            Assert.Equal("USPS", carrierCode);
        }

        [Fact]
        public void GetVolusionGateway_ReturnsUsps_WhenUspsAndFirstClassServiceUsed_Test()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = VolusionWebClient.GetVolusionGateway(shipmentEntity);

            Assert.Equal("USPS", carrierCode);
        }

    }
}
