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
        /// <param name="shipments">List of shipments to process</param>
        /// <param name="chosenRate">Rate that was chosen to use, if there was any</param>
        /// <param name="counterRateCarrierConfiguredWhileProcessing">Execute after a counter rate carrier was configured</param>
        /// <returns></returns>
        Task<IEnumerable<ProcessShipmentResult>> Process(IEnumerable<ShipmentEntity> shipments,
            ICarrierConfigurationShipmentRefresher shipmentRefresher,
            RateResult chosenRate, Action counterRateCarrierConfiguredWhileProcessing);

        /// <summary>
        /// Filtered rates that should be displayed after shipping
        /// </summary>
        RateGroup FilteredRates { get; }
    }
}
