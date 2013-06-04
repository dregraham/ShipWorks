using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// A candidate order to be included ina  to
    /// </summary>
    public class CombinedOrderComponent
    {
        /// <summary>
        /// Underlying order entity
        /// </summary>
        EbayOrderEntity order;
        public EbayOrderEntity Order
        {
            get { return order; }
        }

        /// <summary>
        /// Whether or not to include this component in the final combined payment
        /// </summary>
        public bool Included { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CombinedOrderComponent(EbayOrderEntity order, bool included)
        {
            this.order = order;
            Included = included;
        }
    }
}
