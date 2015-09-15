using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipWorks.Data;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;

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

        public void ChangeContent(IGridSelection selection)
        {
            viewModel.LoadOrder(selection.Keys.FirstOrDefault());
        }

        public void LoadState()
        {
            //throw new NotImplementedException();
        }

        public void ReloadContent()
        {
            //throw new NotImplementedException();
        }

        public void SaveState()
        {
            //throw new NotImplementedException();
        }

        public void UpdateContent()
        {
            //throw new NotImplementedException();
        }

        public void UpdateStoreDependentUI()
        {
            //throw new NotImplementedException();
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
