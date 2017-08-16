using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// Web client for backward compatibility with older V2
    /// ShipWorks modules.  Provides parameter name/value and response transformation
    /// </summary>
    public class LegacyAdapterStoreWebClient : GenericStoreWebClient
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(LegacyAdapterStoreWebClient));

        // URI of the xsl extension
        const string shipworksUriBase = "http://www.interapptive.com/shipworks/";

        // action used to determine SW module version compatibility
        const string ACTION_VERSIONPROBE = "version_probe";

        // Tracks which URL's are known conform to the legacy 2x way of doing modules
        static Dictionary<string, bool> compatibilityMap = new Dictionary<string, bool>();

        // mapping of V3 parameter name to legacy parameter name
        Dictionary<string, VariableTransformer> variableTransformers;

        // new variables to be submitted with the request
        NameValueCollection newVariables = new NameValueCollection();

        // provides Capabilities values for the stylesheet to output the store Capabilities node for
        GenericModuleCapabilities legacyCapabilities;

        // stylesheet to be used to transform communications
        XslCompiledTransform stylesheet;

        // Some modules fail the version probe with a non OK HttpStatusCode.  If that's the case this can be set so the client
        // knows when a failure is just an indicator to go into compatibility mode.
        HttpStatusCode? versionProbeCompatibilityIndicator = null;

        // Whether or not to also send variables as HTTP Gets
        HttpVerb compatibilityVerb = HttpVerb.Get;

        /// <summary>
        /// Gets the collection of variables to be added to every request
        /// </summary>
        public NameValueCollection AdditionalVariables
        {
            get { return newVariables; }
        }

        /// <summary>
        /// Constructor specifying the stylesheet used to transform downloaded data
        /// </summary>
        public LegacyAdapterStoreWebClient(GenericModuleStoreEntity store, XslCompiledTransform communicationStylesheet, GenericModuleCapabilities legacyCapabilities)
            : this(store, communicationStylesheet, legacyCapabilities, null)
        {

        }

        /// <summary>
        /// Constructor for providing a mapping of old parameter names to new ones
        /// </summary>
        public LegacyAdapterStoreWebClient(
            GenericModuleStoreEntity store,
            XslCompiledTransform communicationStylesheet,
            GenericModuleCapabilities legacyCapabilities,
            Dictionary<string, VariableTransformer> variableTransformers)
            : base(store)
        {
            if (communicationStylesheet == null)
            {
                throw new ArgumentNullException("communicationStylesheet", "A stylesheet must be specified for the communication transform.");
            }

            if (legacyCapabilities == null)
            {
                throw new ArgumentNullException("legacyCapabilities", "Adapter capapabilities must be specified.");
            }

            // stylesheet for transforming module responses
            this.stylesheet = communicationStylesheet;

            // store capabilities
            this.legacyCapabilities = legacyCapabilities;

            // variable transformation
            if (variableTransformers == null)
            {
                this.variableTransformers = new Dictionary<string, VariableTransformer>(StringComparer.InvariantCultureIgnoreCase);
            }
            else
            {
                // copy the dictionary, with a new comparer
                this.variableTransformers = new Dictionary<string, VariableTransformer>(variableTransformers, StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Specify whether or not variables will be placed on the querystring when running
        /// in compatibility mode
        /// </summary>
        public HttpVerb CompatibilityVerb
        {
            get { return compatibilityVerb; }
            set { compatibilityVerb = value; }
        }

        /// <summary>
        /// Some modules fail the version probe with a non OK HttpStatusCode.  If that's the case this can be set so the client
        /// knows when a failure is just an indicator to go into compatibility mode.
        /// </summary>
        public HttpStatusCode? VersionProbeCompatibilityIndicator
        {
            get { return versionProbeCompatibilityIndicator; }
            set { versionProbeCompatibilityIndicator = value; }
        }

        /// <summary>
        /// Process the request.  This override ensures we are initialized and know if we are in compatibility mode first.
        /// </summary>
        protected override GenericModuleResponse ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            bool compatibilityMode = IsCompatibilityMode();

            // getmodule didn't even exist before - so we just create the response from scratch ourselves.
            if (compatibilityMode && action == "getmodule")
            {
                return SimulateGetModuleRequest();
            }

            // Process the request
            return base.ProcessRequest(request, action);
        }

        /// <summary>
        /// Indicates if the current web client should run in compatibility mode
        /// </summary>
        private bool IsCompatibilityMode()
        {
            bool compatibilityMode;
            if (!compatibilityMap.TryGetValue(Store.ModuleUrl, out compatibilityMode))
            {
                compatibilityMode = DetermineCompatibiltyMode();
                compatibilityMap[Store.ModuleUrl] = compatibilityMode;
            }

            return compatibilityMode;
        }

        /// <summary>
        /// Determines if the client is connecting to a module written for ShipWorks version 2.
        /// </summary>
        private bool DetermineCompatibiltyMode()
        {
            // make a quick web call to the store and determine if we're talking to a V2 or V3 module.
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            // if the client's CompatibilityIndicator is MovedPermanently, disable automatic redirects on the request
            if (versionProbeCompatibilityIndicator.HasValue && versionProbeCompatibilityIndicator == HttpStatusCode.MovedPermanently)
            {
                request.AllowAutoRedirect = false;
            }

            // Execute the request
            try
            {
                log.Info("Probing store module schema version.");

                GenericModuleResponse response = base.ProcessRequest(request, ACTION_VERSIONPROBE);
                log.Info("Call succeeded.");

                // Look for SchemaVersion.  If it is present, then it has to be the new format, as the old format doesn't even have it.
                if (response.SchemaVersion >= new Version("1.0.0"))
                {
                    log.Info("Web client not running in Compatibility Mode.");

                    // only SW 3 modules report 1.0 or higher
                    return false;
                }
                else
                {
                    // SW 2 doesn't even have the schema node, and transforms default to 0.0.0
                    log.Info("Web client switching to Compatibility Mode.");
                    return true;
                }
            }
            catch (GenericStoreException ex)
            {
                if (ex.InnerException is XmlSchemaValidationException)
                {
                    log.Info("Web client switching to Compatibility Mode (schema validation failure).");
                    return true;
                }

                WebException webException = ex.InnerException as WebException;
                if (webException != null && versionProbeCompatibilityIndicator != null)
                {
                    HttpWebResponse response = webException.Response as HttpWebResponse;
                    if (response != null && response.StatusCode == versionProbeCompatibilityIndicator)
                    {
                        log.Info("Web client switching to Compatibility Mode (HTTP status code check).");
                        return true;
                    }

                    // HTTP 301 errors don't have a Response so can't actually show up on the StatusCode.  Must look directly at the error message.
                    if (versionProbeCompatibilityIndicator == HttpStatusCode.MovedPermanently &&
                        String.Compare(webException.Message, "Moved Permanently", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        log.Info("Web client switching to Compatibility Mode (Moved Permanently).");
                        return true;
                    }
                }

                // finally, if the module returned invalid XML, switch to compatibility mode
                if (ex.InnerException is XmlException)
                {
                    return true;
                }

                throw;
            }
        }

        /// <summary>
        /// Perform any manipulation of the request variables before it is sent
        /// </summary>
        protected override void TransformRequest(IHttpVariableRequestSubmitter request, string action)
        {
            // If we aren't in compatibility mode there is nothing to transform
            if (action == ACTION_VERSIONPROBE || !IsCompatibilityMode())
            {
                return;
            }

            // set the request verb for running in compatibility mode
            request.Verb = CompatibilityVerb;

            // Perform any variable transformations
            for (int i = request.Variables.Count - 1; i >= 0; i--)
            {
                HttpVariable currentVariable = request.Variables[i];

                // determine if there's a transformer available
                if (!variableTransformers.ContainsKey(currentVariable.Name))
                {
                    // go to the next
                    continue;
                }

                VariableTransformer transformer = variableTransformers[currentVariable.Name];
                if (transformer == null)
                {
                    throw new InvalidOperationException("Cannot register a null PostVariableTransformer.  To remove a parameter, provide a PostVarialbeTransformer that provides a null value.");
                }

                // remove the old variable
                request.Variables.RemoveAt(i);

                // transform the variable to a new one
                HttpVariable replacementVariable = transformer.TransformVariable(currentVariable);

                // null indicates we just remove the parameter, so only add in a transformed value if there is one
                if (replacementVariable != null)
                {
                    request.Variables.Insert(i, replacementVariable);
                }
            }

            // add any variables to the request
            foreach (string newVariableName in newVariables.AllKeys)
            {
                request.Variables.Add(newVariableName, newVariables[newVariableName]);
            }
        }

        /// <summary>
        /// Transforms the xml with the stylesheet
        /// </summary>
        protected override string TransformResponse(string resultXml, string action)
        {
            // Since we don't even go to the web for "getmodule" when its in compatibility mode
            // we know we aren't transforming that.  And if its not compatibility mode, obviously we don't do anything.
            if (action == ACTION_VERSIONPROBE || !IsCompatibilityMode() || action == "getmodule")
            {
                return resultXml;
            }

            try
            {
                // transform the response using the stylesheet that is configured
                using (StringReader reader = new StringReader(resultXml))
                {
                    XmlReader xmlReader = XmlReader.Create(reader);

                    try
                    {
                        using (StringWriter stringWriter = new StringWriter())
                        {
                            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                            // setup the arguments for the transformation
                            XsltArgumentList args = new XsltArgumentList();
                            args.AddParam("moduleAction", "", action);
                            args.AddExtensionObject(shipworksUriBase + "extensions", new LegacyAdapterXslExtensions());

                            // run the transform
                            stylesheet.Transform(xmlReader, args, xmlTextWriter);

                            xmlTextWriter.Flush();

                            // return the transformed document
                            return stringWriter.ToString();
                        }
                    }
                    finally
                    {
                        xmlReader.Close();
                    }
                }
            }
            catch (XsltException ex)
            {
                // transformation failure
                throw new GenericStoreException("Unable to transform response: " + ex.Message, ex);
            }
            catch (XmlException ex)
            {
                // If an HTML response containing a DTD is attempted to be transformed, XmlException is raised.
                throw new GenericStoreException("ShipWorks is unable to communicate with the store.  Check the url and credentials and try again.", ex);
            }
        }

        /// <summary>
        /// Since "getmodule" did not exist for legacy modules, we simulate it
        /// </summary>
        private GenericModuleResponse SimulateGetModuleRequest()
        {
            StoreType storeType = StoreTypeManager.GetType(Store);

            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                xmlWriter.WriteStartElement("ShipWorks");

                // Just use some fake hard-coded values that will validate
                xmlWriter.WriteAttributeString("moduleVersion", "2.9.0");
                xmlWriter.WriteAttributeString("schemaVersion", "1.0.0");

                xmlWriter.WriteStartElement("Module");

                xmlWriter.WriteElementString("Platform", storeType.StoreTypeName);
                xmlWriter.WriteElementString("Developer", string.Format("{0} (ShipWorks Legacy Transform)", storeType.StoreTypeName));

                xmlWriter.WriteStartElement("Capabilities");

                xmlWriter.WriteElementString("DownloadStrategy", GetDownloadStrategyValue(legacyCapabilities.DownloadStrategy));

                xmlWriter.WriteStartElement("OnlineCustomerID");
                xmlWriter.WriteAttributeString("supported", legacyCapabilities.OnlineCustomerSupport ? "true" : "false");
                xmlWriter.WriteAttributeString("dataType", GetDataTypeValue(legacyCapabilities.OnlineCustomerDataType));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("OnlineStatus");
                xmlWriter.WriteAttributeString("supported", legacyCapabilities.OnlineStatusSupport != GenericOnlineStatusSupport.None ? "true" : "false");
                xmlWriter.WriteAttributeString("dataType", GetDataTypeValue(legacyCapabilities.OnlineStatusDataType));
                xmlWriter.WriteAttributeString("supportsComments", legacyCapabilities.OnlineStatusSupport == GenericOnlineStatusSupport.StatusWithComment ? "true" : "false");
                xmlWriter.WriteAttributeString("downloadOnly", legacyCapabilities.OnlineStatusSupport == GenericOnlineStatusSupport.DownloadOnly ? "true" : "false");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("OnlineShipmentUpdate");
                xmlWriter.WriteAttributeString("supported", legacyCapabilities.OnlineShipmentDetails ? "true" : "false");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                return new GenericModuleResponse(stringWriter.ToString());
            }
        }

        /// <summary>
        /// Get the string to use for the given enum
        /// </summary>
        private static string GetDataTypeValue(GenericVariantDataType dataType)
        {
            switch (dataType)
            {
                case GenericVariantDataType.Text: return "text";
                case GenericVariantDataType.Numeric: return "numeric";
            }

            throw new InvalidOperationException("Unhandled dataType: " + dataType);
        }

        /// <summary>
        /// Get the string to use for the given enum
        /// </summary>
        private string GetDownloadStrategyValue(GenericStoreDownloadStrategy downloadStrategy)
        {
            switch (downloadStrategy)
            {
                case GenericStoreDownloadStrategy.ByOrderNumber: return "ByOrderNumber";
                case GenericStoreDownloadStrategy.ByModifiedTime: return "ByModifiedTime";
            }

            throw new InvalidOperationException("Unhandled dataType: " + downloadStrategy);
        }
    }
}
