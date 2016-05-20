using System;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento two web client
    /// </summary>
    class MagentoTwoWebClient : MagentoWebClient
    {
        private readonly MagentoStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoTwoWebClient(MagentoStoreEntity store) : base(store)
        {
            this.store = store;
        }

        /// <summary>
        /// Intercept GenericStore requests and execute REST requests
        /// for our Magento 2 module
        /// </summary>
        protected override GenericModuleResponse ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            switch (action)
            {
                case "getstatuscodes":
                    return ProcessGetRequest("/store/statuscodes", "GetStatusCodes", request);

                case "getorders":
                    return ProcessGetRequest("/orders", "GetOrders", request);

                case "getcount":
                    return ProcessGetRequest("/orders/count", "GetCount", request);

                case "getmodule":
                    return ProcessGetRequest("/module", "GetModule", request);

                case "updateorder":
                    return ProcessUpdateOrder(request);

                case "getstore":
                    return ProcessGetRequest("/store", "GetStore", request);

                default:
                    throw new InvalidOperationException("MagentoTwoWebClient doesn't support action: " + action);
            }
        }
        private GenericModuleResponse ProcessUpdateOrder(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/orders/update")
            };

            // Generate the body of the request and add it to our request
            XElement postRequestBody = GeneratePostRequestBody(request);
            xmlRequest.Variables.Add(string.Empty, postRequestBody.ToString());

            return ProcessRequestInternal(xmlRequest, "UpdateOrder");
        }

        /// <summary>
        /// Generates an XElement document in the correct format for our magento module
        /// that contains the parameters from the HttpVaribleRequestSubmitter
        /// </summary>
        private XElement GeneratePostRequestBody(HttpVariableRequestSubmitter request)
        {
            // All of the variables go into the data element
            XElement body = new XElement("data");

            // add all of the variables to the data element
            foreach (HttpVariable var in request.Variables)
            {
                body.Add(new XElement(var.Name, var.Value));
            }

            // Create the document root element and add the body to it
            XElement requestXml = new XElement("request");
            requestXml.Add(body);

            return requestXml;
        }

        /// <summary>
        /// Processes a get request using the given parameters
        /// </summary>
        private GenericModuleResponse ProcessGetRequest(string path, string logName, HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + $"{path}"),
                Verb = HttpVerb.Get
            };

            AddVariablesToGetRequest(request, xmlRequest);

            return ProcessRequestInternal(xmlRequest, logName);
        }

        /// <summary>
        /// Takes the variables from one HttpVariableRequestSubmitter and moves them to another
        /// </summary>
        private void AddVariablesToGetRequest(HttpVariableRequestSubmitter from, HttpVariableRequestSubmitter to)
        {
            foreach (HttpVariable httpVariable in from.Variables)
            {
                to.Variables.Add(httpVariable);
            }
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private GenericModuleResponse ProcessRequestInternal(HttpRequestSubmitter request, string action)
        {
            request.Headers.Add(HttpRequestHeader.Authorization,
                $"Bearer {SecureText.Decrypt(store.ModulePassword, store.ModuleUsername)}");

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Magento, action);
            logEntry.LogRequest(request);

            try
            {
                using (IHttpResponseReader reader = request.GetResponse())
                {
                    string result = reader.ReadResult();
                    logEntry.LogResponse(result, "txt");

                    XmlDocument xmlResponse = new XmlDocument {XmlResolver = null};
                    xmlResponse.LoadXml(result);

                    XPathNavigator xpath = xmlResponse.CreateNavigator();
                    GenericModuleResponse response = new GenericModuleResponse(xpath.Value);

                    // Valid the module version and schema version
                    ValidateModuleVersion(response.ModuleVersion);
                    ValidateSchemaVersion(response.SchemaVersion);

                    if (response.HasError)
                    {
                        throw new GenericStoreException(response.ErrorDescription);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(GenericStoreException));
            }
        }
    }
}
