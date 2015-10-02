using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers
{
    public abstract class ShipmentAdapter : IShipmentAdapter
    {
        private readonly ShipmentEntity shipmentEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentAdapter" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        protected ShipmentAdapter(ShipmentEntity shipmentEntity)
        {
            this.shipmentEntity = shipmentEntity;
            OriginPersonAdapter = new PersonAdapter(shipmentEntity, "Origin");
            DestinationPersonAdapter = new PersonAdapter(shipmentEntity, "Ship");
        }

        /// <summary>
        /// Selected shipment type code for the current shipment
        /// </summary>
        public ShipmentTypeCode ShipmentType
        {
            get { return shipmentEntity.ShipmentTypeCode; }
            set { shipmentEntity.ShipmentTypeCode = value; }
        }

        /// <summary>
        /// Is the loaded shipment processed?
        /// </summary>
        public bool IsProcessed
        {
            get { return shipmentEntity.Processed; }
            set { shipmentEntity.Processed = value; }
        }

        /// <summary>
        /// Origin address type that should be used
        /// </summary>
        public long OriginAddressType
        {
            get { return shipmentEntity.OriginOriginID; }
            set { shipmentEntity.OriginOriginID = value; }
        }

        /// <summary>
        /// The origin person adapter
        /// </summary>
        public PersonAdapter OriginPersonAdapter { get; set; }

        /// <summary>
        /// The destination person adapter
        /// </summary>
        public PersonAdapter DestinationPersonAdapter { get; set; }
    }
}
