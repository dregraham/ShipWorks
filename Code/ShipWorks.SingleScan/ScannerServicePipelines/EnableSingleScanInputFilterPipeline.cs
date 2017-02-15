using System;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan.ScannerServicePipelines
{
    /// <summary>
    /// Enable scanner service pipeline
    /// </summary>
    public class EnableSingleScanInputFilterPipeline : IScannerServicePipeline
    {
        readonly IObservable<IShipWorksMessage> stream;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnableSingleScanInputFilterPipeline(IObservable<IShipWorksMessage> stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(ScannerService scannerService)
        {
            return stream.OfType<EnableSingleScanInputFilterMessage>()
                .Where(_ => scannerService.IsSingleScanEnabled())
                .Subscribe(_ => scannerService.Enable());
        }
    }
}
