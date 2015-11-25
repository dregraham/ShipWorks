﻿using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// LabelService for Best Rate
    /// </summary>
    public class BestRateLabelService : ILabelService
    {
        /// <summary>
        /// Creates the label
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            // This is by design. The best rate shipment type should never actually
            // process a shipment due to the pre-process functionality
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Voids the label
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {

        }
    }
}