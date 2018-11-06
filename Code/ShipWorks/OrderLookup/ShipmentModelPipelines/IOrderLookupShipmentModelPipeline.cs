using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for clearing the search box
    /// </summary>
    [Service]
    public interface IOrderLookupShipmentModelPipeline
    {
        /// <summary>
        /// Register the pipeline
        /// </summary>
        IDisposable Register(IOrderLookupShipmentModel model);
    }
}