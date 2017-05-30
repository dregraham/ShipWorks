using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Allows validator to show validation messages when a shipment is processed.
    /// </summary>
    public class UpsLocalRatingPipeline : IInitializeForCurrentUISession
    {
        private readonly ISchedulerProvider scheduleProvider;
        private readonly IUpsLocalRateValidator validator;

        private readonly IConnectableObservable<ShipmentsProcessedMessage> messages;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRatingPipeline(ISchedulerProvider scheduleProvider,
            IMessenger messenger,
            IUpsLocalRateValidator validator)
        {
            this.scheduleProvider = scheduleProvider;
            this.validator = validator;

            messages = messenger.OfType<ShipmentsProcessedMessage>().Publish();
            subscription = messages.Connect();
        }

        /// <summary>
        /// Subscribe to the ShipmentsProcessedMessage
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // Given a ShipmentsProcessedMessage 
            subscription = messages
                // Bail if message doesn't come from the ShippingDlg
                .Where(m => !(m.Sender is IShippingDlg))
                // Select all the successfully processed shipments
                .Select(m => m.Shipments.Where(result => result.IsSuccessful).Select(result => result.Shipment))
                // Bail if there were no successfully processed shipments
                .Where(shipments => shipments.Any())
                // Call validate on the successfully processed shipments
                .Select(shipments => validator.Validate(shipments))
                // Check to see if there is a message to display
                .Where(r => !string.IsNullOrWhiteSpace(r.Message))
                // Go to the UI thread to display the message
                .ObserveOn(scheduleProvider.WindowsFormsEventLoop)
                // Tell the validator to display the message
                .Subscribe(validationResult => validationResult.ShowMessage());
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            subscription.Dispose();
        }

        /// <summary>
        /// Disposes the subscription
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}
