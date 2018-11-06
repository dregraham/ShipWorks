using System;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when a label should be created
    /// </summary>
    public class ShippingProfilePipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly IShippingProfileService shippingProfileService;
        private readonly ILog log;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfilePipeline(IObservable<IShipWorksMessage> messageStream,
            IShippingProfileService shippingProfileService,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager,
            IMessenger messenger)
        {
            this.messageStream = messageStream;
            this.shippingProfileService = shippingProfileService;
            this.schedulerProvider = schedulerProvider;
            this.messenger = messenger;
            log = logManager(typeof(ShippingProfilePipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ApplyProfileMessage>()
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .Select(x => shippingProfileService.Get(x.ProfileID).Apply(viewModel.Shipment))
                .ObserveOn(schedulerProvider.Dispatcher)
                .Do(x =>
                {
                    // Because now the profile can change the ShipmentType of the shipment
                    // we are mimicking the logic found in the ChangeShipmentTypePipeline
                    // If the user clicks to apply a profile but never gives focus to the
                    // shipping panel and the shipping panel never loses focus nothing saves
                    // the shipment, this forces the panel to refresh the new shipment and save it.
                    viewModel.LoadShipment(x);
                    viewModel.SaveToDatabase();

                    messenger.Send(new ShipmentChangedMessage(this, x, nameof(viewModel.ShipmentType)));
                })
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while applying profile to shipment", ex))
                .Subscribe();
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
