using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An abstract class that encapsulates the functionality for creating and submitting
    /// requests to eBay.
    /// </summary>
    public abstract class EbayRequest<TResult, TEbayRequest, TEbayResponse> where TEbayRequest : AbstractRequestType, new() where TEbayResponse : AbstractResponseType
    {
        EbayRequestConfiguration configuration;
        EbayToken token;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayRequest"/> class.
        /// </summary>
        protected EbayRequest(EbayToken token, string callName)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(token.Token))
            {
                throw new ArgumentException("token cannot be blank", "token");
            }

            this.configuration = new EbayRequestConfiguration(callName);
            this.token = token;
        }

        /// <summary>
        /// Execute the request and return the response
        /// </summary>
        public abstract TResult Execute();

        /// <summary>
        /// Gets the type of the eBay request.
        /// </summary>
        protected virtual AbstractRequestType CreateRequest()
        {
            return new TEbayRequest();
        }

        /// <summary>
        /// Submits the request to eBay.
        /// </summary>
        protected TEbayResponse SubmitRequest()
        {
            // There are cases where a request could fail unexpectedly; An ebay forum post said this
            // is "expected behavior" and should be retried when encountered.
            AbstractResponseType response = SubmitRequest(3);
            ValidateResponse(response);

            TEbayResponse cast = response as TEbayResponse;
            if (cast == null)
            {
                throw new EbayException("eBay responded with a response type that ShipWorks could not understand.");
            }

            return cast;
        }

        /// <summary>
        /// Submits the request to eBay and conditionally retries to submit the request if an
        /// InvalidOperationException or WebException is encountered based on the number of
        /// retry attempts remaining.
        /// </summary>
        /// <param name="retryAttemptsRemaining">The retry attempts remaining.</param>
        /// <returns>An AbstractResponseType object.</returns>
        private AbstractResponseType SubmitRequest(int retryAttemptsRemaining)
        {
            AbstractResponseType response = null;
            using (CustomEBaySoapService service = CreateEbaySoapService())
            {
                // There are cases where a request could fail unexpectedly; An ebay forum post said this
                // is "expected behavior" and should be retried when encountered. WebException or
                // InvalidOperationException could possibly warrant retrying to submit the request based
                // on the number of attempts remaining
                try
                {
                    // Just make sure the version of the request is set  prior to executing the request
                    // otherwise a SoapException will be generated
                    AbstractRequestType request = CreateRequest();
                    request.Version = configuration.ApiVersion;
                    request.MessageID = Guid.NewGuid().ToString();

                    response = service.ExecuteRequest(request, configuration.RequestName);
                }
                catch (WebException ex)
                {
                    // Establishing SSL issue.
                    bool isConnectionError = ex.Message.IndexOf("SSL") != -1 || ex.Message.IndexOf("underlying connection") != -1;

                    if (isConnectionError)
                    {
                        if (retryAttemptsRemaining > 0)
                        {
                            // Try submitting the response again
                            response = SubmitRequest(--retryAttemptsRemaining);
                        }
                        else
                        {
                            throw new EbayException("The eBay server reported the following error: " + ex.Message, ex);
                        }
                    }
                    else
                    {
                        throw new EbayException("Unable to communicate with the eBay server: " + ex.Message, ex);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // Internal 500 error issue
                    if (retryAttemptsRemaining > 0)
                    {
                        // Try resubmitting the response again
                        response = SubmitRequest(--retryAttemptsRemaining);
                    }
                    else
                    {
                        throw new EbayException("The eBay server reported the following error: " + ex.Message, ex);
                    }
                }
                catch (SoapException ex)
                {
                    if (retryAttemptsRemaining > 0)
                    {
                        // We'll get random eBay server errors such as "The data returned from eBay may be incomplete due to an eBay system error.",
                        // XML parse exceptions, and other internal errors; they don't really provide a reason why this may occur; the mentality
                        // is just, "Yep, that'll happen." According to eBay docs, we should retry the request for most of these types of errors.
                        response = SubmitRequest(--retryAttemptsRemaining);
                    }
                    else
                    {
                        string message = ex.Message;

                        if (ex.Detail != null)
                        {
                            XmlNode detailMessage = ex.Detail.SelectSingleNode("//DetailedMessage");
                            if (detailMessage != null)
                            {
                                message = detailMessage.InnerText;
                            }
                        }

                        throw new EbayException("eBay returned the following error:\n\n" + message, ex);
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(EbayException));
                }

                return response;
            }
        }

        /// <summary>
        /// Validates the response by checking to see if any errors occurred.
        /// </summary>
        /// <param name="response">The response.</param>
        private void ValidateResponse(AbstractResponseType response)
        {
            if (response.Errors != null && response.Errors.Length > 0)
            {
                // Errors occurred, but eBay treats warnings as errors; we're only interested
                // in those items with a severity level of Error
                List<ErrorType> ebayErrors = response.Errors.Where(e => e.SeverityCode == SeverityCodeType.Error).ToList();

                string errorMessage = string.Empty;
                foreach (ErrorType error in ebayErrors)
                {
                    if (error.ErrorCode == "10007")
                    {
                        // Use a more descriptive message for errors that occurred on the eBay servers
                        errorMessage += "An internal error occurred on the eBay server.\n";
                    }
                    else
                    {
                        errorMessage += error.LongMessage + "\n";
                    }
                }

                if (ebayErrors.Count > 0)
                {
                    // Throw an exception to indicate we encountered actual errors in the response
                    throw new EbayException(errorMessage, response.Errors[0].ErrorCode);
                }
            }
        }

        /// <summary>
        /// Creates a custom eBay soap service.
        /// </summary>
        /// <returns>A CustomEBaySoapService object.</returns>
        private CustomEBaySoapService CreateEbaySoapService()
        {
            // Create the service
            CustomEBaySoapService service = new CustomEBaySoapService(new ApiLogEntry(ApiLogSource.eBay, configuration.RequestName));
            service.Timeout = (int) configuration.Timeout.TotalMilliseconds;

            // Set credentials
            service.RequesterCredentials = new CustomSecurityHeaderType();
            service.RequesterCredentials.eBayAuthToken = token.Token;

            // Credentials
            if (EbayUrlUtilities.UseLiveServer)
            {
                service.RequesterCredentials.Credentials = new UserIdPasswordType();

                // I was getting "Certificate Mistmatch" errors.  I initiated a eBay Live Chat on the eBay
                // site and Bruce Thomson on 05/27/05 recomended leaving these properties out, as they are actually
                // also known by eBay through the auth token.  Seems to fix the problem.
                //  service.RequesterCredentials.Credentials.AppId = SecureText.Decrypt(liveApplication, "apptive");
                //  service.RequesterCredentials.Credentials.DevId = SecureText.Decrypt(liveDeveloper,   "apptive");
                //  service.RequesterCredentials.Credentials.AuthCert = SecureText.Decrypt(liveCertificate, "apptive");
            }
            else
            {
                service.RequesterCredentials.Credentials = new UserIdPasswordType();
                service.RequesterCredentials.Credentials.AppId = EbayUtility.SandboxApplicationCredential;
                service.RequesterCredentials.Credentials.DevId = EbayUtility.SandboxDeveloperCredential;
                service.RequesterCredentials.Credentials.AuthCert = EbayUtility.SandboxCertificateCredential;
            }

            string requestURL = EbayUrlUtilities.SoapUrl
                + "?callname=" + configuration.RequestName
                + "&siteid=0"
                + "&appid=" + service.RequesterCredentials.Credentials.AppId
                + "&version=" + configuration.ApiVersion
                + "&routing=default";

            service.Url = requestURL;

            return service;
        }


        #region CustomEBaySoapService

        /// <summary>
        /// Class that enables us to execute an ebay SOAP call by name.
        /// </summary>
        class CustomEBaySoapService : eBayAPIInterfaceService
        {
            /// <summary>
            /// Constructor, specifying a log entry
            /// </summary>
            public CustomEBaySoapService(ApiLogEntry logEntry)
                : base(logEntry)
            { }

            /// <summary>
            /// Execute the given request and return the response.
            /// </summary>
            public AbstractResponseType ExecuteRequest(AbstractRequestType request, string callname)
            {
                object[] results = this.Invoke(callname, new object[] { request });
                return ((AbstractResponseType) (results[0]));
            }
        }

        #endregion CustomEBaySoapService
    }
}
