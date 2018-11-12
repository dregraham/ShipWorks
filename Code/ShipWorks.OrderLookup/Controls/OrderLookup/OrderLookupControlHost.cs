using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IOrderLookup))]
    public partial class OrderLookupControlHost : UserControl, IOrderLookup
    {
        private readonly MainOrderLookupViewModel orderLookupViewModel;
        private MainOrderLookupControl mainOrderLookupControl;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControlHost(MainOrderLookupViewModel orderLookupViewModel, OrderLookupLabelShortcutPipeline shortcutPipeline, IMessenger messenger)
        {
            this.messenger = messenger;
            InitializeComponent();
            this.orderLookupViewModel = orderLookupViewModel;
            orderLookupViewModel.ShipmentModel.ShipmentNeedsBinding += OnShipmentModelShipmentSaving;
            orderLookupViewModel.ShipmentModel.CanAcceptFocus = () => this.Visible && this.CanFocus;
            shortcutPipeline.Register(orderLookupViewModel.ShipmentModel);
        }

        /// <summary>
        /// A shipment is about to be saved
        /// </summary>
        private void OnShipmentModelShipmentSaving(object sender, EventArgs e) => CommitBindingsOnFocusedControl();

        /// <summary>
        /// Set the element host on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Dock = DockStyle.Fill;

            base.OnLoad(e);

            mainOrderLookupControl = new MainOrderLookupControl()
            {
                DataContext = orderLookupViewModel
            };

            ElementHost host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = mainOrderLookupControl
            };

            Controls.Add(host);
        }

        /// <summary>
        /// Unload the order from the viewmodel shipmentModel
        /// </summary>
        public void Unload()
        {
            CommitBindingsOnFocusedControl();
            orderLookupViewModel.ShipmentModel.Unload();
        }

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        public async Task CreateLabel()
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
                    Observable.Return(Unit.Default).Where(_ => !orderLookupViewModel.ShipmentModel.CreateLabel())
                })
                .FirstAsync();
        }

        /// <summary>
        /// Register the profile handler
        /// </summary>
        public void RegisterProfileHandler(Func<Func<ShipmentTypeCode?>, Action<IShippingProfile>, IDisposable> profileRegistration) =>
            orderLookupViewModel.ShipmentModel.RegisterProfileHandler(profileRegistration);

        /// <summary>
        /// Allow the creation of a label
        /// </summary>
        public bool CreateLabelAllowed()
        {
            return orderLookupViewModel.ShipmentModel?.ShipmentAdapter?.Shipment?.Processed == false;
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;

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
            IInputElement focusedElement = FindFocusedInputElement(mainOrderLookupControl);
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
            DependencyObject focusScope = FocusManager.GetFocusScope(mainOrderLookupControl);
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
