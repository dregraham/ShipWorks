using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Fraud risk downloader for Shopify
    /// </summary>
    public interface IShopifyFraudDownloader
    {
        /// <summary>
        /// Download fraud risks from Shopify
        /// </summary>
        /// <param name="webClient">Shopify web client that will be used to download risks</param>
        /// <param name="shopifyOrderID">ID of the Shopify order for which to download risks</param>
        /// <returns>All fraud risks for the given order</returns>
        Task<IEnumerable<OrderPaymentDetailEntity>> Download(IShopifyWebClient webClient, long shopifyOrderID);

        /// <summary>
        /// Merge fraud risks into the given order
        /// </summary>
        /// <param name="order">Order into which the fraud risks should be merged</param>
        /// <param name="fraudRisks">Fraud risks to merge</param>
        void Merge(ShopifyOrderEntity order, IEnumerable<OrderPaymentDetailEntity> fraudRisks);
    }
}
