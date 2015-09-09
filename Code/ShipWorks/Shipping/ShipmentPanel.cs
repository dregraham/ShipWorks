using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore;
using Autofac;
using System.Windows;

namespace ShipWorks.Shipping
{
    public enum WpfScreens
    {
        ShipmentPanel
    }

    public partial class ShipmentPanel : UserControl, IDockingPanelContent
    {
        public ShipmentPanel()
        {
            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            elementHost1.Child = IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<UIElement>(WpfScreens.ShipmentPanel);
        }

        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

        public void ChangeContent(IGridSelection selection)
        {
            //throw new NotImplementedException();
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
