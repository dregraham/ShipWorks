using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IOrderLookup))]
    public partial class ScanToShipControlHost : UserControl, IOrderLookup
    {
        private ScanToShipControl scanToShipControl;
        private readonly IScanToShipViewModel scanToShipViewModel;
        private readonly IMessenger messenger;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipControlHost(IScanToShipViewModel scanToShipViewModel, IMessenger messenger, IShippingManager shippingManager)
        {
            this.messenger = messenger;
            InitializeComponent();

            this.scanToShipViewModel = scanToShipViewModel;
            this.scanToShipViewModel.OrderLookupViewModel.ShipmentModel.ShipmentNeedsBinding += OnShipmentModelShipmentSaving;
            this.scanToShipViewModel.OrderLookupViewModel.ShipmentModel.CanAcceptFocus = () => this.Visible && this.CanFocus;
            this.scanToShipViewModel.OrderLookupViewModel.ShipmentModel.CreateLabelWrapper = CreateLabelWrapper;

            this.scanToShipViewModel.ScanPackViewModel.CanAcceptFocus = () => Visible && CanFocus;
            this.shippingManager = shippingManager;
        }

        public OrderEntity Order => scanToShipViewModel.OrderLookupViewModel.ShipmentModel.SelectedOrder;

        /// <summary>
        /// A shipment is about to be saved
        /// </summary>
        private void OnShipmentModelShipmentSaving(object sender, EventArgs e) => CommitBindingsOnFocusedControl();

        /// <summary>
        /// Set the element host on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            Dock = DockStyle.Fill;

            base.OnLoad(e);
            scanToShipControl = new ScanToShipControl()
            {
                DataContext = scanToShipViewModel
            };

            ElementHost host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = scanToShipControl
            };

            Controls.Add(host);
            Visible = true;
        }

        /// <summary>
        /// Unload the order from the viewmodel shipmentModel
        /// </summary>
        public void Unload()
        {
            CommitBindingsOnFocusedControl();
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel.Unload();
            scanToShipViewModel.ScanPackViewModel.Reset();
        }

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        public Task CreateLabel() =>
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel.CreateLabel();

        /// <summary>
        /// Allow reshipping an order
        /// </summary>
        public bool ShipAgainAllowed() =>
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.ShipmentAdapter?.Shipment?.Processed == true;

        /// <summary>
        /// Allow reprint label
        /// </summary>
        public bool ReprintAllowed() => ShipAgainAllowed();


        /// <summary>
        /// Ship the shipment again
        /// </summary>
        public void ShipAgain()
        {
            long? shipmentId = scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.ShipmentAdapter?.Shipment?.ShipmentID;

            if (shipmentId != 0 && shipmentId != null)
            {
                messenger.Send(new OrderLookupShipAgainMessage(this, shipmentId.Value));
            }
        }

        /// <summary>
        /// Unverify the order
        /// </summary>
        public void Unverify()
        {
            long? orderId = scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.SelectedOrder?.OrderID;

            if (orderId.HasValue && orderId.Value != 0)
            {
                messenger.Send(new OrderLookupUnverifyMessage(this, orderId.Value));
            }
        }

        /// <summary>
        /// Unverify the order
        /// </summary>
        public void Reprint()
        {
            ShipmentEntity shipment = scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.ShipmentAdapter?.Shipment;
            long? shipmentId = shipment?.ShipmentID;

            if (shipmentId.HasValue && shipmentId != 0)
            {
                shippingManager.RefreshShipment(shipment);
                messenger.Send(new ReprintLabelsMessage(this, new[] { shipment }), string.Empty);
            }
            
        }

        /// <summary>
        /// Register the profile handler
        /// </summary>
        public void RegisterProfileHandler(Func<Func<ShipmentEntity>, Action<IShippingProfile>, IDisposable> profileRegistration)
        {
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel.RegisterProfileHandler(profileRegistration);
        }

        /// <summary>
        /// Save the order
        /// </summary>
        public void Save()
        {
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel.SaveToDatabase();
        }

        /// <summary>
        /// Create a new shipment for the order loaded in scan to ship mode
        /// </summary>
        public void CreateNewShipment()
        {
            long? orderId = scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.SelectedOrder?.OrderID;

            if (orderId.HasValue && orderId.Value != 0)
            {
                messenger.Send(new ScanToShipCreateShipmentMessage(this, orderId.Value));
            }
        }

        /// <summary>
        /// Whether or not creating a new shipment is allowed
        /// </summary>
        public bool CreateNewShipmentAllowed() =>
            scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.SelectedOrder != null;

        /// <summary>
        /// Allow the creation of a label
        /// </summary>
        public bool CreateLabelAllowed()
        {
            return scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.ShipmentAdapter?.Shipment?.Processed == false;
        }

        /// <summary>
        /// Allow unverify order
        /// </summary>
        public bool UnverifyOrderAllowed()
        {
            var order = scanToShipViewModel.OrderLookupViewModel.ShipmentModel?.SelectedOrder;
            
            // Return true if the order verified and there are no processed shipments
            return order?.Verified == true && order?.Shipments?.Any(s => s.Processed) == false ;
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        private async Task CreateLabelWrapper(Func<bool> createLabel)
        {
            CommitBindingsOnFocusedControl();

            // Wait for one of the following scenarios:
            await Observable.Merge(new[]
                {
                    // The label creation process has completed
                    messenger.OfType<ShipmentsProcessedMessage>().Select(_ => Unit.Default),
                    // Time out after 30 seconds
                    Observable.Timer(TimeSpan.FromSeconds(30)).Select(_ => Unit.Default),
                    // The create label process didn't start
                    Observable.Return(Unit.Default).Where(_ => !createLabel())
                })
                .FirstAsync();
        }

        /// <summary>
        /// Is the object an input element that does not hold keyboard focus
        /// </summary>
        private bool IsNonKeyboardInputElement(object element) =>
            element is System.Windows.Controls.Button;

        /// <summary>
        /// Commit any LostFocus bindings on the currently focused control
        /// </summary>
        private void CommitBindingsOnFocusedControl()
        {
            IInputElement focusedElement = FindFocusedInputElement(scanToShipControl);
            if (!IsNonKeyboardInputElement(focusedElement))
            {
                CommitBindings(focusedElement);
            }
        }

        /// <summary>
        /// Find the element that is currently focused within the given dependency object
        /// </summary>
        private IInputElement FindFocusedInputElement(DependencyObject container)
        {
            DependencyObject focusScope = null;

            if (container != null)
            {
                focusScope = FocusManager.GetFocusScope(container);
            }

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
