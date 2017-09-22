using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// When rates are retrieved Updates viewModel services if the viewModel's shipmentType is Amazon
    /// </summary>
    public class RatesRetrievedPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messages;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrievedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<RatesRetrievedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(_ => UpdateAmazonServices(viewModel));
        }

        /// <summary>
        /// Updates viewModel services if the viewModel's shipmentType is Amazon
        /// </summary>
        private void UpdateAmazonServices(ShippingPanelViewModel viewModel)
        {
            if (viewModel.ShipmentType == ShipmentTypeCode.Amazon)
            {
                viewModel.UpdateServices();
            }
        }

        /// <summary>
        /// Dispose of subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
