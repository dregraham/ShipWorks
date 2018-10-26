using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.OrderItems
{
    /// <summary>
    /// View model for OrderItemsControl
    /// </summary>
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.Amazon)]
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.FedEx)]
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IOrderItemsViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(OrderItemsControl))]
    public class OrderItemsViewModel : IOrderItemsViewModel
    {
#pragma warning disable CS0067 // Defined in interface, but we don't need to track ProperyChanged
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067 // Defined in interface, but we don't need to track ProperyChanged

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemsViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = true;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Order Items";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// The order lookup ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Does the order item exist
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool OrderItemExists => ShipmentModel?.SelectedOrder?.OrderItems?.Any() == true;

        /// <summary>
        /// Dispose the view model
        /// </summary>
        /// <remarks>
        /// This was left unimplemented so that IOrderLookupViewModel can be reused.
        /// </remarks>
        public void Dispose()
        {
        }
    }
}
