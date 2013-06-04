using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using System.Xml.XPath;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Generic downloader customized for Magento
    /// </summary>
    class MagentoDownloader : GenericModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Create an order identifier based on xml coming from the store for an order.  Magento
        /// can have OrderNumberPostfixes
        /// </summary>
        protected override OrderIdentifier CreateOrderIdentifier(XPathNavigator orderXPath)
        {
            // pull out the order number
            long orderNumber = XPathUtility.Evaluate(orderXPath, "OrderNumber", 0L);
            string orderPostfix = XPathUtility.Evaluate(orderXPath, "Debug/OrderNumberPostfix", "");
            if (orderPostfix.Length > 0)
            {
                orderPostfix = "-" + orderPostfix;
            }
            return new MagentoOrderIdentifier(orderNumber, orderPostfix);
        }

        /// <summary>
        /// Process custom xml elements from the magento module, just before saving.
        /// </summary>
        public override void OnOrderLoadComplete(OrderEntity order, System.Xml.XPath.XPathNavigator xpath)
        {
            MagentoOrderEntity magentoOrder = order as MagentoOrderEntity;
            if (magentoOrder != null)
            {
                magentoOrder.MagentoOrderID = XPathUtility.Evaluate(xpath, "Debug/OrderID", order.OrderNumber);
            }
        }
    }
}
