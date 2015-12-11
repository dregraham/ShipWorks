using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipment type
    /// </summary>
    public class ChangeShipmentTypePipeline : IShippingPanelObservableRegistration
    {
        private readonly ILog log;
        private readonly IShippingManager shippingManager;
        //private IDisposable subscription;
        //public ShippingPanelViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeShipmentTypePipeline(IShippingManager shippingManager, Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(ChangeShipmentTypePipeline));
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return viewModel.PropertyChangeStream
                .Where(x => x == nameof(viewModel.ShipmentType))
                .Where(x => viewModel.IsProcessed.HasValue && !viewModel.IsProcessed.Value)
                .Select(_ => ChangeShipmentType(viewModel))
                .SubscribeWithRetry(x => viewModel.Populate(x), HandleException);



            //viewModel = registrationViewModel;

            //CreateSubscription();

            //return Disposable.Create(() => subscription?.Dispose());
        }

        ///// <summary>
        ///// Create the subscription that will handle changing the shipment type
        ///// </summary>
        //private void CreateSubscription()
        //{
        //    subscription = viewModel.PropertyChangeStream
        //        .Where(x => x == nameof(viewModel.ShipmentType))
        //        .Where(x => viewModel.IsProcessed.HasValue && !viewModel.IsProcessed.Value)
        //        .Select(_ => ChangeShipmentType())
        //        .Subscribe(x => viewModel.Populate(x), HandleException);
        //}

        /// <summary>
        /// Get a shipping adapter from the changed shipment type
        /// </summary>
        private ICarrierShipmentAdapter ChangeShipmentType(ShippingPanelViewModel viewModel) =>
            shippingManager.ChangeShipmentType(viewModel.ShipmentType, viewModel.Shipment);

        /// <summary>
        /// Handle an exception raised while changing the shipment type
        /// </summary>
        private bool HandleException(Exception ex)
        {
            log.Error(ex);
            return true;
        }
    }
}
