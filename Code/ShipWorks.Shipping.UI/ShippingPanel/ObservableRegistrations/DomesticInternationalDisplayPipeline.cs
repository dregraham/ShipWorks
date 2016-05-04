using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Threading;
using ShipWorks.Shipping.Services;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Display Domestic/International when the origin or destination changes
    /// </summary>
    public class DomesticInternationalDisplayPipeline : IShippingPanelTransientPipeline
    {
        readonly HashSet<string> domesticAffectingProperties = new HashSet<string>
        {
            nameof(AddressViewModel.CountryCode),
            nameof(AddressViewModel.PostalCode),
            nameof(AddressViewModel.StateProvCode)
        };

        readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public DomesticInternationalDisplayPipeline(ISchedulerProvider schedulerProvider)
        {
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            IObservable<string> propertyChangeStreams = Observable.Merge(
                    viewModel.Origin.PropertyChangeStream,
                    viewModel.Destination.PropertyChangeStream);

            IDisposable updateViewModel = propertyChangeStreams
                .Where(domesticAffectingProperties.Contains)
                .Where(_ => !viewModel.IsLoadingShipment)
                .Select(_ => viewModel.ShipmentAdapter)
                .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(shipmentAdapter => Update(viewModel, shipmentAdapter));

            IDisposable updateText = propertyChangeStreams
                .Where(domesticAffectingProperties.Contains)
                .Subscribe(_ => viewModel.DomesticInternationalText = viewModel.IsDomestic ? "Domestic" : "International");

            subscription = new CompositeDisposable(updateViewModel, updateText);
        }

        /// <summary>
        /// Update the person adapters and set DomesticInternationalText appropriately
        /// </summary>
        private void Update(ShippingPanelViewModel viewModel, ICarrierShipmentAdapter shipmentAdapter)
        {
            if (!ReferenceEquals(viewModel.ShipmentAdapter, shipmentAdapter))
            {
                // If the shipment adapter has changed since we got notified of the property change, just bail
                return;
            }

            if (shipmentAdapter?.Shipment?.OriginPerson != null)
            {
                UpdatePersonalAdapterValues(shipmentAdapter.Shipment.OriginPerson, viewModel.Origin);
            }

            if (shipmentAdapter?.Shipment?.ShipPerson != null)
            {
                UpdatePersonalAdapterValues(shipmentAdapter.Shipment.ShipPerson, viewModel.Destination);
            }

            viewModel.UpdateServices();
            viewModel.RefreshInsurance();
        }

        /// <summary>
        /// Update a person adapter based on the address view model
        /// </summary>
        private void UpdatePersonalAdapterValues(PersonAdapter shipmentPersonAdapter, AddressViewModel addressViewModel)
        {
            shipmentPersonAdapter.StateProvCode = Geography.GetStateProvCode(addressViewModel.StateProvCode);
            shipmentPersonAdapter.PostalCode = addressViewModel.PostalCode;
            shipmentPersonAdapter.CountryCode = addressViewModel.CountryCode;
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
