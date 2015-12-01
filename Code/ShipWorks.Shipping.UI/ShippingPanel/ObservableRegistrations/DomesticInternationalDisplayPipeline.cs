using Interapptive.Shared.Threading;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Display Domestic/International when the origin or destination changes
    /// </summary>
    public class DomesticInternationalDisplayPipeline : IShippingPanelObservableRegistration
    {
        bool forceDomesticInternationalChanged;
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
            // Merge the two address view models together so that we can respond to their property changed events
            return Observable.Merge(
                (new[] { viewModel.Origin, viewModel.Destination }).Select(
                    o => Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => o.PropertyChanged += h,
                        h => o.PropertyChanged -= h
                        )
                    ))
                    .Where(evt =>
                    {
                        string propertyName = evt.EventArgs.PropertyName;

                        bool handleField = propertyName.Equals(nameof(AddressViewModel.CountryCode), StringComparison.InvariantCultureIgnoreCase) ||
                                           propertyName.Equals(nameof(AddressViewModel.PostalCode), StringComparison.InvariantCultureIgnoreCase) ||
                                           propertyName.Equals(nameof(AddressViewModel.StateProvCode), StringComparison.InvariantCultureIgnoreCase);

                        // forceDomesticInternationalChanged is used for race conditions:
                        // For example (from rate criteria changing), ShipmentType property changes, and then before the throttle time, SupportsMultipleShipments changes.
                        // Since SupportsMultipleShipments isn't a rating field, the event would not be fired, even though
                        // ShipmentType changed and the event needs to be raised.
                        // So keep track that during the throttling a rate criteria was changed.
                        forceDomesticInternationalChanged = forceDomesticInternationalChanged || handleField;

                        return forceDomesticInternationalChanged;
                    })
                .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                .Subscribe(_ => SetDomesticInternationalText(viewModel));
        }

        /// <summary>
        /// Set the domestic international text
        /// </summary>
        public void SetDomesticInternationalText(ShippingPanelViewModel viewModel)
        {
            // Reset forceDomesticInternationalChanged so that we don't force it on the next round.
            forceDomesticInternationalChanged = false;

            viewModel.Destination.SaveToEntity(viewModel.ShipmentAdapter.Shipment.ShipPerson);
            viewModel.Origin.SaveToEntity(viewModel.ShipmentAdapter.Shipment.OriginPerson);

            viewModel.DomesticInternationalText = viewModel.ShipmentAdapter.IsDomestic ? "Domestic" : "International";
        }
    }
}
