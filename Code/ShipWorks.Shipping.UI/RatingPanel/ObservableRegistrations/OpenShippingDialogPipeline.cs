using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when the shipping dialog opens
    /// </summary>
    public class OpenShippingDialogPipeline : IRatingPanelGlobalPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return messages.OfType<OpenShippingDialogMessage>()
                .Subscribe(_ =>
                {
                    viewModel.SetRateResults(Enumerable.Empty<RateResult>(), string.Empty, Enumerable.Empty<object>());
                    viewModel.IsLoading = false;
                });
        }
    }
}
