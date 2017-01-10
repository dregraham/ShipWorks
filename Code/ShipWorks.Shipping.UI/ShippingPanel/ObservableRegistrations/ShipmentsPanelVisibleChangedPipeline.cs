using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore;
using ShipWorks.Messaging.Messages.Panels;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the shipments panel opening and closing
    /// </summary>
    public class ShipmentsPanelVisibleChangedPipeline : IShippingPanelGlobalPipeline
    {
        private readonly IObservable<IShipWorksMessage> messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsPanelVisibleChangedPipeline(IObservable<IShipWorksMessage> messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the handler pipelines
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            IDisposable panelShown = messenger.OfType<PanelShownMessage>()
                .Where(x => DockPanelIdentifiers.IsShipmentsPanel(x.Panel))
                .Subscribe(_ => viewModel.IsShipmentsPanelHidden = false);

            IDisposable panelHidden = messenger.OfType<PanelHiddenMessage>()
                .Where(x => DockPanelIdentifiers.IsShipmentsPanel(x.Panel))
                .Subscribe(_ => viewModel.IsShipmentsPanelHidden = true);

            return new CompositeDisposable(panelShown, panelHidden);
        }
    }
}
