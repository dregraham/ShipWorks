using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipment type
    /// </summary>
    public class ChangeShipmentTypePipeline : IShippingPanelTransientPipeline
    {
        private readonly ILog log;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly Func<Owned<IInsuranceBehaviorChangeViewModel>> createInsuranceBehaviorChangeViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeShipmentTypePipeline(IShippingManager shippingManager,
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            Func<Owned<IInsuranceBehaviorChangeViewModel>> createInsuranceBehaviorChangeViewModel,
            Func<Type, ILog> logFactory)
        {
            this.createInsuranceBehaviorChangeViewModel = createInsuranceBehaviorChangeViewModel;
            log = logFactory(typeof(ChangeShipmentTypePipeline));
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = viewModel.PropertyChangeStream
                .Where(x => x == nameof(viewModel.ShipmentType))
                .Where(x => viewModel.Shipment != null && viewModel.ShipmentStatus == ShipmentStatus.Unprocessed)
                .Select(_ => ChangeShipmentType(viewModel))
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while changing shipment types", ex))
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(x =>
                {
                    viewModel.LoadShipment(x, nameof(viewModel.ShipmentType));
                    viewModel.SaveToDatabase();
                    messenger.Send(new ShipmentChangedMessage(this, x, nameof(viewModel.ShipmentType)));
                });
        }

        /// <summary>
        /// Get a shipping adapter from the changed shipment type
        /// </summary>
        private ICarrierShipmentAdapter ChangeShipmentType(ShippingPanelViewModel viewModel)
        {
            bool originalInsuranceSelection = viewModel.Shipment.Insurance;
            var shipmentAdapter = shippingManager.ChangeShipmentType(viewModel.ShipmentType, viewModel.Shipment);
            var newInsuranceSelection = shipmentAdapter.GetPackageAdapters().Any(x => x.InsuranceChoice.Insured);
            createInsuranceBehaviorChangeViewModel().Value.Notify(originalInsuranceSelection, newInsuranceSelection);

            return shipmentAdapter;
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
