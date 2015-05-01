using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Interapptive.Shared.Net;
using Interapptive.Shared.Net.OAuth;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Etsy.Enums;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Etsy Web Services Client
    /// </summary>
    public class EtsyWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyWebClient));

        // The store we are connecting to
        readonly EtsyStoreEntity store;
        string unvalidatedSecretToken = string.Empty;

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public EtsyWebClient(EtsyStoreEntity etsyStore)
        {
            if (etsyStore == null)
            {
                throw new ArgumentNullException("etsyStore");
            }

            store = etsyStore;
        }

        /// <summary>
        /// Get's the URL a user will use to authorize their etsy account to use ShipWorks.
        /// </summary>
        /// <returns>URL for Etsy Authorization</returns>
        public Uri GetRequestTokenURL(Uri callbackURL)
        {
            if (callbackURL == null)
            {
                throw new ArgumentNullException("callbackURL");
            }

            OAuth oAuth = new OAuth(EtsyEndpoints.EncryptedConsumerKey, EtsyEndpoints.EncryptedConsumerSecretKey)
            {
                Url = EtsyEndpoints.RequestToken
            };

            oAuth.OtherParameters.Add("scope", EtsyEndpoints.DefaultScope);
            oAuth.OtherParameters.Add("oauth_callback", callbackURL.ToString());

            string response = ProcessRequest(oAuth, "GetRequestTokenURL");

            if (response.IndexOf(EtsyEndpoints.LogOnURLQueryParameter, StringComparison.OrdinalIgnoreCase) > -1)
            {
                string redirectURL = string.Empty;

                //The response from etsy is like "redirectURL=http://.../.../..?oauth_token_secret=...&...
                //parse out redirectURL from response.
                redirectURL = HttpUtility.UrlDecode(response.Substring(EtsyEndpoints.LogOnURLQueryParameter.Length));

                //parse out token secret from redirectURL
                NameValueCollection queryStringCollection = HttpUtility.ParseQueryString(redirectURL);

                unvalidatedSecretToken = queryStringCollection["oauth_token_secret"];

                return new Uri(redirectURL);
            }
            else
            {
                throw new ArgumentException("Invalid URL Generated from Etsy. No URL given.");
            }
        }

        /// <summary>
        /// Given a token and verifier, authorize token and set OAuthToken and OAuthTokenSecret in store.
        /// </summary>
        public void AuthorizeToken(string token, string verifier)
        {
            OAuth oAuth = new OAuth(EtsyEndpoints.EncryptedConsumerKey, EtsyEndpoints.EncryptedConsumerSecretKey)
            {
                Url = EtsyEndpoints.AccessToken,
                Token = token,
                TokenSecret = unvalidatedSecretToken
            };

            oAuth.OtherParameters.Add("oauth_verifier", verifier);

            string response = ProcessRequest(oAuth, "AuthorizeToken", true);

            store.SaveFields("Checkpoint");

            try
            {
                NameValueCollection responseCollection = HttpUtility.ParseQueryString(response);
                store.OAuthToken = SecureText.Encrypt(responseCollection["oauth_token"], "token");
                store.OAuthTokenSecret = SecureText.Encrypt(responseCollection["oauth_token_secret"], "token");

                RetrieveTokenShopDetails();
            }
            catch
            {
                store.RollbackFields("Checkpoint");

                throw;
            }
        }

        /// <summary>
        /// Load shop information for the token associated with this store
        /// </summary>
        public void RetrieveTokenShopDetails()
        {
            JToken shop = GetEtsyShop();

            store.EtsyShopID = (long) shop["shop_id"];
            store.EtsyStoreName = (string) shop["shop_name"];
            store.EtsyLoginName = (string) shop["User"]["login_name"];
        }

        /// <summary>
        /// Populates store information from Etsy
        /// </summary>
        public void RetrieveShopInformation()
        {
            JToken shop = GetEtsyShop();

            store.EtsyShopID = (long) shop["shop_id"];
            store.StoreName = (string) shop["shop_name"];
            store.Email = (string) shop["User"]["primary_email"];
        }

        /// <summary>
        /// Get's store create date from Etsy
        /// </summary>
        public DateTime GetStoreCreationDate()
        {
            JToken shop = GetEtsyShop();

            double creationDateEpoch = shop.GetValue("creation_tsz", 0d);

            return DateTimeUtility.FromUnixTimestamp(creationDateEpoch).ToUniversalTime();
        }

        /// <summary>
        /// Query etsy for the 'Shop' object associated with the store
        /// </summary>
        private JToken GetEtsyShop()
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetShops);
            oAuth.OtherParameters.Add("includes", "User");

            string response = ProcessRequest(oAuth, "GetTokenShopDetails");

            JToken responseObject = JObject.Parse(response);
            JArray results = (JArray) responseObject["results"];

            if (results == null || results.Count == 0)
            {
                throw new EtsyException("The user you logged in with is not registered as a seller on Etsy.");
            }

            // For now we are hard-coded to the first shop
            JToken shop = results[0];

            return shop;
        }

        /// <summary>
        /// Get the number of orders from Etsy between startDate and endDate
        /// </summary>
        public int GetOrderCount(DateTime startDate, DateTime endDate)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetFindAllShopReceiptsUrl(store.EtsyShopID));

            oAuth.OtherParameters.Add("min_created", DateTimeUtility.ToUnixTimestamp(startDate).ToString());
            oAuth.OtherParameters.Add("max_created", DateTimeUtility.ToUnixTimestamp(endDate).ToString());
            oAuth.OtherParameters.Add("limit", "1");

            string response = ProcessRequest(oAuth, "GetOrderCount");

            JToken results = JObject.Parse(response);

            return (int) results["count"];
        }

        /// <summary>
        /// Get's orders created after startDate.
        /// </summary>
        public List<JToken> GetOrders(DateTime startDate, DateTime endDate, int limit, int offset)
        {
            double startDateStamp = DateTimeUtility.ToUnixTimestamp(startDate);
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetFindAllShopReceiptsUrl(store.EtsyShopID));

            oAuth.OtherParameters.Add("min_created", startDateStamp.ToString());
            oAuth.OtherParameters.Add("max_created", DateTimeUtility.ToUnixTimestamp(endDate).ToString());
            oAuth.OtherParameters.Add("includes", EtsyEndpoints.OrderIncludes);
            oAuth.OtherParameters.Add("limit", limit.ToString());
            oAuth.OtherParameters.Add("offset", offset.ToString());

            string response = ProcessRequest(oAuth, "GetOrders");

            JObject orderResponse = JObject.Parse(response);
            List<JToken> ordersToReturn = new List<JToken>();

            if (orderResponse != null)
            {
                JArray ordersList = orderResponse.SelectToken("results") as JArray;

                if (ordersList != null && ordersList.Count > 0)
                {
                    // Sort the orders by update date ascending
                    ordersToReturn = ordersList.OrderBy(o => (int) o["creation_tsz"]).ToList<JToken>();

                    // Remove orders on the startDate border, b\c we already have those
                    ordersToReturn = ordersToReturn.Where(o => (int) o["creation_tsz"] != startDateStamp || !IsOrderInDatabase((int) o["receipt_id"])).ToList();
                }
            }

            return ordersToReturn;
        }

        /// <summary>
        /// Gets a list of transactions associated with the given receipt
        /// </summary>
        public JToken GetTransactionsForReceipt(long receiptID, int limit, int offset)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetTransactionsForReceipt(receiptID));

            oAuth.OtherParameters.Add("includes", EtsyEndpoints.TransactionIncludes);
            oAuth.OtherParameters.Add("limit", limit.ToString());
            oAuth.OtherParameters.Add("offset", offset.ToString());

            string response = ProcessRequest(oAuth, "GetTransactions");
            return JObject.Parse(response);
        }

        /// <summary>
        /// Uploads payment, shipment and comments to Etsy.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="comment">Only upload comment if not empty</param>
        /// <param name="wasShipped">Only upload status has value</param>
        /// <param name="wasPaid">Only upload status has value</param>
        public void UploadStatusDetails(long orderNumber, string comment, bool? wasPaid = null, bool? wasShipped = null)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetMarkAsShippedUrl(orderNumber.ToString()));

            if (wasShipped.HasValue)
            {
                oAuth.OtherParameters.Add("was_shipped", wasShipped.Value.ToString());
            }

            if (wasPaid.HasValue)
            {
                oAuth.OtherParameters.Add("was_paid", wasPaid.Value.ToString());
            }

            if (!string.IsNullOrEmpty(comment))
            {
                oAuth.OtherParameters.Add("message_from_seller", comment);
            }

            oAuth.OtherParameters.Add("method", "PUT");

            ProcessRequest(oAuth, "UploadStatusDetails");
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(long etsyShopID, long orderNumber, string trackingNumber, string etsyCarrierName)
        {
            UploadShipmentDetails(etsyShopID, orderNumber, trackingNumber, etsyCarrierName, true);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(long etsyShopID, long orderNumber, string trackingNumber, string etsyCarrierName, bool resetShippingStatusAndRetryOnFailure)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetSubmitTrackingUrl(etsyShopID, orderNumber));

            oAuth.OtherParameters.Add("tracking_code", trackingNumber);
            oAuth.OtherParameters.Add("carrier_name", etsyCarrierName);

            oAuth.OtherParameters.Add("method", "POST");

            try
            {
                ProcessRequest(oAuth, "SubmitTracking");
            }
            catch (EtsyException webException)
            {
                string errorDetail;

                if (TryGetEmailAlreadySentMessage(webException, out errorDetail))
                {
                    // Updating tracking information can fail if the receipt is already marked as shipped.  If this happens,
                    // we'll try to reset the shipped status and try updating the tracking information again.
                    log.Warn(string.Format("Etsy order {0} already marked as shipped.", orderNumber), webException);

                    if (resetShippingStatusAndRetryOnFailure)
                    {
                        UploadStatusDetails(orderNumber, null, null, false);
                        UploadShipmentDetails(etsyShopID, orderNumber, trackingNumber, etsyCarrierName, false);
                    }
                    else
                    {
                        throw new EtsyException(String.Format("Error uploading tracking information: {0}", errorDetail));
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Try to get an email already sent message from the exception
        /// </summary>
        /// <remarks>This method is mainly meant to remove the logic that figures out what type of exception
        /// we're dealing with, if it's the right error code, etc. from what we actually want to do 
        /// with that information.</remarks>
        private static bool TryGetEmailAlreadySentMessage(Exception exception, out string errorDetail)
        {
            errorDetail = null;

            if (exception == null)
            {
                return false;
            }

            WebException webException = exception.InnerException as WebException;
            if (webException == null)
            {
                return false;
            }

            HttpWebResponse response = webException.Response as HttpWebResponse;
            if (response == null)
            {
                return false;
            }

            errorDetail = response.GetResponseHeader("X-Error-Detail");
            return response.StatusCode == HttpStatusCode.BadRequest && errorDetail.Contains("email has already been sent");
        }

        /// <summary>
        /// Given a comma seperated list of recieptsID's, return the orders shipped and paid status.
        /// </summary>
        private JArray GetOrderStatuses(string receipts)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetReceiptUrl(receipts));

            oAuth.OtherParameters.Add("fields", "receipt_id,was_shipped,was_paid");

            string response = ProcessRequest(oAuth, "GetOrderStatuses");

            JObject result = JObject.Parse(response);

            return (JArray) result["results"];
        }

        /// <summary>
        /// Returns an OAuth oject with url, token, and tokenSecret filled in.
        /// </summary>
        private OAuth GetNewOAuth(Uri url)
        {
            return new OAuth(EtsyEndpoints.EncryptedConsumerKey, EtsyEndpoints.EncryptedConsumerSecretKey)
                {
                    Url = url,
                    Token = SecureText.Decrypt(store.OAuthToken, "token"),
                    TokenSecret = SecureText.Decrypt(store.OAuthTokenSecret, "token")
                };
        }

        /// <summary>
        /// If order is already in the local DB return true.
        /// </summary>
        private bool IsOrderInDatabase(int orderNumber)
        {
            return 1 == OrderCollection.GetCount(SqlAdapter.Default,
                            OrderFields.StoreID == store.StoreID &
                            OrderFields.OrderNumber == orderNumber &
                            OrderFields.IsManual == false);
        }

        /// <summary>
        /// Returns the DateTime at Etsy
        /// </summary>
        public DateTime GetEtsyDateTime()
        {
            DateTime serverDate = new DateTime();

            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetUser);
            var response = GetResponseReader(oAuth, "GetDateTime");

            if (!DateTime.TryParse(response.HttpWebResponse.Headers["Date"], out serverDate))
            {
                throw new WebException("Response did not contain a Header Date");
            }

            return serverDate.ToUniversalTime();
        }

        /// <summary>
        /// Actual call to Etsy
        /// </summary>
        private static string ProcessRequest(OAuth oAuth, string action, bool encryptResponse = false)
        {
            IHttpResponseReader response = GetResponseReader(oAuth, action, encryptResponse);
            return response.ReadResult();
        }

        /// <summary>
        /// Get the HttpResponseReader from Etsy
        /// </summary>
        private static IHttpResponseReader GetResponseReader(OAuth oAuth, string action, bool encryptResponse = false)
        {
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Etsy, action);

            // Set if we want encryption
            logger.Encryption = encryptResponse ? ApiLogEncryption.Encrypted : ApiLogEncryption.Default;

            try
            {
                string url = oAuth.GenerateRequest();

                HttpVariableRequestSubmitter submiter = new HttpVariableRequestSubmitter();
                submiter.Verb = HttpVerb.Get;
                submiter.Uri = new Uri(url);

                logger.LogRequest(submiter);

                using (IHttpResponseReader response = submiter.GetResponse())
                {
                    string result = response.ReadResult();
                    logger.LogResponse(result, "txt");

                    // If Etsy goes down, they start returning web pages for the status
                    if (result.Contains("<html"))
                    {
                        throw new EtsyException("The Etsy site appears to be down or experiencing issues.");
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.LogResponse(ex);

                throw WebHelper.TranslateWebException(ex, typeof(EtsyException));
            }
        }

        /// <summary>
        /// Given a comma seperated list of order numbers, return the order numbers where the Etsy status doesn't match the parameters.
        /// </summary> 
        public IEnumerable<long> GetOrderNumbersWithChangedStatus(string orderNumbers, string etsyFieldName, bool currentStatus)
        {
            JArray receipts = GetOrderStatuses(orderNumbers);

            return from x in receipts
                       where (bool)x[etsyFieldName] != currentStatus
                       select (long)x["receipt_id"];
        }

        /// <summary>
        /// Get payment information from Etsy for the comma seperated list of orders
        /// </summary>
        public JArray GetPaymentInformation(string formattedOrderNumbers)
        {
            OAuth oAuth = GetNewOAuth(EtsyEndpoints.GetReceiptUrl(formattedOrderNumbers));

            oAuth.OtherParameters.Add("fields", "receipt_id,was_paid,payment_method,message_from_payment");

            string response = ProcessRequest(oAuth, "GetPaymentInformation");

            JObject result = JObject.Parse(response);

            return (JArray) result["results"];
        }
    }
}
