using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Click Cart Pro order downloader
    /// </summary>
    class ClickCartProDownloader : GenericModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProDownloader(StoreEntity store) 
            : base(store)
        {

        }

        /// <summary>
        /// Assigns an order number to the provided order
        /// </summary>
        protected override void AssignOrderNumber(OrderEntity order)
        {
            if (order.IsNew)
            {
                order.OrderNumber = GetNextOrderNumber();
            }
        }

        /// <summary>
        /// Create an order identifier based on incoming Order Xml
        /// </summary>
        protected override OrderIdentifier CreateOrderIdentifier(XPathNavigator orderXPath)
        {
            return new ClickCartProOrderIdentifier(XPathUtility.Evaluate(orderXPath, "Debug/OrderID", "0"));
        }
    }
}
