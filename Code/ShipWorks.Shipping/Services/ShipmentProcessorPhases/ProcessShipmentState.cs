using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Initial state for processing a shipment
    /// </summary>
    public class ProcessShipmentState
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentState(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentState(ShipmentEntity shipment, IDictionary<long, Exception> licenseCheckCache, RateResult chosenRate)
        {
            ChosenRate = chosenRate;
            OriginalShipment = shipment;
            LicenseCheckCache = licenseCheckCache;
        }

        /// <summary>
        /// Rate that was chosen for processing
        /// </summary>
        public RateResult ChosenRate { get; }

        /// <summary>
        /// Cache of license check results
        /// </summary>
        public IDictionary<long, Exception> LicenseCheckCache { get; }

        /// <summary>
        /// Original shipment scheduled for processing
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Exception from the previous phase, if any
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Was previous phase successful
        /// </summary>
        public bool Success => Exception == null;
    }
}