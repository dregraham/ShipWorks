using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Shipping.UI.MessageHandlers;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Load order on selection changed pipeline
    /// </summary>
    public class LoadOrderOnSelectionChangedPipeline : IShippingPanelGlobalPipeline
    {
        readonly IOrderSelectionChangedHandler changeHandler;
        readonly ILog log;
        readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoadOrderOnSelectionChangedPipeline(
            IOrderSelectionChangedHandler changeHandler,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager)
        {
            this.changeHandler = changeHandler;
            this.schedulerProvider = schedulerProvider;
            log = logManager(typeof(LoadOrderOnSelectionChangedPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return new CompositeDisposable(
                changeHandler.OrderChangingStream()
                    .Do(message =>
                    {
                        viewModel.IsLoading = true;

                        // If the view model sent the message, it's to reload the order. So don't try saving first
                        if (message.Sender != viewModel)
                        {
                            viewModel.SaveToDatabase();
                        }

                        viewModel.AllowEditing = false;

                        viewModel.UnloadShipment();
                    })
                    .CatchAndContinue((Exception ex) => log.Error("An error occurred while selecting an order", ex))
                    .Subscribe(_ => { }),
                changeHandler.ShipmentLoadedStream()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Do(_ => viewModel.AllowEditing = true)
                    .CatchAndContinue((Exception ex) => log.Error("An error occurred while loading order selection", ex))
                    .Subscribe(viewModel.LoadOrder)
            );
        }
    }
}
