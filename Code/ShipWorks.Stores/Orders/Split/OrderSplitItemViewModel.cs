using System;
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
            
            // These cannot be broken out into factory methods because the CanExecute WeakReference
            // will be false, and the buttons will no longer update correctly
            Decrement = new RelayCommand(
                () => UpdateSplitQuantity(x => ClampValue(x - 1)),
                () => ClampValue(splitQuantity - 1) != splitQuantity);

            Increment = new RelayCommand(
                () => UpdateSplitQuantity(x => ClampValue(x + 1)),
                () => ClampValue(splitQuantity + 1) != splitQuantity);

            OrderItemID = item.OrderItemID;
            Name = item.Name;
            TotalQuantity = (decimal) item.Quantity;
            OriginalQuantity = (decimal) item.Quantity;
            SplitQuantity = "0";
            Attributes = item.OrderItemAttributes?.Select(x => new KeyValuePair<string, string>(x.Name, x.Description)).ToImmutableList() ??
                ImmutableList.Create(new KeyValuePair<string, string>(string.Empty, " "));
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
            get => SplitQuantityValue.ToString("0.##");
            set
            {
                if (decimal.TryParse(value, out decimal parsedValue))
                {
                    SplitQuantityValue = parsedValue;
                }
                else
                {
                    SplitQuantity = SplitQuantityValue.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the decimal split amount
        /// </summary>
        public decimal SplitQuantityValue
        {
            get => splitQuantity;
            set => UpdateSplitQuantity(_ => ClampValue(value));
        }

        /// <summary>
        /// List of attributes for the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<string, string>> Attributes { get; }

        /// <summary>
        /// Clamp the given value to the total amount
        /// </summary>
        private decimal ClampValue(decimal value) =>
            value.Clamp(0, TotalQuantity);
        
        /// <summary>
        /// Update the split amount value
        /// </summary>
        private void UpdateSplitQuantity(Func<decimal, decimal> changeValue)
        {
            if (handler.Set(nameof(SplitQuantity), ref splitQuantity, changeValue(splitQuantity)))
            {
                OriginalQuantity = TotalQuantity - splitQuantity;
            }
        }
    }
}