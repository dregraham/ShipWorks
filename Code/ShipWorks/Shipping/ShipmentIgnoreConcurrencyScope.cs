using System;
using Common.Logging;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Class to disable Concurrency checks when saving a shipment.  However, the database and
    /// current value of Processed must be the same otherwise a concurrency exception will be
    /// allowed.
    /// </summary>
    public class ShipmentIgnoreConcurrencyScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentIgnoreConcurrencyScope));

        private readonly ShipmentEntity shipment;
        private readonly IConcurrencyPredicateFactory previousConcurrencyPredicateFactory;

        /// <summary>
        /// Constructs a new ShipmentIgnoreConcurrencyScope.  The given SqlSession is active until
        /// the SqlSessionScope object is disposed.
        /// </summary>
        public ShipmentIgnoreConcurrencyScope(ShipmentEntity shipment)
        {
            log.InfoFormat($"Entering ShipmentIgnoreConcurrencyScope for shipmentID: {shipment.ShipmentID}");
            this.shipment = shipment;

            // Ensure that we're using optimistic concurrency with this entity because we don't want to overwrite
            // changes that may have happened elsewhere
            previousConcurrencyPredicateFactory = shipment.ConcurrencyPredicateFactoryToUse;
            shipment.ConcurrencyPredicateFactoryToUse = new ShipmentNotProcessedIgnoreConcurrencyFactory();
        }

        /// <summary>
        /// Reset ConcurrencyPredicateFactoryToUse to the original one
        /// </summary>
        public void Dispose()
        {
            shipment.ConcurrencyPredicateFactoryToUse = previousConcurrencyPredicateFactory;

            log.InfoFormat($"Leaving ShipmentIgnoreConcurrencyScope for shipmentID: {shipment.ShipmentID}");
        }
    }
}
