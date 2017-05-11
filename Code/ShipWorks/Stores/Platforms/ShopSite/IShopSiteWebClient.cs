using System.Xml;
using Interapptive.Shared.Threading;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite
    /// </summary>
    public interface IShopSiteWebClient
    {
        /// <summary>
        /// Determines if we can successfully connect to and login to ShopSite
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Get the next page of orders, starting with the order with the specified order number
        /// </summary>
        XmlDocument GetOrders(long startOrder);

        /// <summary>
        /// Progress reporter associated with the client
        /// </summary>
        /// <remarks>
        /// If this is null, the client cannot be canceled and progress will not be reported
        /// </remarks>
        IProgressReporter ProgressReporter { get; set; }
    }
}
