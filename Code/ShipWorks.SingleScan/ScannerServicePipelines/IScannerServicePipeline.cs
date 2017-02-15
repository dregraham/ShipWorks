using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

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
