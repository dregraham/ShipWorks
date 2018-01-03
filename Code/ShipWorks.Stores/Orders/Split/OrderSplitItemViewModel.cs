using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
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
        private decimal originalQuantity;
        private decimal splitQuantity;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitItemViewModel(IOrderItemEntity item)
        {
            MethodConditions.EnsureArgumentIsNotNull(item, nameof(item));

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            OrderItemID = item.OrderItemID;
            Name = item.Name;
            TotalQuantity = (decimal) item.Quantity;
            OriginalQuantity = (decimal) item.Quantity;
            SplitQuantity = "0";
            Attributes = item.OrderItemAttributes?.Select(x => $"{x.Name}: {x.Description}").ToImmutableList() ??
                ImmutableList.Create(" ");

            Increment = new RelayCommand(() => IncrementAction(), () => splitQuantity < TotalQuantity);
            Decrement = new RelayCommand(() => DecrementAction(), () => splitQuantity > 0);
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Increment the value of split items by one
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Increment { get; }

        /// <summary>
        /// Decrement the value of split items by one
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Decrement { get; }

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
        /// Total quantity of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Quantity of the item on the original order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal OriginalQuantity
        {
            get => originalQuantity;
            set => handler.Set(nameof(OriginalQuantity), ref originalQuantity, value);
        }

        /// <summary>
        /// Quantity of the item on the split order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SplitQuantity
        {
            get => splitQuantity.ToString("0.##");
            set
            {
                if (decimal.TryParse(value, out decimal parsedValue))
                {
                    UpdateSplitQuantity(parsedValue);
                }
                else
                {
                    SplitQuantity = splitQuantity.ToString();
                }
            }
        }

        /// <summary>
        /// List of attributes for the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<string> Attributes { get; }

        /// <summary>
        /// Gets the decimal split quantity
        /// </summary>
        public decimal SplitQuantityValue => splitQuantity;

        /// <summary>
        /// Decrement the value of split items by one
        /// </summary>
        private void DecrementAction() =>
            UpdateSplitQuantity(splitQuantity - 1);

        /// <summary>
        /// Increment the value of split items by one
        /// </summary>
        private void IncrementAction() =>
            UpdateSplitQuantity(splitQuantity + 1);

        /// <summary>
        /// Update the split quantity value
        /// </summary>
        private void UpdateSplitQuantity(decimal value)
        {
            var clampedValue = value.Clamp(0, TotalQuantity);

            if (handler.Set(nameof(SplitQuantity), ref splitQuantity, clampedValue))
            {
                OriginalQuantity = TotalQuantity - splitQuantity;
            }
        }
    }
}