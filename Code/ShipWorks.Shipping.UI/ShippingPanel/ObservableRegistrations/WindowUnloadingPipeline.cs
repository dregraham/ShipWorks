using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the window unloading message
    /// </summary>
    public class WindowUnloadingPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private IDisposable subscription;
        private readonly ILog log;
        private readonly Func<IMainForm> getOwner;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowUnloadingPipeline(
            Func<IMainForm> getOwner,
            IObservable<IShipWorksMessage> messageStream,
            Func<Type, ILog> createLog)
        {
            this.getOwner = getOwner;
            this.messageStream = messageStream;
            log = createLog(GetType());
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<WindowResettingMessage>()
                .Do(_ => SaveShipmentAndUnload(viewModel))
                .CatchAndContinue((SingleUserModeException ex) => log.Error("Could not save view model", ex))
                .Subscribe();
        }

        /// <summary>
        /// Save shipment and unload panel
        /// </summary>
        /// <remarks>
        /// We've got to invoke if necessary instead of using the scheduler because the scheduler
        /// was scheduling the save regardless of if it was necessary. So when archiving, ShipWorks
        /// could crash depending on when the save occurred.
        /// </remarks>
        private void SaveShipmentAndUnload(ShippingPanelViewModel viewModel)
        {
            var owner = getOwner();

            if (owner.InvokeRequired)
            {
                owner.Invoke(((Action<ShippingPanelViewModel>) SaveShipmentAndUnload), new[] { viewModel });
            }
            else
            {
                viewModel?.SaveToDatabase();
                viewModel?.UnloadShipment();
            }
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
