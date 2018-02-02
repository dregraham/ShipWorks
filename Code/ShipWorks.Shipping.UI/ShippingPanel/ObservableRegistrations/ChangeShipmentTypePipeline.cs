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
                    // We need to get a reference to the view model before loading the shipment because loading the shipment
                    // will dispose the lifetimeScope that owns the createInsuranceBehaviorChangeViewModel func
                    var insuranceViewModel = createInsuranceBehaviorChangeViewModel().Value;

                    viewModel.LoadShipment(x.adapter, nameof(viewModel.ShipmentType));
                    viewModel.SaveToDatabase();

                    // Show the notification after the view model is fully loaded and saved to avoid race conditions with other pipelines
                    var newInsuranceSelection = x.adapter.GetPackageAdapters().Any(p => p.InsuranceChoice.Insured);
                    insuranceViewModel.Notify(x.originalInsuranceSelection, newInsuranceSelection);

                    messenger.Send(new ShipmentChangedMessage(this, x.adapter, nameof(viewModel.ShipmentType)));
                });
        }

        /// <summary>
        /// Get a shipping adapter from the changed shipment type
        /// </summary>
        private (ICarrierShipmentAdapter adapter, bool originalInsuranceSelection) ChangeShipmentType(ShippingPanelViewModel viewModel)
        {
            bool originalInsuranceSelection = viewModel.Shipment.Insurance;
            var shipmentAdapter = shippingManager.ChangeShipmentType(viewModel.ShipmentType, viewModel.Shipment);
            return (shipmentAdapter, originalInsuranceSelection);
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
