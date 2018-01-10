using System.Collections.Generic;
using System.Linq;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders 
    /// </summary>
    public partial class GenericModuleOrderEntity : IAmazonOrder
    {
        /// <summary>
        /// The Amazon Order ID from Amazon
        /// </summary>
        bool IAmazonOrder.IsPrime => IsPrime == (int) AmazonIsPrime.Yes;
        
        /// <summary>
        /// List of IAmazonOrderItem representing the Amazon order items
        /// </summary>
        public IEnumerable<IAmazonOrderItem> AmazonOrderItems
        {
            get { return OrderItems.Select(s => s as IAmazonOrderItem); }
        }

        /// <summary>
        /// Should the order be treated as same day
        /// </summary>
        bool IAmazonOrder.IsSameDay() => IsSameDay;
    }
}