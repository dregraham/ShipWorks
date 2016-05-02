using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.Stores.Platforms.Magento.WebServices;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Class for communivating with MagentoConnect module
    /// </summary>
    public class MagentoConnectWebClient : MagentoWebClient
    {
        // SOAP proxy
        MagentoService service = null;

        // current logon session
        string session = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoConnectWebClient(MagentoStoreEntity store) : base(store)
        {

        }

        /// <summary>
        /// Intercept GenericStore requests and execute SOAP requests instead
        /// </summary>
        protected override GenericModuleResponse ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            MagentoStoreEntity magentoStore = (MagentoStoreEntity) Store;
            request.Variables.Add("storecode", magentoStore.ModuleOnlineStoreCode);
            request.Variables.Add("sendemail", magentoStore.MagentoTrackingEmails ? "1" : "0");

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
                    throw new InvalidOperationException("MagentoConnectWebClient doesn't support action: " + action);
            }
        }

        /// <summary>
        /// Create the web proxy for communicating with Magento
        /// </summary>
        private MagentoService CreateService(string action)
        {
            service = new MagentoService(new ApplicationCore.Logging.ApiLogEntry(ApiLogSource.Magento, action));
            service.Url = GetServiceUrl();

            if (string.IsNullOrEmpty(session))
            {
                session = service.login(Store.ModuleUsername, SecureText.Decrypt(Store.ModulePassword, Store.ModuleUsername));
            }

            return service;
        }

        /// <summary>
        /// Constructs the url to the Soap endpoint
        /// </summary>
        private string GetServiceUrl()
        {
            Uri storeUri = new Uri(Store.ModuleUrl);

            return String.Format("{0}://{1}/{2}", storeUri.Scheme, storeUri.Host, "index.php/api/soap/index/");
        }

        /// <summary>
        /// Gets status codes
        /// </summary>
        private GenericModuleResponse ProcessGetStatusCodes(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("getStatusCodes", request.Variables);
        }

        /// <summary>
        /// Executes a GetModule request
        /// </summary>
        private GenericModuleResponse ProcessGetModule(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("getModule", request.Variables);
        }

        /// <summary>
        /// Executes a GetModule request
        /// </summary>
        private GenericModuleResponse ProcessGetStore(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("getStore", request.Variables);
        }

        /// <summary>
        /// Executes the getCount api call
        /// </summary>
        private GenericModuleResponse ProcessGetCount(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("getCount", request.Variables, "start", "storeCode");
        }

        /// <summary>
        /// Executes the getOrders api call
        /// </summary>
        private GenericModuleResponse ProcessGetOrders(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("getOrders", request.Variables, "start", "maxcount", "storecode");
        }

        /// <summary>
        /// Executes the updateOrder api call
        /// </summary>
        private GenericModuleResponse ProcessUpdateOrder(HttpVariableRequestSubmitter request)
        {
            return ProcessSoapRequest("updateOrder", request.Variables, "order", "command", "comments", "tracking", "carrier", "sendemail");
        }

        /// <summary>
        /// Executes a soap request and packages the result as if it were our Generic response
        /// </summary>
        private GenericModuleResponse ProcessSoapRequest(string api, HttpVariableCollection parameters, params string[] paramOrder)
        {
            try
            {
                using (MagentoService service = CreateService(api))
                {

                    object[] soapParameters;
                    if (paramOrder == null)
                    {
                        soapParameters = null;
                    }
                    else
                    {
                        List<object> temp = new List<object>();
                        foreach (string namedParameter in paramOrder)
                        {
                            temp.Add(parameters[namedParameter]);
                        }

                        soapParameters = temp.ToArray();
                    }

                    string result = service.call(session, String.Format("shipWorksApi.{0}", api), soapParameters) as string;

                    GenericModuleResponse response = new GenericModuleResponse(result);

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
