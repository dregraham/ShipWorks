using System;
using System.Collections.Generic;
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
        protected InsureShipRequestBase(ShipmentEntity shipment, InsureShipAffiliate affiliate)
            : this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipRequestBase)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        protected InsureShipRequestBase(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log)
            : base(ApiLogSource.InsureShip, "InsureShip")
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
        public virtual HttpWebResponse RawResponse { get; private set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        protected IInsureShipSettings Settings { get; private set; }
        
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public abstract IInsureShipResponse Submit();

        /// <summary>
        /// Uses the Order ID and the Shipment ID to create the unique shipment identifier .
        /// </summary>
        public virtual string GetUniqueShipmentId()
        {
            return string.Format("{0}-{1}", Shipment.OrderID, Shipment.ShipmentID);    
        }

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
            if (!IsTrustedCertificate())
            {
                string message = "A trusted connection to InsureShip could not be established.";

                Log.Error(message);
                throw new InsureShipException(message);
            }

            RequestSubmitter = new HttpVariableRequestSubmitter { Uri = postUri };

            foreach (string key in postData.Keys)
            {
                RequestSubmitter.Variables.Add(key, postData[key]);
            }

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Settings.Username, Settings.Password)));
            string auth = string.Format("Basic {0}", credentials);

            RequestSubmitter.ContentType = "application/x-www-form-urlencoded";
            RequestSubmitter.Headers.Add("Accept", "application/json");
            RequestSubmitter.Headers.Add("Authorization", auth);

            AddAllowedHttpStatusCodes();

            try
            {
                LogRequest(Encoding.Default.GetString(RequestSubmitter.GetPostContent()), "log");

                RawResponse = RequestSubmitter.GetResponse().HttpWebResponse;
                LogInsureShipResponse(RawResponse);
            }
            catch (WebException ex)
            {
                Log.Error(ex);
                LogInsureShipResponse(ex.Response as HttpWebResponse);

                RawResponse = ex.Response as HttpWebResponse;
            }
        }

        /// <summary>
        /// Adds the allowed HTTP status codes to the request based on the response
        /// codes we are expecting from the InsureShip API.
        /// </summary>
        private void AddAllowedHttpStatusCodes()
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
        /// Logs the response from InsureShip.
        /// </summary>
        /// <param name="response">The response.</param>
        private void LogInsureShipResponse(HttpWebResponse response)
        {
            if (response != null)
            {
                StringBuilder responseText = new StringBuilder();
                responseText.AppendLine(string.Format("{0} {1}", (int) response.StatusCode, response.StatusCode.ToString()));
                responseText.AppendLine(response.Headers.ToString());

                LogResponse(responseText.ToString(), "log");
            }
        }

        /// <summary>
        /// Determines whether the connection to InsureShip is secure by inspecting the certificate.
        /// </summary>
        /// <returns></returns>
        private bool IsTrustedCertificate()
        {
            ICertificateInspector certificateInspector = new CertificateInspector(TangoCounterRatesCredentialStore.Instance.InsureShipCertificateVerificationData);
            CertificateRequest request = new CertificateRequest(Settings.CertificateUrl, certificateInspector);

            return request.Submit() == CertificateSecurityLevel.Trusted;
        }
    }
}
