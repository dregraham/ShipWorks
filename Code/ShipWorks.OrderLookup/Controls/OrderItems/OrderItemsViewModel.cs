using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.OrderItems
{
    /// <summary>
    /// View model for OrderItemControl
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
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemsViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
        public bool OrderItemExists => ShipmentModel.SelectedOrder.OrderItems.Any();

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }

        /// <summary>
        /// Dispose the view model
        /// </summary>
        public void Dispose() => ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}
