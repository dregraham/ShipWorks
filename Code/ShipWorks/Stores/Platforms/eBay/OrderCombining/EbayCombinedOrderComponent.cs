using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// A candidate order to be included in a combined order
    /// </summary>
    public class EbayCombinedOrderComponent
    {
         EbayOrderEntity order;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCombinedOrderComponent(EbayOrderEntity order, bool included)
        {
            this.order = order;
            Included = included;
        }

        /// <summary>
        /// Underlying order entity
        /// </summary>
        public EbayOrderEntity Order
        {
            get { return order; }
        }

        /// <summary>
        /// Whether or not to include this component in the final combined payment
        /// </summary>
        public bool Included 
        { 
            get; 
            set; 
        }
    }
}
