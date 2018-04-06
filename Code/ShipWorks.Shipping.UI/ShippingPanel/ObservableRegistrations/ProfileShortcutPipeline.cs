using System;
using System.Reactive.Linq;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Listens for applyprofile keyboard shortcuts and sends an ApplyProfileMessage when it receives one
    /// </summary>
    public class ProfileShortcutPipeline : IShippingPanelTransientPipeline
    {
        private readonly IMessenger messenger;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileShortcutPipeline(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyProfile))
                .Where(m => viewModel.Shipment != null)
                .Subscribe(m => messenger.Send(new ApplyProfileMessage(
                    this, viewModel.Shipment.ShipmentID, m.Shortcut.RelatedObjectID.Value)));
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