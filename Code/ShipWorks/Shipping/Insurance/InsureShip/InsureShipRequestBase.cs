using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Abstract Base of an InsureShipRequest
    /// </summary>
    public abstract class InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        protected InsureShipRequestBase(ShipmentEntity shipment, InsureShipAffiliate affiliate)
        {
            Shipment = shipment;
            Affiliate = affiliate;
            Settings = new InsureShipSettings();
            ResponseFactory = new InsureShipResponseFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        protected InsureShipRequestBase(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log)
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
        protected IInsureShipResponseFactory ResponseFactory
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        protected ILog Log
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        public ShipmentEntity Shipment
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets or sets the affiliate.
        /// </summary>
        public InsureShipAffiliate Affiliate 
        { 
            get; 
            private set;
        }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        public virtual int StatusCode
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a response exception, null if one did not occurr.
        /// </summary>
        public virtual Exception ResponseException
        {
            get;
            set;
        }

        protected IInsureShipSettings Settings
        {
            get; 
            set;
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public abstract IInsureShipResponse Submit();

        /// <summary>
        /// Gets the unique shipment identifier.
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
        /// Submits byte data to a URI via POST.  Sets the StatusCode based on response.
        /// </summary>
        protected IInsureShipResponse SubmitPost(Uri postUri, string postData)
        {
            HttpWebRequest request = WebRequest.Create(postUri) as HttpWebRequest;
            
            byte[] postBytes = Encoding.UTF8.GetBytes(postData.ToCharArray());

            string auth = string.Format("Basic {0}",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Settings.Username, Settings.Password)))
                );

            request.Method = "POST";
            request.PreAuthenticate = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            request.Accept = "application/json";
            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            request.Headers.Add("Authorization", auth);

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postData.Length);
                requestStream.Close();
            }

            HttpWebResponse webResponse = null;
            try
            {
                webResponse = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                ResponseException = ex;
                webResponse = (HttpWebResponse)ex.Response;
            }

            StatusCode = (int) webResponse.StatusCode;

            return ResponseFactory.CreateInsureShipmentResponse(this);
        }
    }
}
