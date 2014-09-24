using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Abstract Base of an InsureShipRequest
    /// </summary>
    public abstract class InsureShipRequestBase : ApiLogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        protected InsureShipRequestBase(ShipmentEntity shipment, InsureShipAffiliate affiliate, string logFileName)
            : this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipRequestBase)), logFileName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        protected InsureShipRequestBase(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log, string logFileName)
            : base(ApiLogSource.InsureShip, logFileName)
        {
            ResponseFactory = responseFactory;
            Shipment = shipment;
            Affiliate = affiliate;
            Log = log;
            Settings = insureShipSettings;
        }

        /// <summary>
        /// Gets or sets the response factory.
        /// </summary>
        protected IInsureShipResponseFactory ResponseFactory { get; private set; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        protected ILog Log { get; private set; }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        public ShipmentEntity Shipment { get; private set; }

        /// <summary>
        /// Gets or sets the affiliate.
        /// </summary>
        public InsureShipAffiliate Affiliate { get; private set; }

        /// <summary>
        /// Gets the request submitter.
        /// </summary>
        protected HttpVariableRequestSubmitter RequestSubmitter { get; private set; }

        /// <summary>
        /// Gets or sets the raw response.
        /// </summary>
        public virtual string ResponseContent { get; private set; }  // This is virtual because Moq overrides this in a test

        /// <summary>
        /// Gets the response status code.
        /// </summary>
        public virtual HttpStatusCode ResponseStatusCode { get; private set; }  // This is virtual because Moq overrides this in a test

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        protected IInsureShipSettings Settings { get; private set; }
        
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public abstract IInsureShipResponse Submit();

        /// <summary>
        /// Virtual method that hides the static OrderUtility.PopulateOrderDetails so 
        /// that we can unit test the request.
        /// </summary>
        public virtual void PopulateShipmentOrder()
        {
            OrderUtility.PopulateOrderDetails(Shipment);
        }

        /// <summary>
        /// A helper method that builds up the request details using the URI and data provided.
        /// </summary>
        /// <param name="postUri">The post URI.</param>
        /// <param name="postData">The post data.</param>
        protected void SubmitPost(Uri postUri, Dictionary<string, string> postData)
        {
            // Confirm that the connection with InsureShip has not been compromised
            EnsureSecureConnection();
            
            // Create/configure a new HttpRequestSubmitter
            ConfigureNewRequestSubmitter(postUri);
            foreach (string key in postData.Keys)
            {
                RequestSubmitter.Variables.Add(key, postData[key]);
            }

            try
            {
                // Log the request before submitting it to InsureShip
                //LogRequest(Encoding.Default.GetString(RequestSubmitter.GetPostContent()), "log");
                LogRequest(RequestSubmitter);
                HttpWebResponse httpWebResponse = RequestSubmitter.GetResponse().HttpWebResponse;

                // Grab the response data and save it to the local instance properties and 
                // log the response
                ReadResponse(httpWebResponse);
                LogInsureShipResponse(httpWebResponse);
            }
            catch (WebException ex)
            {
                Log.Error(ex);

                HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                ReadResponse(httpWebResponse);

                LogInsureShipResponse(httpWebResponse);
            }
        }

        /// <summary>
        /// Configures a new request submitter for the given URI, setting up the common headers and
        /// the allowable HTTP status codes.
        /// </summary>
        /// <param name="uri">The URI.</param>
        protected void ConfigureNewRequestSubmitter(Uri uri)
        {
            RequestSubmitter = new HttpVariableRequestSubmitter { Uri = uri };

            AddHeaders();
            AddAllowedHttpStatusCodes();
        }

        /// <summary>
        /// Ensures that a secure connection is made with InsureShip.
        /// </summary>
        /// <exception cref="InsureShipException"></exception>
        protected virtual void EnsureSecureConnection()
        {
            if (!IsTrustedCertificate())
            {
                const string message = "A trusted connection to InsureShip could not be established.";

                Log.Error(message);
                throw new InsureShipException(message);
            }
        }

        /// <summary>
        /// Determines whether the connection to InsureShip is secure by inspecting the certificate.
        /// </summary>
        /// <returns></returns>
        private bool IsTrustedCertificate()
        {
            ICertificateInspector certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.InsureShipCertificateVerificationData);
            CertificateRequest request = new CertificateRequest(Settings.CertificateUrl, certificateInspector);

            return request.Submit() == CertificateSecurityLevel.Trusted;
        }

        /// <summary>
        /// Adds the common headers to the request including authentication info and content type.
        /// </summary>
        protected void AddHeaders()
        {
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Settings.Username, Settings.Password)));
            string auth = string.Format("Basic {0}", credentials);

            RequestSubmitter.ContentType = "application/x-www-form-urlencoded";
            RequestSubmitter.Headers.Add("Accept", "application/json");
            RequestSubmitter.Headers.Add("Authorization", auth);
        }

        /// <summary>
        /// Adds the allowed HTTP status codes to the request based on the response
        /// codes we are expecting from the InsureShip API.
        /// </summary>
        protected void AddAllowedHttpStatusCodes()
        {
            List<HttpStatusCode> allowedCodes = new List<HttpStatusCode>();
            foreach (Enum value in Enum.GetValues(typeof (InsureShipResponseCode)))
            {
                int httpStatusCodeValue = int.Parse(EnumHelper.GetApiValue(value));
                allowedCodes.Add((HttpStatusCode) httpStatusCodeValue);
            }

            RequestSubmitter.AllowHttpStatusCodes(allowedCodes.ToArray());
        }

        /// <summary>
        /// Gets the content of the response.
        /// </summary>
        protected void ReadResponse(HttpWebResponse response)
        {
            ResponseStatusCode = (response == null) ? 0 : response.StatusCode;

            if (response != null)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            ResponseContent = reader.ReadToEnd();
                            reader.Close();
                        }

                        responseStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Logs the response from InsureShip.
        /// </summary>
        /// <param name="response">The response.</param>
        protected void LogInsureShipResponse(HttpWebResponse response)
        {
            if (response != null)
            {
                StringBuilder responseText = new StringBuilder();

                responseText.AppendLine(string.Format("{0} {1}", (int) response.StatusCode, response.StatusCode.ToString()));
                responseText.AppendLine(response.Headers.ToString());
                responseText.AppendLine(ResponseContent);
                
                LogResponse(responseText.ToString(), "log");
            }
        }
    }
}
