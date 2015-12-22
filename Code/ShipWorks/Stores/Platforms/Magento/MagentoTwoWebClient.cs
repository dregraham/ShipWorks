using System;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Magento
{
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
                    return ProcessGetStatusCodes(request);

                case "getorders":
                    return ProcessGetOrders(request);

                case "getcount":
                    return ProcessGetCount(request);

                case "getmodule":
                    return ProcessGetModule(request);

                case "updateorder":
                    return ProcessUpdateOrder(request);

                case "getstore":
                    return ProcessGetStore(request);

                default:
                    throw new InvalidOperationException("MagentoTwoWebClient doesn't support action: " + action);
            }
        }

        private GenericModuleResponse ProcessGetStore(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/store")
            };
            
            return ProcessRequestInternal(xmlRequest, "GetStore");
        }

        private GenericModuleResponse ProcessUpdateOrder(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/orders/update")
            };

            XElement requestXml = new XElement("request");

            XElement body = new XElement("data");

            foreach (HttpVariable var in request.Variables)
            {
                body.Add(new XElement(var.Name, var.Value));
            }

            requestXml.Add(body);

            xmlRequest.Variables.Add(string.Empty, requestXml.ToString());

            return ProcessRequestInternal(xmlRequest, "UpdateOrder");
        }

        private GenericModuleResponse ProcessGetModule(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/module")
            };
            
            return ProcessRequestInternal(xmlRequest, "GetModule");
        }

        private GenericModuleResponse ProcessGetCount(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/orders/count")
            };
            
            XElement body = new XElement("Request");

            foreach (HttpVariable var in request.Variables)
            {
                body.Add(new XElement(var.Name, var.Value));
            }

            xmlRequest.Variables.Add(string.Empty, body.ToString());

            return ProcessRequestInternal(xmlRequest, "GetCount");
        }

        private GenericModuleResponse ProcessGetOrders(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/orders")
            };

            XElement body = new XElement("Request");

            foreach (HttpVariable var in request.Variables)
            {
                body.Add(new XElement(var.Name, var.Value));
            }
            
            xmlRequest.Variables.Add(string.Empty, body.ToString());

            return ProcessRequestInternal(xmlRequest, "GetOrders");
        }

        private GenericModuleResponse ProcessGetStatusCodes(HttpVariableRequestSubmitter request)
        {
            HttpXmlVariableRequestSubmitter xmlRequest = new HttpXmlVariableRequestSubmitter
            {
                Uri = new Uri(Store.ModuleUrl + "/store/statuscodes")
            };

            return ProcessRequestInternal(xmlRequest, "GetStatusCodes");
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private GenericModuleResponse ProcessRequestInternal(HttpXmlVariableRequestSubmitter request, string action)
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
