using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IOrderLookup))]
    public partial class OrderLookupControlHost : UserControl, IOrderLookup
    {
        private readonly OrderLookupViewModel orderLookupViewModel;
        private OrderLookupControl orderLookupControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControlHost(OrderLookupViewModel orderLookupViewModel)
        {
            InitializeComponent();
            this.orderLookupViewModel = orderLookupViewModel;
        }

        /// <summary>
        /// Set the element host on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Dock = DockStyle.Fill;

            base.OnLoad(e);

            orderLookupControl = new OrderLookupControl()
            {
                DataContext = orderLookupViewModel
            };
            orderLookupControl.IsKeyboardFocusWithinChanged += OnIsKeyboardFocusWithinChanged;
            orderLookupControl.LostFocus += OnShippingPanelControlLostFocus;

            ElementHost host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = orderLookupControl
            };

            Controls.Add(host);
        }

        /// <summary>
        /// Unload the order from the viewmodel orchestrator
        /// </summary>
        public void Unload() => orderLookupViewModel.Orchestrator.Unload();

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;

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
                //orderLookupViewModel.Orchestrator.SaveToDatabase();
            }
        }

        /// <summary>
        /// The shipping panel has lost focus
        /// </summary>
        private void OnShippingPanelControlLostFocus(object sender, RoutedEventArgs e)
        {
            if (IsNonKeyboardInputElement(e.OriginalSource))
            {
                //orderLookupViewModel.Orchestrator.SaveToDatabase();
            }
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
            IInputElement focusedElement = FindFocusedInputElement(orderLookupControl);
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
            DependencyObject focusScope = FocusManager.GetFocusScope(orderLookupControl);
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
