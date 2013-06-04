using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Class for identifying orders that are from Magento.  When an order is edited
    /// on the Magento website, the existing order is marked as cancelled and a new one 
    /// is created with a postfix (-1, -2, -3...).  This identifier handles these.
    /// </summary>
    public class MagentoOrderIdentifier : OrderNumberIdentifier
    {

        // the postfix on the order number
        string orderPostfix = "";

        public string PostFix
        {
            get { return orderPostfix;}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOrderIdentifier(long orderNumber, string postFix) : base (orderNumber)
        {
            this.orderPostfix = postFix;
        }
        
        /// <summary>
        /// Apply the order number and postfix value to the supplied order.
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            base.ApplyTo(order);

            if (orderPostfix.Length > 0)
            {
                order.ApplyOrderNumberPostfix(orderPostfix);
            }
        }

        /// <summary>
        /// Apply the order number and postfix to the supplied download detail entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            // using the base behavior
            base.ApplyTo(downloadDetail);

            // not applying the postfix because when magento orders get a postfix, it's from
            // the order being edited on the site.  It's really not a whole new order being downloaded.
        }
    }
}
