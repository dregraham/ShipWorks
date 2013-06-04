using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Configures order prototypes to locate orders
    /// </summary>
    public class ClickCartProOrderIdentifier : OrderIdentifier
    {
        string clickCartProOrderId = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProOrderIdentifier(string clickCartProOrderId) 
        {
            this.clickCartProOrderId = clickCartProOrderId;
        }

        /// <summary>
        /// Apply the prototype to the DownloadDetail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = clickCartProOrderId;
        }

        /// <summary>
        /// Apply the Click Cart Pro order id to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            ClickCartProOrderEntity ccpOrder = order as ClickCartProOrderEntity;
            if (ccpOrder == null)
            {
                throw new InvalidOperationException("A non Click Cart Pro order was passed to the ClickCartProOrderIdentifier");
            }

            ccpOrder.ClickCartProOrderID = clickCartProOrderId;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("ClickCartProOrderID:{0}", clickCartProOrderId);
        }
    }
}
