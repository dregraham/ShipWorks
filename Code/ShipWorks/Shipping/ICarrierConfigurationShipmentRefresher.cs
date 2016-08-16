using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Refresh shipments when a carrier is configured for the first time
    /// </summary>
    public interface ICarrierConfigurationShipmentRefresher : IDisposable
    {
        /// <summary>
        /// Allows a given context to provide a way to retrieve all shipments
        /// </summary>
        Func<IEnumerable<ShipmentEntity>> RetrieveShipments { get; set; }

        /// <summary>
        /// Tell the refresher which shipments are currently being processed so it can react accordingly
        /// </summary>
        void ProcessingShipments(IEnumerable<ShipmentEntity> processingShipmentList);

        /// <summary>
        /// Tell the refresher that processing is finished so it can behave normally
        /// </summary>
        void FinishProcessing();
    }
}