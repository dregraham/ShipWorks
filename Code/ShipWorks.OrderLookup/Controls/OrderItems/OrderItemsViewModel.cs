using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
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
    public class OrderItemsViewModel : OrderLookupViewModelBase, IOrderItemsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemsViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {

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
        public override string Title { get; protected set; } = "Order Items";

        /// <summary>
        /// Field layout repository
        /// </summary>
        public IOrderLookupFieldLayoutProvider FieldLayoutProvider => ShipmentModel.FieldLayoutProvider;

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.Items;

        /// <summary>
        /// Does the order item exist
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool OrderItemExists => ShipmentModel?.SelectedOrder?.OrderItems?.Any() == true;
    }
}
