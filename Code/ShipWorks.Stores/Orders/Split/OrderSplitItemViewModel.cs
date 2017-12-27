using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View model for an item during an order split
    /// </summary>
    public class OrderSplitItemViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private readonly double totalQuantity;
        private double splitQuantity;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitItemViewModel(IOrderItemEntity item)
        {
            MethodConditions.EnsureArgumentIsNotNull(item, nameof(item));

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            OrderItemID = item.OrderItemID;
            Name = item.Name;
            totalQuantity = item.Quantity;
            OriginalQuantity = item.Quantity;
            SplitQuantity = 0;
            Attributes = item.OrderItemAttributes?.Select(x => x.Name).ToImmutableList() ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID of the order item
        /// </summary>
        public long OrderItemID { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name { get; set; }

        /// <summary>
        /// Quantity of the item on the original order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double OriginalQuantity { get; set; }

        /// <summary>
        /// Quantity of the item on the split order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double SplitQuantity
        {
            get => splitQuantity;
            set
            {
                var clampedValue = value.Clamp(0, totalQuantity);

                if (handler.Set(nameof(SplitQuantity), ref splitQuantity, clampedValue))
                {
                    OriginalQuantity = totalQuantity - splitQuantity;
                }
            }
        }

        /// <summary>
        /// List of attributes for the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<string> Attributes { get; }
    }
}