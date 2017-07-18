using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// Order downloader for Commerce Interface stores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceDownloader : GenericModuleDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager, configurationData, sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Process custom xml elements from the CommerceInterface module, just before saving
        /// </summary>
        /// <param name="order"></param>
        /// <param name="xpath"></param>
        public override void OnOrderLoadComplete(OrderEntity order, XPathNavigator xpath)
        {
            CommerceInterfaceOrderEntity ciOrder = order as CommerceInterfaceOrderEntity;
            if (ciOrder != null)
            {
                ciOrder.CommerceInterfaceOrderNumber = XPathUtility.Evaluate(xpath, "Debug/CIOrderNumber", order.OrderNumber.ToString());
            }
        }
    }
}
