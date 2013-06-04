using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static Uri CreateReceiptOnSandbox
        {
            get
            {
                return new Uri(etsyURL + "receipts");
            }
        }

        /// <summary>
        /// Given the shopID return the URL to FindAllShopReceipts
        /// </summary>
        public static Uri GetFindAllShopReceiptsURL(long shopID)
        {
            return new Uri(string.Format("{0}shops/{1}/receipts", etsyURL, shopID));
        }

        /// <summary>
        /// Given a comma seperated list of id's return the URL to MarkAsShipped
        /// </summary>
        public static Uri GetMarkAsShippedURL(string receipts)
        {
            return new Uri(string.Format("{0}private/receipts/{1}", etsyURL, receipts));
        }

        /// <summary>
        /// Given a comma seperated list of id's return the URL to GetReceipt
        /// </summary>
        public static Uri GetReceiptURL(string receipts)
        {
            return new Uri(string.Format("{0}receipts/{1}", etsyURL, receipts));
        }

        /// <summary>
        /// Given a transactionID, return a link to the line item.
        /// </summary>
        public static Uri GetItemUrl(string transactionID)
        {
            return new Uri(string.Format("https://www.etsy.com/transaction/{0}", transactionID));
        }
    }
}