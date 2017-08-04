using System.Threading.Tasks;
using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Generic downloader customized for Magento
    /// </summary>
    [Component]
    public class MagentoDownloader : GenericModuleDownloader, IMagentoModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager, configurationData, sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Begin order download
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            MagentoStoreEntity magentoStore = (MagentoStoreEntity) Store;
            trackedDurationEvent.AddProperty("Magento", ((MagentoVersion) magentoStore.MagentoVersion).ToString());

            return base.Download(trackedDurationEvent);
        }

        /// <summary>
        /// Create an order identifier based on xml coming from the store for an order.  Magento
        /// can have OrderNumberPostfixes
        /// </summary>
        protected override OrderIdentifier CreateOrderIdentifier(XPathNavigator orderXPath)
        {
            // pull out the order number
            long orderNumber = XPathUtility.Evaluate(orderXPath, "OrderNumber", 0L);
            string orderPostfix = XPathUtility.Evaluate(orderXPath, "OrderNumberPostfix", "");

            // Look in the old location if can't find it in new location
            if (string.IsNullOrEmpty(orderPostfix))
            {
                orderPostfix = XPathUtility.Evaluate(orderXPath, "Debug/OrderNumberPostfix", "");
            }

            if (orderPostfix.Length > 0)
            {
                orderPostfix = "-" + orderPostfix;
            }

            string orderPrefix = XPathUtility.Evaluate(orderXPath, "OrderNumberPrefix", "");

            return new MagentoOrderIdentifier(orderNumber, orderPrefix, orderPostfix);
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
