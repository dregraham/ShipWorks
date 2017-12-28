﻿using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// None label service
    /// </summary>
    public class NoneLabelService : ILabelService
    {
        /// <summary>
        /// Creates a none label
        /// </summary>
        public Task<IDownloadedLabelData> Create(ShipmentEntity shipment)
        {
            throw new ShippingException("No carrier is selected for the shipment.");
        }

        /// <summary>
        /// Voids a none label
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            // None does not support voiding
        }
    }
}