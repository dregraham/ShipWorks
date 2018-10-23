using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.Controls.OrderConfirmationDialog
{
    /// <summary>
    /// View Model for confirming multiple matched orders
    /// </summary>
    [Component]
    public class OrderConfirmationViewModel : IOrderConfirmationViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private OrderEntity selectedOrder;
        private IEnumerable<OrderEntity> orders;

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderConfirmationViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderEntity SelectedOrder
        {
            get => selectedOrder;
            set => handler.Set(nameof(SelectedOrder), ref selectedOrder, value);
        }

        /// <summary>
        /// The list of orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<OrderEntity> Orders
        {
            get => orders;
            set
            {
                orders = value;
                SelectedOrder = orders.FirstOrDefault();
            }
        }
    }
}
