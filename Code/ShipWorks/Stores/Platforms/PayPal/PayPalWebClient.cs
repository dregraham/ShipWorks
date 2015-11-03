using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using System.Web.Services.Protocols;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.Net;
using log4net;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using System.Security.Cryptography;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Manages the PayPal API session and connection
    /// </summary>
    public class PayPalWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(PayPalWebClient));

        // The account this web client is operating for
        PayPalAccountAdapter paypalAccount;

        // collection of transaction types for which PayPal will throw an error if you try to get details on
        static List<PayPalTransactionExclusion> excludedTransactions = new List<PayPalTransactionExclusion>()
        {
            new PayPalTransactionExclusion("Failed Transfer", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Transfer", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Bill", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("PayPal Services", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("PayPal Services Credit Received", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Mass Payment Sent", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Reversal", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Fee Reversal", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion("Denied Payment", PayPalTransactionExclusionType.Match),
            new PayPalTransactionExclusion(".*subscription.*", PayPalTransactionExclusionType.Regex)
        };

        /// <summary>
        /// Constructor for specifying the paypal account credentials to use when connecting
        /// </summary>
        public PayPalWebClient(PayPalAccountAdapter ppAccount)
        {
            // do some validation
            if (ppAccount.ApiUserName.Length == 0)
            {
                if (ppAccount.CredentialType == PayPalCredentialType.Certificate)
                {
                    throw new PayPalException("A PayPal API Certificate must be imported for ShipWorks to connect to PayPal.");
                }
                else
                {
                    throw new PayPalException("Please enter your PayPal API username.");
                }
            }

            if (ppAccount.ApiPassword.Length == 0)
            {
                throw new PayPalException("Please enter your PayPal API password.");
            }

            if (ppAccount.ApiSignature.Length == 0 && ppAccount.CredentialType != PayPalCredentialType.Certificate)
            {
                throw new PayPalException("Please enter your PayPal API signature.");
            }

            this.paypalAccount = ppAccount;
        }


        #region CustomPayPalApiBinding

        /// <summary>
        /// Class that enabled us to execute an ebay SOAP call by name.
        /// </summary>
        class CustomPayPalApiBinding : PayPalAPISoapBinding
        {
            // Logger 
            static readonly ILog log = LogManager.GetLogger(typeof(CustomPayPalApiBinding));

            /// <summary>
            /// Constructor
            /// </summary>
			public CustomPayPalApiBinding(ApiLogEntry log) 
                : base(log)
			{

			}

            /// <summary>
            /// Execute the given request and return the response.
            /// </summary>
            public AbstractResponseType ExecuteRequest(AbstractRequestType request, string callname)
            {
                try
                {
                    // Get the call we are going to execute
                    MethodInfo method = GetType().GetMethod(callname);

                    // The first parameter is the container type
                    Type containerType = method.GetParameters()[0].ParameterType;

                    // We have to create the "Req" wrapper tha PayPal uses
                    object requestContainer = Activator.CreateInstance(containerType);

                    // The first field of the container holds our request
                    PropertyInfo[] properties = requestContainer.GetType().GetProperties(
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    properties[0].SetValue(requestContainer, request, null);

                    object[] results = this.Invoke(callname, new object[] { requestContainer });
                    return ((AbstractResponseType)(results[0]));
                }
                catch (Exception ex)
                {
                    // log the exception
                    log.Error(string.Format("Exception on call '{0}'", callname), ex);

                    throw;
                }
            }
        }

        #endregion

        /// <summary>
        /// Determines if we should connect to the 
        /// </summary>
        public static bool UseLiveServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("PayPalLiveServer", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("PayPalLiveServer", value);
            }
        }

        /// <summary>
        /// Current SOAP version we are supporting
        /// </summary>
        public static string ApiVersion
        {
            get { return "54.0"; }
        }

        /// <summary>
        /// Returns the eBay URL to submit requests to
        /// </summary>
        private static string GetApiUrl(bool signature)
        {
            if (UseLiveServer)
            {
				if (signature)
				{
					return "https://api-3t.paypal.com/2.0/";
				}
				else
				{
					return "https://api.paypal.com/2.0/";
				}
            }
            else
            {
                return "https://api.sandbox.paypal.com/2.0/";
            }
        }

        /// <summary>
        /// Execute the given PayPal SOAP request.
        /// </summary>
        public AbstractResponseType ExecuteRequest(AbstractRequestType request)
        {
            // Fill in the version
            if (request.Version == null)
            {
                request.Version = ApiVersion;
            }

            // Get the call name
            string callname = GetApiCallName(request);

            // Create the service to use
            using (CustomPayPalApiBinding service = CreateWebService(callname))
            {
                // Execute the call
                AbstractResponseType response;

                int retry = 2;

                while (true)
                {
                    try
                    {
                        response = service.ExecuteRequest(request, callname);

                        break;
                    }
                    catch (WebException ex)
                    {
                        // Any ebay forum post said this is "expected behavior" and should be retried when encountered.
                        if (ex.Message.IndexOf("SSL") != -1 || ex.Status == WebExceptionStatus.SecureChannelFailure)
                        {
                            if (retry > 0)
                            {
                                retry--;
                            }
                            else
                            {
                                throw new PayPalException(ex.Message, ex);
                            }
                        }
                        else
                        {
                            throw new PayPalException(ex.Message, ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(PayPalException));
                    }
                }

                // Errors occurred
                if (response.Errors != null && response.Errors.Length > 0)
                {
                    throw new PayPalException(response);
                }

                // return the paypal response
                return response;
            }
        }

        /// <summary>
        /// Create the web service logged using the specified callname
        /// </summary>
        /// <param name="callname"></param>
        /// <returns></returns>
        private CustomPayPalApiBinding CreateWebService(string callname)
        {
            // create the logging object
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.PayPal, callname);

            CustomPayPalApiBinding payPalApi = new CustomPayPalApiBinding(logEntry);

            UserIdPasswordType credentials = new UserIdPasswordType();
            credentials.Username = paypalAccount.ApiUserName;
            credentials.Password = paypalAccount.ApiPassword;
            credentials.Subject = "";

            // Security
            payPalApi.RequesterCredentials = new CustomSecurityHeaderType();
            payPalApi.RequesterCredentials.Credentials = credentials;

            // Server
            bool useCertificate = (paypalAccount.CredentialType == PayPalCredentialType.Certificate);
            payPalApi.Url = GetApiUrl(!useCertificate);

            // Use a certificate
            if (useCertificate)
            {
                if (paypalAccount.ApiCertificate == null)
                {
                    throw new PayPalException("An API certificate has not been imported.");
                }

                try
                {
                    // Get the certificate for the user
                    ClientCertificate certificate = new ClientCertificate();
                    certificate.Import(paypalAccount.ApiCertificate);

                    // add the certificate
                    payPalApi.ClientCertificates.Add(certificate.X509Certificate);
                }
                catch (CryptographicException ex)
                {
                    throw new PayPalException("There is a problem with your PayPal API Certificate:\n\n" + ex.Message, ex);
                }
            }
            // Or signature
            else
            {
                credentials.Signature = paypalAccount.ApiSignature;
                payPalApi.ClientCertificates.Clear();
            }

            return payPalApi;
        }

        /// <summary>
        /// Get the name of the SOAP call that is used for the given request
        /// </summary>
        private static string GetApiCallName(AbstractRequestType request)
        {
            string callname = request.GetType().Name.Replace("RequestType", "");

            return callname;
        }

        /// <summary>
        /// Returns a transaction so we can get some details about the seller
        /// </summary>
        /// <returns></returns>
        public PaymentTransactionType GetMostRecentTransaction()
        {
            // find payment received transactions in the past week
            TransactionSearchRequestType searchRequest = new TransactionSearchRequestType();
            searchRequest.StartDate = DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0, 0));
            searchRequest.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
            searchRequest.TransactionClassSpecified = true;
            searchRequest.TransactionClass = PaymentTransactionClassCodeType.Received;

            // execute the search
            TransactionSearchResponseType searchResult = (TransactionSearchResponseType)ExecuteRequest(searchRequest);
            if (searchResult.PaymentTransactions != null && searchResult.PaymentTransactions.Length > 0)
            {
                string transactionId = searchResult.PaymentTransactions[0].TransactionID;

                // get the details for this transaction
                GetTransactionDetailsRequestType getRequest = new GetTransactionDetailsRequestType();
                getRequest.TransactionID = transactionId;
                getRequest.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };

                // execute the get request
                GetTransactionDetailsResponseType getResult = (GetTransactionDetailsResponseType)ExecuteRequest(getRequest);

                return getResult.PaymentTransactionDetails;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Validate that the given API username and password can connect to PayPal.
        /// </summary>
        public void ValidateCredentials()
        {
            // Do a fake search, just to see if we can connect.
            TransactionSearchRequestType search = new TransactionSearchRequestType();
            search.TransactionID = "0";

            try
            {
                ExecuteRequest(search);
            }
            catch (InvalidOperationException ex)
            {
                throw new PayPalException("Unable to authenticate with the credentials provided.", ex);
            }
        }

        /// <summary>
        /// Gets the time according to the paypal servers
        /// </summary>
        public DateTime GetPayPalTime()
        {
            try
            {
                GetBalanceRequestType getBalanceRequest = new GetBalanceRequestType();
                GetBalanceResponseType response = (GetBalanceResponseType)ExecuteRequest(getBalanceRequest);

                if (response.TimestampSpecified)
                {
                    return response.Timestamp;
                }
                else
                {
                    return DateTime.UtcNow;
                }
            }
            catch (PayPalException)
            {
                return DateTime.UtcNow;
            }
        }


        /// <summary>
        /// Return transaction headers for transactions that occur between the given dates. Filter 
        /// specifies whether or not to remove transaction types for which we can't get 
        /// details on. 
        /// </summary>
        [NDependIgnoreLongMethod]
        public List<PaymentTransactionSearchResultType> GetTransactions(DateTime rangeStart, DateTime rangeEnd, bool filter)
        {
            TransactionSearchRequestType searchRequest = new TransactionSearchRequestType();
            searchRequest.StartDate = rangeStart;
            searchRequest.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
            searchRequest.TransactionClassSpecified = true;
            searchRequest.TransactionClass = PaymentTransactionClassCodeType.All;

            // end date
            if (rangeEnd > DateTime.MinValue)
            {
                searchRequest.EndDate = rangeEnd;
                searchRequest.EndDateSpecified = true;
            }

            log.InfoFormat("Getting PayPal transactions for the date range '{0}' - '{1}'.", rangeStart, rangeEnd);

            // execute the search
            try
            {
                TransactionSearchResponseType searchResult = (TransactionSearchResponseType)ExecuteRequest(searchRequest);

                // collection to hold the results
                List<PaymentTransactionSearchResultType> transactions = new List<PaymentTransactionSearchResultType>();

                if (searchResult.PaymentTransactions != null)
                {
                    transactions.AddRange(searchResult.PaymentTransactions);

                    // sort by timestamp 
                    transactions.Sort((a, b) =>
                    {
                        return a.Timestamp.CompareTo(b.Timestamp);
                    });

                    if (filter)
                    {
                        // remove the transaction types that PayPal doesn't let us query against
                        transactions.RemoveAll(t =>
                        {
                            return excludedTransactions.Any(ex => ex.Matches(t.Type));
                        });
                    }
                }
                
                return transactions;
            }
            catch (PayPalException ex)
            {
                // check for "results truncated" error/warning.  PayPal only returns 100 results, so we have to retry
                // with smaller chunks.
                if (ex.Errors.Count(e => e.Code.Trim() == "11002") > 0)
                {
                    log.InfoFormat("PayPal reported too many results, recalculating download range chunk size.");

                    TimeSpan difference = rangeEnd - rangeStart;
                    if (difference.TotalMinutes < 1)
                    {
                        log.InfoFormat("PayPal reported too many results, with a span of 1 minute; quitting.");

                        // +100 transactions in a single minute... probably a safe limit
                        // just a safeguard so we don't go forever
                        throw;
                    }

                    // try a smaller batch (1/16th the time of the last range attempted)
                    TimeSpan chunkSize = new TimeSpan(difference.Ticks / 16);
                    DateTime chunkStart = rangeStart;

                    // collection for results
                    List<PaymentTransactionSearchResultType> transactions = new List<PaymentTransactionSearchResultType>();

                    // walk the total timespan in small parts
                    while (chunkStart.Add(chunkSize) < rangeEnd)
                    {
                        DateTime chunkEnd = chunkStart.Add(chunkSize);

                        // download the next chunk
                        transactions.AddRange(GetTransactions(chunkStart, chunkEnd, true));

                        // move to the next chunk
                        chunkStart = chunkEnd;
                    }

                    // now add in the final chunk
                    transactions.AddRange(GetTransactions(chunkStart, rangeEnd, true));

                    // sort by timestamp 
                    transactions.Sort((a, b) =>
                    {
                        return a.Timestamp.CompareTo(b.Timestamp);
                    });

                    return transactions;
                }
                else
                {
                    // some other error, re-raise it
                    throw;
                }
            }

        }

        /// <summary>
        /// Retrieves the specified transaction from PayPal.
        /// </summary>
        public PaymentTransactionType GetTransaction(string transactionID)
        {
            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = transactionID;
            request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };

            // execute the request
            GetTransactionDetailsResponseType response = (GetTransactionDetailsResponseType)ExecuteRequest(request);
            return response.PaymentTransactionDetails;
        }
    }
}
