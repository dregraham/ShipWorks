using System;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Listens for CreateLable ShortcutMessage and sends a CreateLabelMessage when it receives one
    /// </summary>
    public class LabelShortcutPipeline : IShippingPanelTransientPipeline
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelShortcutPipeline(IMessenger messenger, IMainForm mainForm, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.CreateLabel))
                .Where(m => viewModel.Shipment != null)
                .Where(m => !viewModel.Shipment.Processed)
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Where(_ => !mainForm.AdditionalFormsOpen())
                .Subscribe(m =>
                {
                    // This causes the shipping panel to save if there are unsaved changes
                    mainForm.Focus();
 
                    messenger.Send(new CreateLabelMessage(this, viewModel.Shipment.ShipmentID));
                });
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose() => subscription?.Dispose();

    }
}