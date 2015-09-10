using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using System;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Shipment panel container
    /// </summary>
    public partial class ShipmentPanel : UserControl, IDockingPanelContent
    {
        ShipmentPanelControl shipmentPanelControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            shipmentPanelControl = new ShipmentPanelControl();
            shipmentPanelelementHost.Child = shipmentPanelControl;
        }
        
        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public void ChangeContent(IGridSelection selection)
        {
            ShipmentPanelViewModel model = IoC.UnsafeGlobalLifetimeScope.Resolve<ShipmentPanelViewModel>();
            model.LoadOrder(new OrderEntity());
            shipmentPanelControl.ViewModel = model;
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
