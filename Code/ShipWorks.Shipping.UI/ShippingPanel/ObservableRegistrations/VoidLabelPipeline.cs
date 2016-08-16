using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when a label should be voided
    /// </summary>
    public class VoidLabelPipeline : IShippingPanelTransientPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingManager shippingManager;
        private readonly IShippingErrorManager errorManager;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public VoidLabelPipeline(IMessenger messenger, ISchedulerProvider schedulerProvider,
            IMessageHelper messageHelper, IShippingManager shippingManager, IShippingErrorManager errorManager,
            Func<Type, ILog> logManager)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.messageHelper = messageHelper;
            this.shippingManager = shippingManager;
            this.errorManager = errorManager;
            log = logManager(typeof(VoidLabelPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messenger.OfType<VoidLabelMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .Where(x => messageHelper.ShowDialog(() => new ShipmentVoidConfirmDlg()) == DialogResult.OK)
                .SelectInBackgroundWithDialog(schedulerProvider, CreateProgressDialog, VoidShipment)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling processed shipment", ex))
                .Subscribe(HandleResult);
        }

        /// <summary>
        /// Handle the results of the void
        /// </summary>
        private void HandleResult(GenericResult<ICarrierShipmentAdapter> voidResult)
        {
            if (!voidResult.Success)
            {
                messageHelper.ShowError(voidResult.Message);
            }
            else if (voidResult.Value != null)
            {
                VoidShipmentResult voidShipmentResult = new VoidShipmentResult(voidResult.Value.Shipment);
                messenger.Send(new ShipmentsVoidedMessage(this, new[] { voidShipmentResult }));
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        private GenericResult<ICarrierShipmentAdapter> VoidShipment(VoidLabelMessage message)
        {
            return shippingManager.VoidShipment(message.ShipmentID, errorManager);
        }

        /// <summary>
        /// Create a progress dialog
        /// </summary>
        private IDisposable CreateProgressDialog()
        {
            return messageHelper.ShowProgressDialog("Voiding Shipments", "ShipWorks is voiding the shipments.");
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
