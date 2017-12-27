using System.ComponentModel;
using System.Reflection;
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
        private readonly decimal totalAmount;
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
            totalAmount = item.Amount;
            OriginalAmount = item.Amount;
            SplitAmount = 0;
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Amount of the item on the original order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// Amount of the item on the split order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal SplitAmount
        {
            get => splitAmount;
            set
            {
                var clampedValue = value.Clamp(0, totalAmount);

                if (handler.Set(nameof(SplitAmount), ref splitAmount, clampedValue))
                {
                    OriginalAmount = totalAmount - splitAmount;
                }
            }
        }
    }
}