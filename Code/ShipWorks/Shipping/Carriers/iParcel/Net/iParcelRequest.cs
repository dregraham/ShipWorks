using System;
using System.Data;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.iParcel.WebServices;
using log4net;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.iParcel.Net
{
    /// <summary>
    /// Base class for iParcelRequests
    /// </summary>
    public abstract class iParcelRequest
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(iParcelRequest));

        private readonly iParcelCredentials credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelRequest" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="requestTypeName">Name of the request type for logging purposes.</param>
        protected iParcelRequest(iParcelCredentials credentials, string requestTypeName)
        {
            this.credentials = credentials;
            RequestTypeName = requestTypeName;

            RequestElements = new List<IiParcelRequestElement>();
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <value>The credentials.</value>
        protected iParcelCredentials Credentials { get { return credentials; } }
        
        /// <summary>
        /// Gets the request elements contained in the i-parcel request.
        /// </summary>
        /// <value>The request elements.</value>
        public List<IiParcelRequestElement> RequestElements { get; private set; }

        /// <summary>
        /// Gets the name of the operation being invoked on the i-parcel system.
        /// </summary>
        /// <value>The name of the operation.</value>
        public abstract string OperationName { get; }

        /// <summary>
        /// Gets the name of the root element for the XML sent in UploadXMLFile method of the 
        /// i-parcel web service.
        /// </summary>
        /// <value>The name of the root element.</value>
        public abstract string RootElementName { get; }

        /// <summary>
        /// Gets the name of the request type.
        /// </summary>
        /// <value>The name of the request type.</value>
        public string RequestTypeName { get; private set; }

        /// <summary>
        /// Uses the IiParcelRequestElement objects in the RequestElements list to generate
        /// the raw request XML to submit to i-parcel.
        /// </summary>
        /// <returns>An XML string</returns>
        public string GetRequestXml()
        {
            XElement root = new XElement(RootElementName);

            foreach(IiParcelRequestElement requestElement in RequestElements)
            {
                root.Add(requestElement.Build());
            }

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            return doc.ToString();
        }

        /// <summary>
        /// Executes the logged request.
        /// </summary>
        /// <returns>The raw response from iParcel in the form of a DataSet.</returns>
        public virtual DataSet Submit()
        {
            try
            {
                using (XMLSOAP iParcelWebService = new XMLSOAP(new ApiLogEntry(ApiLogSource.iParcel, RequestTypeName)))
                {
                    string requestXml = GetRequestXml();
                    // Submit the XML to i-parcel and perform some initial error checking
                    DataSet response = iParcelWebService.UploadXMLFile(OperationName, requestXml);
                    CheckForErrors(response);

                    return response;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in ExecuteLoggedRequest", ex);
                throw WebHelper.TranslateWebException(ex, typeof(iParcelException));
            }
        }

        /// <summary>
        /// Checks the given response for a table named ErrorInfo.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="iParcelException">Thrown when a null response is given or when the response contains a table named ErrorInfo</exception>
        protected virtual void CheckForErrors(DataSet response)
        {
            bool hasErrors = response == null || response.Tables.Count == 0 || response.Tables[0].Rows.Count == 0 || response.Tables.Contains("ErrorInfo");

            if (hasErrors)
            {
                if (response == null)
                {
                    throw new iParcelException("No response was returned from i-Parcel.");
                }

                string errorMessage = response.Tables["ErrorInfo"].Rows[0][0].ToString();
                throw new iParcelException(errorMessage);
            }
        }
        
    }
}