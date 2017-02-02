using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Preprocess a shipment
    /// </summary>
    public interface IShipmentPreProcessor
    {
        /// <summary>
        /// Run the preprocessor
        /// </summary>
        IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback);
    }
}