using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Click Cart Pro order downloader
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.ClickCartPro)]
    public class ClickCartProDownloader : GenericModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager, configurationData, sqlAdapterFactory)
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
