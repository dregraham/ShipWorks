using System;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan.ScannerServicePipelines
{
    /// <summary>
    /// Disable scanner service pipeline
    /// </summary>
    public class DisableSingleScanInputFilterPipeline : IScannerServicePipeline
    {
        readonly IObservable<IShipWorksMessage> stream;

        /// <summary>
        /// Constructor
        /// </summary>
        public DisableSingleScanInputFilterPipeline(IObservable<IShipWorksMessage> stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(ScannerService scannerService)
        {
            return stream.OfType<DisableSingleScanInputFilterMessage>()
                .Subscribe(_ => scannerService.Disable());
        }
    }
}
