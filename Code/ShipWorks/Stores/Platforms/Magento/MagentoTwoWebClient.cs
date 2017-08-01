using System;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;
using System.Web;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Magento.Enums;
using Interapptive.Shared.Utility;

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
            try
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
            catch (MagentoException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Process update order
        /// </summary>
        private GenericModuleResponse ProcessUpdateOrder(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/rest/V1/shipworks/orders/update")
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
                Uri = new Uri(Store.ModuleUrl + "/rest/V1/shipworks" + $"{path}"),
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
            if (store.ModuleUsername == string.Empty)
            {
                request.Headers.Add(HttpRequestHeader.Authorization,
                    $"Bearer {SecureText.Decrypt(store.ModulePassword, string.Empty)}");
            }
            else
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    IMagentoTwoRestClient magentoRestClient =
                        scope.Resolve<IMagentoTwoRestClient>(new TypedParameter(typeof(MagentoStoreEntity), store));

                    request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {magentoRestClient.GetToken()}");
                }
            }

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
                    GenericModuleResponse response = new GenericModuleResponse(HttpUtility.HtmlDecode(xpath.Value));

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
                // attempt to translate the exception as a web exception
                Exception translatedException = WebHelper.TranslateWebException(ex, typeof(GenericStoreException));

                // if translating is successful the new exception should be a GenericStoreException
                // this is the preferred method because the translation process will preserve the stack trace
                if (translatedException.GetType() == typeof(GenericStoreException))
                {
                    throw translatedException;
                }

                // Translating failed, throw a new GenericStoreExcpetion
                throw new GenericStoreException(ex.Message, ex);
            }
        }
    }
}
