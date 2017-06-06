using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.SingleScan.ScannerServicePipelines
{
    /// <summary>
    /// Pipelines for the scanner service
    /// </summary>
    [Service]
    public interface IScannerServicePipeline
    {
        /// <summary>
        /// Register the pipeline
        /// </summary>
        IDisposable Register(ScannerService scannerService);
    }
}
