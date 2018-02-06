using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Fraud risk downloader for Shopify
    /// </summary>
    [Component]
    public class ShopifyFraudDownloader : IShopifyFraudDownloader
    {
        private readonly TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        /// <summary>
        /// Download fraud risks from Shopify
        /// </summary>
        /// <param name="webClient">Shopify web client that will be used to download risks</param>
        /// <param name="shopifyOrderID">ID of the Shopify order for which to download risks</param>
        /// <returns>All fraud risks for the given order</returns>
        public async Task<IEnumerable<OrderPaymentDetailEntity>> Download(IShopifyWebClient webClient, long shopifyOrderID)
        {
            IEnumerable<JToken> risks = await Task.Run(() => webClient.GetFraudRisks(shopifyOrderID)).ConfigureAwait(false);

            var orderPaymentDetails = risks?
                .Where(r => r["message"] != null || r["recommendation"] != null)
                .Select(r =>
                    new OrderPaymentDetailEntity
                    {
                        Label = GetJsonValue(r["recommendation"]?.Value<string>()),
                        Value = r["message"]?.Value<string>()?.Truncate(100) ?? string.Empty
                    });

            return orderPaymentDetails ?? Enumerable.Empty<OrderPaymentDetailEntity>();
        }

        /// <summary>
        /// Return a truncated, title cased value of the given string.
        /// </summary>
        private string GetJsonValue(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            value = textInfo.ToTitleCase(value);
            return value.Truncate(100);
        }

        /// <summary>
        /// Merge fraud risks into the given order
        /// </summary>
        /// <param name="order">Order into which the fraud risks should be merged</param>
        /// <param name="fraudRisks">Fraud risks to merge</param>
        public void Merge(ShopifyOrderEntity order, IEnumerable<OrderPaymentDetailEntity> fraudRisks)
        {
            IEnumerable<OrderPaymentDetailEntity> paymentDetailsToCreate;
            if (order.OrderPaymentDetails.None())
            {
                paymentDetailsToCreate = fraudRisks;
            }
            else
            {
                paymentDetailsToCreate = fraudRisks
                    .Except(order.OrderPaymentDetails, (opd1, opd2) => opd1.Label == opd2.Label &&
                                                                       opd1.Value == opd2.Value);
            }

            order.OrderPaymentDetails.AddRange(paymentDetailsToCreate);
        }
    }
}
