using System;
using System.ComponentModel;
using System.Globalization;
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
    public class OrderSplitChargeViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private decimal originalAmount;
        private decimal splitAmount;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitChargeViewModel(IOrderChargeEntity item)
        {
            MethodConditions.EnsureArgumentIsNotNull(item, nameof(item));

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            OrderChargeID = item.OrderChargeID;
            Type = item.Type;
            Description = item.Description;
            TotalAmount = item.Amount;
            OriginalAmount = item.Amount;
            SplitAmount = "0";

            Increment = new RelayCommand(() => IncrementAction(), () => splitAmount < TotalAmount);
            Decrement = new RelayCommand(() => DecrementAction(), () => splitAmount > 0);
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Increment the value of split charges by one
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Increment { get; }

        /// <summary>
        /// Decrement the value of split charges by one
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Decrement { get; }

        /// <summary>
        /// Stream of property changes
        /// </summary>
        public IObservable<string> PropertyChangedStream => handler;

        /// <summary>
        /// ID of the order charge
        /// </summary>
        public long OrderChargeID { get; set; }

        /// <summary>
        /// Type of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Type { get; set; }

        /// <summary>
        /// Description of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Description { get; set; }

        /// <summary>
        /// Total amount of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Amount of the item on the original order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal OriginalAmount
        {
            get => originalAmount;
            set => handler.Set(nameof(OriginalAmount), ref originalAmount, value);
        }

        /// <summary>
        /// Amount of the item on the split order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SplitAmount
        {
            get => splitAmount.ToString("$0.00");
            set
            {
                if (decimal.TryParse(value, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out decimal parsedValue))
                {
                    UpdateSplitAmount(parsedValue);
                }
                else
                {
                    SplitAmount = splitAmount.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the decimal split amount
        /// </summary>
        public decimal SplitAmountValue => splitAmount;

        /// <summary>
        /// Decrement the value of split charges by one
        /// </summary>
        private void DecrementAction() =>
            UpdateSplitAmount(splitAmount - 1);

        /// <summary>
        /// Increment the value of split charges by one
        /// </summary>
        private void IncrementAction() =>
            UpdateSplitAmount(splitAmount + 1);

        /// <summary>
        /// Update the split amount value
        /// </summary>
        private void UpdateSplitAmount(decimal value)
        {
            var clampedValue = value.Clamp(0, TotalAmount);

            if (handler.Set(nameof(SplitAmount), ref splitAmount, clampedValue))
            {
                OriginalAmount = TotalAmount - splitAmount;
            }
        }
    }
}