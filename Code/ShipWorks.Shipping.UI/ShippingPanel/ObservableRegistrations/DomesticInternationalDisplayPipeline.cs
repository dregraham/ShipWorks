using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Shipping.Services;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Display Domestic/International when the origin or destination changes
    /// </summary>
    public class DomesticInternationalDisplayPipeline : IShippingPanelObservableRegistration
    {
        readonly HashSet<string> domesticAffectingProperties = new HashSet<string>
        {
            nameof(AddressViewModel.CountryCode),
            nameof(AddressViewModel.PostalCode),
            nameof(AddressViewModel.StateProvCode)
        };

        readonly ISchedulerProvider schedulerProvider;

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
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return Observable.Merge(
                    viewModel.Origin.PropertyChangeStream,
                    viewModel.Destination.PropertyChangeStream)
                .Where(domesticAffectingProperties.Contains)
                .Select(_ => viewModel.ShipmentAdapter)
                .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                .Subscribe(shipmentAdapter => SetDomesticInternationalText(viewModel, shipmentAdapter));
        }

        /// <summary>
        /// Set the domestic international text
        /// </summary>
        public void SetDomesticInternationalText(ShippingPanelViewModel viewModel, ICarrierShipmentAdapter shipmentAdapter)
        {
            if (!ReferenceEquals(viewModel.ShipmentAdapter, shipmentAdapter))
            {
                // If the shipment adapter has changed since we got notified of the property change, just bail
                return;
            }

            viewModel.DomesticInternationalText = viewModel.IsDomestic ? "Domestic" : "International";
        }
    }
}
