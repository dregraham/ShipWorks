using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Shared code required to process a set of shipments
    /// </summary>
    public interface IShipmentProcessor
    {
        /// <summary>
        /// Process the list of shipments
        /// </summary>
        /// <param name="shipmentsToProcess">The shipments to process.</param>
        /// <param name="shipmentRefresher">The CarrierConfigurationShipmentRefresher</param>
        /// <param name="chosenRateResult">Rate that was chosen to use, if there was any</param>
        /// <param name="counterRateCarrierConfiguredWhileProcessingAction">Execute after a counter rate carrier was configured</param>
        Task<IEnumerable<ProcessShipmentResult>> Process(IEnumerable<ShipmentEntity> shipmentsToProcess,
            ICarrierConfigurationShipmentRefresher shipmentRefresher,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction);
    }
}
