using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;

namespace ShipWorks.Stores.UI.Orders.Split
{
    /// <summary>
    /// OrderSplitViewModel for design mode
    /// </summary>
    public class DesignModeOrderSplitViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeOrderSplitViewModel()
        {
            var item1 = new OrderItemEntity { Name = "Modern Bamboo Chair", Quantity = 1 };
            var item2 = new OrderItemEntity { Name = "Collegiate Cardigan", Quantity = 3 };
            var item3 = new OrderItemEntity { Name = "Collegiate Cardigan", Quantity = 2 };

            item2.OrderItemAttributes.Add(new OrderItemAttributeEntity { Name = "Size", Description = "Small" });
            item3.OrderItemAttributes.Add(new OrderItemAttributeEntity { Name = "Size", Description = "Large" });

            SelectedOrderNumber = "72278";
            OrderNumberPostfix = "-1";

            Items = new[] { item1, item2, item3 }.Select(x => new OrderSplitItemViewModel(x)).ToList();
            Charges = new[]
            {
                new OrderChargeEntity { Type = "Sales Tax", Amount = 9.32M },
                new OrderChargeEntity { Type = "VAT", Amount = 3.88M },
                new OrderChargeEntity { Type = "Convenience Fee", Amount = 12.88M }
            }.Select(x => new OrderSplitChargeViewModel(x));
        }

        /// <summary>
        /// Selected order number to use for the new order
        /// </summary>
        public string SelectedOrderNumber { get; set; }

        /// <summary>
        /// Order number postfix to use for the new order
        /// </summary>
        public string OrderNumberPostfix { get; set; }

        /// <summary>
        /// Order items
        /// </summary>
        public IEnumerable<OrderSplitItemViewModel> Items { get; set; }

        /// <summary>
        /// Order charges
        /// </summary>
        public IEnumerable<OrderSplitChargeViewModel> Charges { get; set; }
    }
}
