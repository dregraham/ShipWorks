using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.Etsy;

namespace ShipWorks.Tests.Stores.Etsy
{
    /// <summary>
    /// Summary description for NeweggWebClientTest
    /// </summary>
    public class EtsyOnlineUpdaterTest
    {
        private EtsyOrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;

        public EtsyOnlineUpdaterTest()
        {
            orderEntity = new EtsyOrderEntity { OrderNumber = 123456 };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
        }

        [Fact]
        public void GetEtsyCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.Equal("dhl", carrierCode);
        }

        [Fact]
        public void GetEtsyCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelGround;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.Equal("dhl", carrierCode);
        }

        [Fact]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClassServiceUsed()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Endicia;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenUspsAndFirstClassServiceUsed()
        {
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipmentEntity.Postal = postalShipmentEntity;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.Equal("usps", carrierCode);
        }

        [Fact]
        public void GetEtsyCarrierCode_ReturnsUsps_WhenOther()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;

            string carrierCode = EtsyOnlineUpdater.GetEtsyCarrierCode(shipmentEntity);

            Assert.Equal("other", carrierCode);
        }
    }
}
