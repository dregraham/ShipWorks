using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Net;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using log4net;

namespace ShipWorks.Stores.Platforms.Generic.LegacyAdapter
{
    /// <summary>
    /// Web client for backward compatiblity with older V2
    /// ShipWorks modules.  Provides parameter name/value and response transformation
    /// </summary>
    public class LegacyAdapterStoreWebClient : GenericStoreWebClient
    {
        // URI of the xsl extension
        const string shipworksUri = "http://www.interapptive.com/shipworks";

        // tracks if the client has been initialized
        bool isInitialized = false;

        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(LegacyAdapterStoreWebClient));

        // action used to determine SW module version compatibility
        const string ACTION_VERSIONPROBE = "version_probe"; 

        // mapping of V3 parameter name to legacy parameter name
        Dictionary<string, PostVariableTransformer> variableTransformers;

        // provides Capabilities values for the stylesheet to output the store Capabilities node for
        LegacyAdapterCapabilities adapterCapabilities;

        // stylesheet to be used to transform communications
        XslCompiledTransform stylesheet;

        // Compatibility Mode is use when talking to V2 modules
        bool compatibilityMode = false;

          /// <summary>
        /// Constructor specifying the stylesheet used to transform downloaded data
        /// </summary>
        public LegacyAdapterStoreWebClient(GenericStoreEntity store, XslCompiledTransform communicationStylesheet, LegacyAdapterCapabilities adapterCapabilities)
            : this(store, communicationStylesheet, adapterCapabilities, null)
        {
            
        }

        /// <summary>
        /// Constructor for providing a mapping of old parameter names to new ones
        /// </summary>
        public LegacyAdapterStoreWebClient(GenericStoreEntity store, XslCompiledTransform communicationStylesheet, 
            LegacyAdapterCapabilities adapterCapabilities, Dictionary<string, PostVariableTransformer> variableTransformers)
            : base(store)
        {
            #region validation

            if (communicationStylesheet == null)
            {
                throw new ArgumentNullException("communicationStylesheet", "A stylesheet must be specified for the communication transform.");
            }

            if (adapterCapabilities == null)
            {
                throw new ArgumentNullException("adapterCapabilities", "Adapter capapabilities must be specified.");
            }

            #endregion

            // stylesheet for tranforming module responses
            this.stylesheet = communicationStylesheet;

            // store capabilities
            this.adapterCapabilities = adapterCapabilities;

            // variable transformation
            if (variableTransformers == null)
            {
                this.variableTransformers = new Dictionary<string, PostVariableTransformer>(StringComparer.InvariantCultureIgnoreCase);
            }
            else
            {
                // copy the dictionary, with a new comparer
                this.variableTransformers = new Dictionary<string, PostVariableTransformer>(variableTransformers, StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Determines if the client is connecting to a module written for ShipWorks version 2.
        /// </summary>
        public void Initialize()
        {
            // make a quick web call to the store and determine if we're talking to a V2 or V3 module.
            HttpVariablePostRequest request = new HttpVariablePostRequest();

            // execute the request
            try
            {
                log.Info("Probing store module ShipWorksVersion.");

                ShipWorksModuleResponse response = ProcessRequest(request, ACTION_VERSIONPROBE);

                log.Info("Call succeeded.");

                // look for ShipWorksVersion
                if (response.ShipWorksVersion >= new Version("3.0.0"))
                {
                    log.Info("Web client not running in Compatibility Mode.");

                    // onliy SW 3 modules report 3.0 or higher
                    compatibilityMode = false;
                }
                else
                {
                    // SW 2 don't even have the ShipWorksVersion node, and transforms default to 2.0.0
                    compatibilityMode = true;
                    log.Info("Web client switching to Compatibility Mode.");
                }
            }
            catch (WebClientException ex)
            {
                if (ex.InnerException is XmlSchemaValidationException)
                {
                    compatibilityMode = true;
                    log.Info("Web client switching to Compatibility Mode.");
                }

                // Considering tracking compatibilityMode state transitions and raising an error if we go from no->yes 
                // to help module authors get converted to V3.  As-is, it would silently go to the transformed version 
                // and look like everything is ok.
            }
        }


        /// <summary>
        /// Perform any manipulation of the request variables before it is sent 
        /// </summary>
        protected override void OnRequestPrepared(HttpVariablePostRequest request, string action)
        {
            // do not perform any translations on the version probe call
            if ((action != ACTION_VERSIONPROBE) && compatibilityMode)
            {
                // Perform any variable transformations
                for (int i = request.Variables.Count - 1; i >= 0; i--)
                {
                    HttpPostVariable currentVariable = request.Variables[i];

                    // determine if there's a transformer available 
                    if (!variableTransformers.ContainsKey(currentVariable.Name))
                    {
                        // go to the next
                        continue;
                    }

                    PostVariableTransformer transformer = variableTransformers[currentVariable.Name];
                    if (transformer == null)
                    {
                        throw new InvalidOperationException("Cannot register a null PostVariableTransformer.  To remove a parameter, provide a PostVarialbeTransformer that provides a null value.");
                    }

                    // remove the old variable
                    request.Variables.RemoveAt(i);

                    // transform the variable to a new one
                    HttpPostVariable replacementVariable = transformer.TransformVariable(currentVariable);

                    // null indicates we just remove the parameter, so only add in a transformed value if there is one
                    if (replacementVariable != null)
                    {
                        request.Variables.Insert(i, replacementVariable);
                    }
                }
            }
        }

        /// <summary>
        /// Maps new parameter names to old
        /// </summary>
        protected override ShipWorksModuleResponse ProcessRequest(HttpVariablePostRequest request, string action)
        {
            // perform initialization first if it is needed
            if (!isInitialized && action != ACTION_VERSIONPROBE)
            {
                Initialize();
                isInitialized = true;
            }

            return base.ProcessRequest(request, action);
        }

        /// <summary>
        /// Transforms the xml with the stylesheet
        /// </summary>
        protected override string TransformResponse(string resultXml, string action)
        {
            // only transform the response if we're running in compatibility mode AND
            // we aren't working with the Version Probe request - which we want untouched
            if ((action != ACTION_VERSIONPROBE) && compatibilityMode)
            {
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
                                args.AddExtensionObject(shipworksUri, adapterCapabilities);

                                // run the transform
                                stylesheet.Transform(xmlReader, args, xmlTextWriter);

                                xmlTextWriter.Flush();

                                // return the trasformed document
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
                    throw new WebClientException("Unable to transform response: " + ex.Message, ex);
                }
            }
            else
            {
                // not running in compatibility mode, to no transforming neccessary. 
                return resultXml;
            }
        }
    }
}
