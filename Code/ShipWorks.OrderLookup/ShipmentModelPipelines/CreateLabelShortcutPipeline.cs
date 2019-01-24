using System;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Listens for CreateLabel ShortcutMessage and sends a ProcessShipmentsMessage when it receives one
    /// </summary>
    public class CreateLabelShortcutPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelShortcutPipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> createLog)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            log = createLog(GetType());
        }

        /// <summary>
        /// Register the pipeline on the shipment model
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel orderLookupShipmentModel) =>
            messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.CreateLabel))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(_ => orderLookupShipmentModel.CreateLabel().Forget())
                .CatchAndContinue((Exception ex) => log.Error("Error while creating label through keyboard shortcut", ex))
                .Subscribe();
    }
}