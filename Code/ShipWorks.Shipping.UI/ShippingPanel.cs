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

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Shipment panel container
    /// </summary>
    public partial class ShippingPanel : UserControl, IDockingPanelContent
    {
        ShippingPanelControl shipmentPanelControl;
        ShippingPanelViewModel viewModel;
        IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();
            messenger = IoC.UnsafeGlobalLifetimeScope.Resolve<IMessenger>();

            shipmentPanelControl = new ShippingPanelControl(viewModel);
            shipmentPanelelementHost.Child = shipmentPanelControl;

            messenger.Handle<CreateLabelMessage>(this, HandleCreateLabelMessage);
        }
        
        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public Task ChangeContent(IGridSelection selection)
        {
            viewModel.SaveToDatabase();
            return viewModel.LoadOrder(selection.Keys.FirstOrDefault());
        }

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
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope(ConfigureShippingDialogDependencies))
            {
                ShipmentProcessor shipmentProcessor = lifetimeScope.Resolve<ShipmentProcessor>();
                CarrierConfigurationShipmentRefresher refresher = lifetimeScope.Resolve<CarrierConfigurationShipmentRefresher>();

                await viewModel.ProcessShipment(shipmentProcessor, refresher);
            }
        }

        /// <summary>
        /// Configure extra dependencies for the shipping dialog
        /// </summary>
        private void ConfigureShippingDialogDependencies(ContainerBuilder builder)
        {
            builder.RegisterInstance(Program.MainForm)
                .As<Control>()
                .ExternallyOwned();
        }
    }
}
