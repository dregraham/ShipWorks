using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using log4net;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using ShipWorks.Shipping;
using ShipWorks.Data.Import.Xml.Schema;

namespace ShipWorks.Stores.Platforms.GenericModule
{
	/// <summary>
	/// Class for connecting to and working with our Generic php module
	/// </summary>
	public class GenericStoreWebClient
	{
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericStoreWebClient));

        // Store instance we're communicating on behalf of
        GenericModuleStoreEntity store = null;

		// Required module version
        private Version requiredModuleVersion;

        // Current schema version
        private Version currentSchemaVersion = new Version("1.1.0");

		/// <summary>
		/// Constructor for using the client to talk to a given store
		/// </summary>
		public GenericStoreWebClient(GenericModuleStoreEntity store)
		{
            this.store = store;

            GenericModuleStoreType type = (GenericModuleStoreType)StoreTypeManager.GetType(store);
            requiredModuleVersion = type.GetRequiredModuleVersion();
		}

        /// <summary>
        /// Gets the expected response encoding
        /// </summary>
        protected Encoding ResponseEncoding
        {
            get
            {
                if (store.ModuleResponseEncoding == (int) GenericStoreResponseEncoding.UTF8)
                {
                    return Encoding.UTF8;
                }
                else
                {
                    // latin-1
                    return Encoding.GetEncoding(28591);
                }
            }
        }

        /// <summary>
        /// The store the webClient is configured to connect to
        /// </summary>
        protected GenericModuleStoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Get the module and capabilities information from the module
        /// </summary>
        public GenericModuleResponse GetModule()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            return ProcessRequest(request, "getmodule");
        }

        /// <summary>
        /// Get the store details from the online store
        /// </summary>
        public GenericModuleResponse GetStore()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            return ProcessRequest(request, "getstore");
        }

        /// <summary>
        /// Gets the number of orders ready to download
        /// </summary>
        public virtual int GetOrderCount(long lastOrderNumber)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("start", lastOrderNumber.ToString());

            // execute the request
            GenericModuleResponse response = ProcessRequest(request, "getcount");

            // pull the count out of the response
            return XPathUtility.Evaluate(response.XPath, "//OrderCount", 0);
        }

        /// <summary>
        /// Get the number of orders that are newer than lastModified
        /// </summary>
        public virtual int GetOrderCount(DateTime? lastModified)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("start", FormatDate(lastModified));

            // execute the request
            GenericModuleResponse response = ProcessRequest(request, "getcount");

            // pull the count out of the response
            return XPathUtility.Evaluate(response.XPath, "//OrderCount", 0);
        }

        /// <summary>
        /// Downloads the next batch of orders from Generic, starting at the lastOrderNumber
        /// </summary>
        public virtual GenericModuleResponse GetNextOrderPage(long lastOrderNumber)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Variables.Add("start", lastOrderNumber.ToString());
            request.Variables.Add("maxcount", store.ModuleDownloadPageSize.ToString());

            return ProcessRequest(request, "getorders");
        }

        /// <summary>
        /// Downloads the next batch of orders from the web module, started on the lastModified time.
        /// </summary>
        public virtual GenericModuleResponse GetNextOrderPage(DateTime? lastModified)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Variables.Add("start", FormatDate(lastModified));
            request.Variables.Add("maxcount", store.ModuleDownloadPageSize.ToString());

            return ProcessRequest(request, "getorders");
        }

        /// <summary>
        /// Update the online status of the specified order
        /// </summary>
        public virtual void UpdateOrderStatus(OrderEntity order, object code, string comment)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            GenericModuleStoreType type = (GenericModuleStoreType)StoreTypeManager.GetType(store);

            request.Variables.Add("order", type.GetOnlineOrderIdentifier(order));
            request.Variables.Add("status", code.ToString());
            request.Variables.Add("comments", comment);

            ProcessRequest(request, "updatestatus");
        }

        /// <summary>
        /// Posts the tracking number for an order back to Generic
        /// </summary>
        public virtual void UploadShipmentDetails(OrderEntity order, ShipmentEntity shipment)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            GenericModuleStoreType type = (GenericModuleStoreType)StoreTypeManager.GetType(store);

            request.Variables.Add("order", type.GetOnlineOrderIdentifier(order));
            request.Variables.Add("tracking", shipment.TrackingNumber);
            request.Variables.Add("carrier", ShippingManager.GetCarrierName((ShipmentTypeCode)shipment.ShipmentType));
            request.Variables.Add("shippingcost", shipment.ShipmentCost.ToString());
            request.Variables.Add("shippingdate", FormatDate(shipment.ShipDate));

            AppendExtendedShipmentDetails(request, shipment);

            ProcessRequest(request, "updateshipment");
        }

        /// <summary>
        /// Appends the extended shipment details to the request's Variables collection if the store's module
        /// version >= 3.10.0.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="shipment">The shipment.</param>
	    private void AppendExtendedShipmentDetails(HttpVariableRequestSubmitter request, ShipmentEntity shipment)
	    {
	        Version moduleVersion = new Version(store.ModuleVersion);
	        if (moduleVersion.CompareTo(new Version("3.10.0")) >= 0)
	        {
	            // The version of the module is >= 3.10.0, so add the extended shipment
	            // information to the request. We wanted module providers to opt-in to
	            // this functionality to avoid negatively impacting overly strict module
	            // implementations that may refuse a request if the variables do not match
	            // exactly what the server is expecting.
	            request.Variables.Add("processeddate", shipment.ProcessedDate.HasValue ? shipment.ProcessedDate.Value.ToString("s") : string.Empty);

	            request.Variables.Add("voided", shipment.Voided.ToString(CultureInfo.InvariantCulture));
	            request.Variables.Add("voideddate", shipment.VoidedDate.HasValue ? shipment.VoidedDate.Value.ToString("s") : string.Empty);
	            request.Variables.Add("voideduser", shipment.VoidedUserID.HasValue ? shipment.VoidedUserID.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);

                request.Variables.Add("serviceused", ShippingManager.GetServiceUsed(shipment));
                request.Variables.Add("totalcharges", shipment.ShipmentCost.ToString());
                request.Variables.Add("totalweight", shipment.TotalWeight.ToString(CultureInfo.InvariantCulture));
                request.Variables.Add("returnshipment", shipment.ReturnShipment.ToString(CultureInfo.InvariantCulture));
            }
	    }

	    /// <summary>
        /// Retrieves the status code definitions from the online store
        /// </summary>
        public GenericModuleResponse GetStatusCodes()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            return ProcessRequest(request, "getstatuscodes");
        }

        /// <summary>
        /// Get the url from the store
        /// </summary>
	    protected virtual Uri GetUrlFromStore(GenericModuleStoreEntity genericStore)
	    {
            return new Uri(genericStore.ModuleUrl);
	    }

        /// <summary>
        /// Submits a request to the online store and returns the response
        /// </summary>
        [NDependIgnoreLongMethod]
        protected virtual GenericModuleResponse ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)store;
            GenericModuleStoreType genericStoreType = (GenericModuleStoreType)StoreTypeManager.GetType(genericStore);

            // add the action parameter
            request.Variables.Add("action", action);

            // if a store code is defined, pass it along
            if (genericStore.ModuleOnlineStoreCode.Length > 0)
            {
                request.Variables.Add("storecode", genericStore.ModuleOnlineStoreCode);
            }

            string username = genericStore.ModuleUsername;
            string password = genericStore.ModulePassword;
            password = SecureText.Decrypt(password, username);

            // add the username and password as parameters
            request.Variables.Add("username", username);
            request.Variables.Add("password", password);

            // setup the uri
            try
            {
                request.Uri = GetUrlFromStore(store);
            }
            catch (UriFormatException ex)
            {
                throw new GenericStoreException("The module URL is not properly formatted.", ex);
            }

            request.Timeout = TimeSpan.FromSeconds(genericStore.ModuleRequestTimeout);
            request.Credentials = new NetworkCredential(username, password);

            if (String.Compare(action, "getmodule", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // some servers will fail if this is True.  The communications settings in the getmodule response control this value,
                // but we have to make a succesful getmodule call to begin with.  This will allow that call to go through.
                request.Expect100Continue = false;
            }
            else
            {
                request.Expect100Continue = store.ModuleHttpExpect100Continue;
            }

            // Allow derived classes to adjust the request
            TransformRequest(request, action);

            // log the request
            ApiLogEntry logger = new ApiLogEntry(genericStoreType.LogSource, action);
            logger.LogRequest(request);

            GenericModuleResponse webResponse;

            // execute the request
            try
            {
                using (IHttpResponseReader postResponse = request.GetResponse())
                {
                    string resultXml = postResponse.ReadResult(ResponseEncoding);

                    // This was added for Miva but is fine in the general case.  An XML Document cannot start with whitespace
                    if (!resultXml.StartsWith("<"))
                    {
                        resultXml = resultXml.Trim();
                    }

                    // log the response
                    logger.LogResponse(resultXml);

                    // Strip invalid input characters.  Defensive against bad modules
                    resultXml = XmlUtility.StripInvalidXmlCharacters(resultXml);

                    string transformedXml = TransformResponse(resultXml, action);

                    // if the response was changed, log it
                    if (!resultXml.Equals((object)transformedXml))
                    {
                        logger.LogResponseSupplement(transformedXml, "Transformed");
                    }

                    // Load the response from the xml
                    webResponse = new GenericModuleResponse(transformedXml);

                    // Valid the module version and schema version
                    ValidateModuleVersion(webResponse.ModuleVersion);
                    ValidateSchemaVersion(webResponse.SchemaVersion);

                    // validate the response against the schema
                    ValidateSchema(transformedXml, action);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(GenericStoreException));
            }

            // check for errors
            if (webResponse.HasError)
            {
                throw new GenericStoreException(webResponse.ErrorDescription);
            }

            return webResponse;
        }

        /// <summary>
        /// Loads a GenericModuleResponse from the specified file
        /// </summary>
        public GenericModuleResponse ResponseFromFile(string file, string action)
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file", "A file must be specified.");
            }

            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException("action", "An action must be specified");
            }

            if (!File.Exists(file))
            {
                throw new FileNotFoundException(String.Format("Unable to locate response file '{0}'", file), file);
            }

            // all files we are loading are GetOrders responses
            string resultXml = File.ReadAllText(file);

            string transformedXml = TransformResponse(resultXml, action);

            // Load the response from the xml
            GenericModuleResponse webResponse = new GenericModuleResponse(transformedXml);

            // Valid the module version and schema version
            ValidateModuleVersion(webResponse.ModuleVersion);
            ValidateSchemaVersion(webResponse.SchemaVersion);

            // validate the response against the schema
            ValidateSchema(transformedXml, action);

            return webResponse;
        }

        /// <summary>
        /// The request construction is complete and ready to be executed.
        /// For derived classes.
        /// </summary>
        protected virtual void TransformRequest(HttpVariableRequestSubmitter request, string action)
        {

        }

        /// <summary>
        /// Transform the Module Response Xml, provided for derived classes, base
        /// implementation does nothing.
        /// </summary>
        protected virtual string TransformResponse(string resultXml, string action)
        {
            return resultXml;
        }

        /// <summary>
        /// Validates the version fo the module against the specified required module version
        /// </summary>
        protected void ValidateModuleVersion(Version moduleVersion)
        {
            if (requiredModuleVersion == null)
            {
                return;
            }

            // Only check if a valid module version was specified.  If it wasn't specified it defaults to zero, which is probably going to indicate
            // a schema violation, which will be checked later.
            if (moduleVersion.Complete() > new Version("0.0.0.0"))
            {
                // Check the version of the module
                if (moduleVersion.Complete() < requiredModuleVersion.Complete())
                {
                    throw new GenericStoreException(string.Format(
                        "Your ShipWorks module needs to be updated.\n\n" +
                        "Version {0} of the module is installed in your online store.\n" +
                        "This version of ShipWorks requires module version {1}.\n\n" +
                        "For instructions on upgrading the module, please see the help file.\n",
                        moduleVersion, requiredModuleVersion));
                }
            }
        }

        /// <summary>
        /// Validate that the schema version specified by the module is compatible.
        /// </summary>
        protected void ValidateSchemaVersion(Version schemaVersion)
        {
            // 1. Its ok if the current schema version is greater than what the module is using.  We will strive for backwards compatibility
            //    so that's ok.
            // 2. When we version the schema version an update to the major\minor will indicate a breaking change.  We can update the 3rd digit
            //    for non-breaking changes.

            // If the major\minor of the module's schema version is bigger than ours, that means there is a newer version of ShipWorks out
            // with a schema format not compatible with this version of shipworks.
            if (schemaVersion.Major > currentSchemaVersion.Major ||
                (schemaVersion.Major == currentSchemaVersion.Major && schemaVersion.Minor > currentSchemaVersion.Minor))
            {
                throw new GenericStoreException(string.Format(
                    "Your version of ShipWorks needs to be updated.\n\n" +
                    "Your ShipWorks store module is using schema version {0}, " +
                    "but this version of ShipWorks only supports up to schema version {1}.\n",
                    schemaVersion, currentSchemaVersion.ToString(3)));
            }
        }

        /// <summary>
        /// Validates the xml response against the ShipWorks schema
        /// </summary>
        protected virtual void ValidateSchema(string resultXml, string action)
        {
            try
            {
                XmlDocument xmlResponse = new XmlDocument();

                // don't resolve external references
                xmlResponse.XmlResolver = null;
                xmlResponse.LoadXml(resultXml);

                // Ensure the root element is ShipWorks, since validation won't throw an error if it isn't
                CheckDocumentRoot(xmlResponse);

                List<string> validationErrors = ShipWorksSchemaValidator.FindValidationErrors(xmlResponse, ShipWorksSchema.Module);

                if (validationErrors.Count > 0)
                {
                    ThrowSchemaValidationException(validationErrors[0], xmlResponse);
                }
            }
            catch (XmlException ex)
            {
                throw new GenericStoreException(ex.Message, ex);
            }
            catch (XmlSchemaValidationException ex)
            {
                throw new GenericStoreException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Throws an XmlSchemaValidationException with an appropriate error message based on the response contents.
        /// </summary>
        private void ThrowSchemaValidationException(string validationError, XmlDocument xmlResponse)
        {
            // spot-check to see if the failed XML looks like it was a valid V2 response, indicating
            // the module just needs to be updated
            if (validationError.Contains("The required attribute 'moduleVersion' is missing"))
            {
                string statedVersion = XPathUtility.Evaluate(xmlResponse.CreateNavigator(), "//ShipWorks/ModuleVersion", "");
                Version moduleVersion = null;
                if (Version.TryParse(statedVersion, out moduleVersion))
                {
                    if (moduleVersion < new Version("3.0.0"))
                    {
                        ThrowOld2xModuleException();
                    }
                }
            }

            throw new XmlSchemaValidationException(string.Format("The ShipWorks Module response does not conform to the published schema.\n\n{0}", validationError));
        }

        /// <summary>
        /// Throws an XmlSchemaValidationException that indicates the module is old and needs upgraded
        /// </summary>
        private void ThrowOld2xModuleException()
        {
            string errorMessage = "The online store module needs to be upgraded to be compatible with ShipWorks 3.";

            // test for SW provided store types.  Other generic stores should be using the legacy client.
            List<int> interapptiveModules = new List<int>()
                        {
                            (int)StoreTypeCode.Magento,
                            (int)StoreTypeCode.Miva,
                            (int)StoreTypeCode.osCommerce,
                            (int)StoreTypeCode.XCart,
                            (int)StoreTypeCode.ZenCart
                        };

            if (interapptiveModules.Contains(Store.TypeCode))
            {
                errorMessage += "\n\nVisit www.interapptive.com/shipworks/download.php to obtain an updated module.";
            }

            throw new XmlSchemaValidationException(errorMessage);
        }

        /// <summary>
        /// Ensures that the document root has a certain name and namespace.
        /// </summary>
        private void CheckDocumentRoot(XmlDocument xmlResponse)
        {
            if (xmlResponse.DocumentElement.NamespaceURI.Length > 0 ||
                xmlResponse.DocumentElement.LocalName != "ShipWorks")
            {
                // The old miva root element was "SqlSchema"
                if (Store.TypeCode == (int) StoreTypeCode.Miva && xmlResponse.DocumentElement.LocalName == "SqlSchema")
                {
                    ThrowOld2xModuleException();
                }

                log.Error("ShipWorks Module XML Response error: The module's XML response contains an invalid root node.");
                throw new XmlException("ShipWorks Module response does not have the required ShipWorks root element.");
            }
        }

        /// <summary>
        /// Formats the date string how it needs to be sent
        /// </summary>
        private string FormatDate(DateTime? lastModified)
        {
            DateTime date = lastModified ?? DateTime.MinValue;
            return date.ToString("s");
        }
    }
}
