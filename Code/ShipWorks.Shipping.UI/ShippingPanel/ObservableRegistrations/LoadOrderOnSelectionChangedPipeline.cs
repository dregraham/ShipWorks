using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Shipping.UI.MessageHandlers;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Load order on selection changed pipeline
    /// </summary>
    public class LoadOrderOnSelectionChangedPipeline : IShippingPanelObservableRegistration
    {
        readonly IOrderSelectionChangedHandler changeHandler;
        readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoadOrderOnSelectionChangedPipeline(
            IOrderSelectionChangedHandler changeHandler,
            ISchedulerProvider schedulerProvider)
        {
            this.changeHandler = changeHandler;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return new CompositeDisposable(
                changeHandler.OrderChangingStream()
                    .Subscribe(message =>
                    {
                        // If the view model sent the message, it's to reload the order. So don't try saving first
                        if (message.Sender == viewModel)
                        {
                            viewModel.SaveToDatabase();
                        }

                        viewModel.AllowEditing = false;
                    }),
                changeHandler.ShipmentLoadedStream()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Do(_ => viewModel.AllowEditing = true)
                    .Subscribe(viewModel.LoadOrder)
            );
        }
    }
}
