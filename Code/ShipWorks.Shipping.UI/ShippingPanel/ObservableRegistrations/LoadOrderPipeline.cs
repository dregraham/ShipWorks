using Interapptive.Shared.Threading;
using ShipWorks.Shipping.UI.MessageHandlers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Load order on selection changed pipeline
    /// </summary>
    public class LoadOrderOnSelectionChangedPipeline : IShippingPanelObservableRegistration
    {
        readonly OrderSelectionChangedHandler changeHandler;
        readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="changeHandler"></param>
        /// <param name="schedulerProvider"></param>
        public LoadOrderOnSelectionChangedPipeline(
            OrderSelectionChangedHandler changeHandler,
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
                    .Subscribe(_ => viewModel.AllowEditing = false),
                changeHandler.ShipmentLoadedStream()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Do(_ => viewModel.AllowEditing = true)
                    .Subscribe(viewModel.LoadOrder)
            );
        }
    }
}
