using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Etsy Web Services Client
    /// </summary>
    public interface IEtsyWebClient
    {
        /// <summary>
        /// Given a token and verifier, authorize token and set OAuthToken and OAuthTokenSecret in store.
        /// </summary>
        void AuthorizeToken(string token, string verifier);

        /// <summary>
        /// Returns the DateTime at Etsy
        /// </summary>
        DateTime GetEtsyDateTime();

        /// <summary>
        /// Get the number of orders from Etsy between startDate and endDate
        /// </summary>
        int GetOrderCount(Range<DateTime> dateRange);

        /// <summary>
        /// Given a comma separated list of order numbers, return the order numbers where the Etsy status doesn't match the parameters.
        /// </summary>
        IEnumerable<long> GetOrderNumbersWithChangedStatus(string orderNumbers, string etsyFieldName, bool currentStatus);

        /// <summary>
        /// Gets orders created after startDate.
        /// </summary>
        List<JToken> GetOrders(Range<DateTime> dateRange, int limit, int offset);

        /// <summary>
        /// Get payment information from Etsy for the comma separated list of orders
        /// </summary>
        JArray GetPaymentInformation(string formattedOrderNumbers);

        /// <summary>
        /// Gets a list of products
        /// </summary>
        JToken GetProduct(int listingId, int productId);

        /// <summary>
        /// Gets the URL a user will use to authorize their etsy account to use ShipWorks.
        /// </summary>
        /// <returns>URL for Etsy Authorization</returns>
        Uri GetRequestTokenURL(Uri callbackURL);

        /// <summary>
        /// Gets store create date from Etsy
        /// </summary>
        DateTime GetStoreCreationDate();

        /// <summary>
        /// Gets a list of transactions associated with the given receipt
        /// </summary>
        JToken GetTransactionsForReceipt(long receiptID, int limit, int offset);

        /// <summary>
        /// Populates store information from Etsy
        /// </summary>
        void RetrieveShopInformation();

        /// <summary>
        /// Load shop information for the token associated with this store
        /// </summary>
        void RetrieveTokenShopDetails();

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(long etsyShopID, long orderNumber, string trackingNumber, string etsyCarrierName);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(long etsyShopID, long orderNumber, string trackingNumber, string etsyCarrierName, bool resetShippingStatusAndRetryOnFailure);

        /// <summary>
        /// Uploads payment, shipment and comments to Etsy.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="comment">Only upload comment if not empty</param>
        /// <param name="wasShipped">Only upload status has value</param>
        /// <param name="wasPaid">Only upload status has value</param>
        void UploadStatusDetails(long orderNumber, string comment, bool? wasPaid = default(bool?), bool? wasShipped = default(bool?));
    }
}