using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Common.Logging;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Shipment panel container
    /// </summary>
    public partial class ShippingPanel : UserControl, IDockingPanelContent
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingPanel));
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

            shipmentPanelelementHost.Child = shippingPanelControl;

            shippingPanelControl.IsKeyboardFocusWithinChanged += OnIsKeyboardFocusWithinChanged;
        }

        /// <summary>
        /// Type of entity control supports
        /// </summary>
        public EntityType EntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Which targets the control supports
        /// </summary>
        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        /// <summary>
        /// Does the control support multi select
        /// </summary>
        public bool SupportsMultiSelect => false;

        /// <summary>
        /// Change the content of the control
        /// </summary>
        public Task ChangeContent(IGridSelection selection)
        {
            return TaskUtility.CompletedTask;
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
        
        /// <summary>
        /// Saves the shipment to the database when the shipping panel loses focus.
        /// </summary>
        private void OnIsKeyboardFocusWithinChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            // The other Focus events, like LostFocus, don't seem to work the way we need, but IsKeyBoardFocusWithinChanged does.
            // If the new value is false, meaning we had focus within this control and it's children and then lost it, and it wasn't already false,
            // save to the db.
            if (!((bool) e.NewValue) && e.NewValue != e.OldValue)
            {
                SaveToDatabase();
            }
        }

        /// <summary>
        /// Tell the view model to save to the database
        /// </summary>
        private void SaveToDatabase()
        {
            viewModel.SaveToDatabase();
        }
    }
}
