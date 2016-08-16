using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Load customs when the address changes
    /// </summary>
    public class LoadCustomsPipeline : IShippingPanelTransientPipeline
    {
        private IDisposable subscription;
        private readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoadCustomsPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = new CompositeDisposable(
                LoadCustomsWhenAddressChanges(viewModel, x => x.ShipmentViewModel, x => x.Origin, x => viewModel.Shipment?.OriginPerson),
                LoadCustomsWhenAddressChanges(viewModel, x => x.ShipmentViewModel, x => x.Destination, x => viewModel.Shipment?.ShipPerson)
            );
        }

        /// <summary>
        /// Load customs when address properties change
        /// </summary>
        private IDisposable LoadCustomsWhenAddressChanges(ShippingPanelViewModel viewModel,
            Func<ShippingPanelViewModel, IShipmentViewModel> shipmentViewModel,
            Func<ShippingPanelViewModel, AddressViewModel> model,
            Func<string, PersonAdapter> getAdapter)
        {
            return model(viewModel).PropertyChangeStream
                .Where(_ => !viewModel.IsLoadingShipment)
                .Select(getAdapter)
                .Where(person => person != null)
                .Subscribe(person =>
                {
                    model(viewModel).SaveToEntity(person);
                    shipmentViewModel(viewModel).LoadCustoms();
                });
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
