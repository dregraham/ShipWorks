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
    public class ApplyProfilePipeline : IShippingPanelTransientPipeline
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
        public ApplyProfilePipeline(IObservable<IShipWorksMessage> messageStream,
            IShippingProfileService shippingProfileService,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager,
            IMessenger messenger)
        {
            this.messageStream = messageStream;
            this.shippingProfileService = shippingProfileService;
            this.schedulerProvider = schedulerProvider;
            this.messenger = messenger;
            log = logManager(typeof(ApplyProfilePipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ApplyProfileMessage>()
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .Select(x => 
                {
                    return shippingProfileService.Get(x.Profile.ShippingProfileID).Apply(viewModel.Shipment);
                })
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while applying profile to shipment", ex))
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(x => 
                {
                    viewModel.LoadShipment(x);
                    viewModel.SaveToDatabase();

                    messenger.Send(new ShipmentChangedMessage(this, x, nameof(viewModel.ShipmentType)));
                });
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
