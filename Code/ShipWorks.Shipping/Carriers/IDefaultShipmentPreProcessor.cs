using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

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
        Task<IEnumerable<ShipmentEntity>> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback);
    }
}