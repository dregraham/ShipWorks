using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Represents the default shipment pre processor
    /// </summary>
    public interface IDefaultShipmentPreProcessor
    {
        /// <summary>
        /// Run the pre processor on the given shipment
        /// </summary>
        IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback);
    }
}