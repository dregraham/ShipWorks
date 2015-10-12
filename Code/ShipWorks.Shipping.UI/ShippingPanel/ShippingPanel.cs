using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Common.Logging;
using ShipWorks.Core.Messaging;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Shipping.UI.MessageHandlers;

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
        private IDisposable subscription;

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

            subscription = messenger.AsObservable<CreateLabelMessage>().Subscribe(HandleCreateLabelMessage);
        }

        public EntityType EntityType => EntityType.ShipmentEntity;

        public FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Shipments };

        public bool SupportsMultiSelect => false;

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

        private async void HandleCreateLabelMessage(CreateLabelMessage message)
        {
            await viewModel.ProcessShipment();
        }

        /// <summary>
        /// Saves the shipment to the database when the shipping panel loses focus.
        /// </summary>
        private void OnIsKeyboardFocusWithinChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            // The other Focus events, like LostFocus, don't seem to work the way we need, but IsKeyBoardFocusWithinChanged does.
            // If the new value is false, meaning we had focus within this control and it's children and then lost it, and it wasn't already false,
            // save to the db.
            if (!((bool)e.NewValue) && e.NewValue != e.OldValue)
            {
                viewModel?.SaveToDatabase();
            }
        }
    }
}
