using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using Common.Logging;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
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
        ShippingPanelViewModel viewModel;
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

            messenger = IoC.UnsafeGlobalLifetimeScope.Resolve<IMessenger>();
        }

        /// <summary>
        /// Handle control load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // We have to instantiate the view model in the load event to ensure that the MainForm is available
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();

            shippingPanelControl = new ShippingPanelControl(viewModel);
            viewModel.CommitBindings = CommitBindingsOnFocusedControl;

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
        public bool SupportsMultiSelect => true;

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
        private void OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // The other Focus events, like LostFocus, don't seem to work the way we need, but IsKeyBoardFocusWithinChanged does.
            // If the new value is false, meaning we had focus within this control and it's children and then lost it, and it wasn't already false,
            // save to the db.
            if (!((bool) e.NewValue) && e.NewValue != e.OldValue)
            {
                viewModel.SaveToDatabase();
            }
        }

        /// <summary>
        /// Commit any LostFocus bindings on the currently focused control
        /// </summary>
        private void CommitBindingsOnFocusedControl()
        {
            IInputElement focusedElement = FindFocusedInputElement(shippingPanelControl);
            CommitBindings(focusedElement);
        }

        /// <summary>
        /// Find the element that is currently focused within the given dependency object
        /// </summary>
        private IInputElement FindFocusedInputElement(DependencyObject container)
        {
            DependencyObject focusScope = FocusManager.GetFocusScope(shippingPanelControl);
            return focusScope == null ?
                null :
                FocusManager.GetFocusedElement(focusScope);
        }

        /// <summary>
        /// Commit the bindings on the given input element
        /// </summary>
        /// <remarks>This sends a lost focus event to force a control that has
        /// LostFocus bindings to commit the bindings</remarks>
        private void CommitBindings(IInputElement element)
        {
            element?.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent, element));
        }
    }
}
