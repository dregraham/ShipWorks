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

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Shipment panel container
    /// </summary>
    public partial class ShippingPanel : UserControl, IDockingPanelContent
    {
        ShippingPanelControl shipmentPanelControl;
        ShippingPanelViewModel viewModel;

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

            shipmentPanelControl = new ShippingPanelControl(viewModel);
            shipmentPanelelementHost.Child = shipmentPanelControl;
        }
        
        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public async void ChangeContent(IGridSelection selection)
        {
            OrderEntity entity = await TaskEx.Run(() => DataProvider.GetEntity(selection.Keys.FirstOrDefault()) as OrderEntity);

            viewModel.LoadOrder(entity);
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
    }
}
