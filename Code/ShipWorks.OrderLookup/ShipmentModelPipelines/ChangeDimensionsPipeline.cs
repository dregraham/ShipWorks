using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for clearing the search box
    /// </summary>
    public class ChangeDimensionsPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeDimensionsPipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider)
        {
            this.schedulerProvider = schedulerProvider;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) =>
            messenger.OfType<ChangeDimensionsMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(message => HandleChangeDimensionsMessage(model, message))
                .Subscribe();

        /// <summary>
        /// Handle the dimension change message
        /// </summary>
        private void HandleChangeDimensionsMessage(IOrderLookupShipmentModel model, ChangeDimensionsMessage message)
        {
            if (message.ScaleReadResult.HasVolumeDimensions)
            {
                IPackageAdapter packageAdapter = model.PackageAdapters.First();

                packageAdapter.DimsProfileID = 0;
                packageAdapter.DimsLength = message.ScaleReadResult.Length;
                packageAdapter.DimsWidth = message.ScaleReadResult.Width;
                packageAdapter.DimsHeight = message.ScaleReadResult.Height;
                packageAdapter.ApplyAdditionalWeight = false;
            }
        }
    }
}
