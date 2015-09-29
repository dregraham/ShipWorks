using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.Common.Threading;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Shipment panel container
    /// </summary>
    public partial class ShippingPanel : UserControl, IDockingPanelContent
    {
        ShippingPanelControl shippingPanelControl;
        readonly ShippingPanelViewModel viewModel;
        readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanel()
        {
            InitializeComponent();

            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();
            messenger = IoC.UnsafeGlobalLifetimeScope.Resolve<IMessenger>();
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            shippingPanelControl = new ShippingPanelControl(viewModel);
            shippingPanelControl.LostFocus += OnShippingPanelControlLostFocus;
            shipmentPanelelementHost.Child = shippingPanelControl;

            messenger.Handle<CreateLabelMessage>(this, HandleCreateLabelMessage);
        }

        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public Task ChangeContent(IGridSelection selection) =>
            viewModel.LoadOrder(selection.Keys.FirstOrDefault());

        public void LoadState()
        {
            // Panel doesn't have any extra state
        }

        public Task ReloadContent()
        {
            return TaskUtility.CompletedTask;
        }

        public void SaveState()
        {
            // Panel doesn't have any extra state
        }

        public Task UpdateContent()
        {
            return TaskUtility.CompletedTask;
        }

        public void UpdateStoreDependentUI()
        {
            // There is no store dependent ui
        }

        private async void HandleCreateLabelMessage(CreateLabelMessage message)
        {
            await viewModel.ProcessShipment();
        }

        /// <summary>
        /// The shipping panel has lost focus
        /// </summary>
        private void OnShippingPanelControlLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!shippingPanelControl.IsKeyboardFocusWithin)
            {
                viewModel?.SaveToDatabase();
            }
        }
    }
}
