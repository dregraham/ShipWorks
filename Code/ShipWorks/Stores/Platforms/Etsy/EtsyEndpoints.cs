using System;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Function to store EtsyEndpoint settings.
    /// </summary>
    public static class EtsyEndpoints
    {
        private const string etsyURL = "https://openapi.etsy.com/v2/";

        public const string LogOnURLQueryParameter = "login_url=";

        public const string DefaultScope = "email_r transactions_r transactions_w";
        public const string OrderIncludes = "Transactions/MainImage(url_75x75,url_570xN),Country(iso_country_code),Coupon";
        public const string TransactionIncludes = "MainImage(url_75x75,url_570xN)";

        public const string EncryptedConsumerKey = "5AVztl792QcPgu30EuKXgtLQ5ibg+IPXQYGcdauzAIc=";
        public const string EncryptedConsumerSecretKey = "wwdcLcTS/Le60r6W91KnNA==";

        public const int GetOrderLimit = 100;

        public static Uri RequestToken
        {
            get
            {
                return new Uri(etsyURL + "oauth/request_token");
            }
        }

        public static Uri AccessToken
        {
            get
            {
                return new Uri(etsyURL + "oauth/access_token");
            }
        }

        public static Uri GetUser
        {
            get
            {
                return new Uri(etsyURL + "users/__SELF__");
            }
        }

        public static Uri GetShops
        {
            get
            {
                return new Uri(etsyURL + "users/__SELF__/shops");
            }
        }

        /// <summary>
        /// Given the shopID return the URL to FindAllShopReceipts
        /// </summary>
        public static Uri GetFindAllShopReceiptsUrl(long shopID)
        {
            return new Uri($"{etsyURL}shops/{shopID}/receipts");
        }

        /// <summary>
        /// Given the receipt ID, get a list of transactions associated with it
        /// </summary>
        public static Uri GetTransactionsForReceipt(long receiptID)
        {
            return new Uri($"{etsyURL}receipts/{receiptID}/transactions");
        }

        /// <summary>
        /// Given a comma seperated list of id's return the URL to MarkAsShipped
        /// </summary>
        public static Uri GetMarkAsShippedUrl(string receipts)
        {
            return new Uri($"{etsyURL}private/receipts/{receipts}");
        }

        /// <summary>
        /// Given a comma seperated list of id's return the URL to GetReceipt
        /// </summary>
        public static Uri GetReceiptUrl(string receipts)
        {
            return new Uri($"{etsyURL}receipts/{receipts}");
        }

        /// <summary>
        /// Given a transactionID, return a link to the line item.
        /// </summary>
        public static Uri GetItemUrl(string transactionID)
        {
            return new Uri("https://www.etsy.com/transaction/" + transactionID);
        }

        /// <summary>
        /// Gets the submit tracking URL.
        /// </summary>
        public static Uri GetSubmitTrackingUrl(long etsyShopID, long orderID)
        {
            return new Uri($"{etsyURL}shops/{etsyShopID}/receipts/{orderID}/tracking");
        }

        /// <summary>
        /// Given a listing and product id, return the Product URL 
        /// </summary>
        public static Uri GetProductUrl(string listingID, string productID)
        {
            //return new Uri($"{etsyURL}listings/{listingID}/products/{productID}");

            return new Uri("https://openapi.etsy.com/v2/oauth/scopes");
        }
    }
}