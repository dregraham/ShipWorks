﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Adapter for Usps specific shipment information
    /// </summary>
    public class UspsShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public UspsShipmentAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the Usps account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.Postal.Usps.UspsAccountID; }
            set { shipment.Postal.Usps.UspsAccountID = value.GetValueOrDefault(); }
        }
    }
}
