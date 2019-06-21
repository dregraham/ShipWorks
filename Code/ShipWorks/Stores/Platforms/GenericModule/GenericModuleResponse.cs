using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Encapsulates a response from a shipworks store module.  Not all store types require a custom shipworks module.
    /// </summary>
    public class GenericModuleResponse
    {
        XmlDocument xmlResponse;
        XPathNavigator xpath;

        // Error code returned by the module
        string errorCode;

        // Error description returned by the module
        string errorDescription;

        // Version data
        Version moduleVersion;
        Version schemaVersion;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleResponse(string response)
        {
            try
            {
                xmlResponse = new XmlDocument();

                // we don't want to resolve external entities.
                xmlResponse.XmlResolver = null;

                xmlResponse.LoadXml(response);
            }
            catch (XmlException ex)
            {
                throw new GenericStoreException(ex.Message, ex);
            }

            xpath = xmlResponse.CreateNavigator();

            LoadResponseData();
        }

        /// <summary>
        /// XPath that can be used to navigate the response XML
        /// </summary>
        public XPathNavigator XPath
        {
            get { return xpath; }
        }

        /// <summary>
        /// The version of the shipworks.php module that was connected to
        /// </summary>
        public Version ModuleVersion
        {
            get { return moduleVersion; }
        }

        /// <summary>
        /// The generic module schema version used by the module.
        /// </summary>
        public Version SchemaVersion
        {
            get { return schemaVersion; }
        }

        /// <summary>
        /// Indiciates if the module reported an error
        /// </summary>
        public bool HasError
        {
            get { return errorCode.Length > 0; }
        }

        /// <summary>
        /// The error code the module returned, if any
        /// </summary>
        public string ErrorCode
        {
            get { return errorCode; }
        }

        /// <summary>
        /// The error description returned by the module, if any
        /// </summary>
        public string ErrorDescription
        {
            get
            {
                return errorDescription;
            }
        }

        /// <summary>
        /// Load all the base response data
        /// </summary>
        private void LoadResponseData()
        {
            errorCode = XPathUtility.Evaluate(xpath, "//Error/Code", "");

            try
            {
                moduleVersion = new Version(XPathUtility.Evaluate(xpath, "ShipWorks/@moduleVersion", "0.0.0"));
                schemaVersion = new Version(XPathUtility.Evaluate(xpath, "ShipWorks/@schemaVersion", "0.0.0"));
            }
            catch (Exception ex)
            {
                throw new GenericStoreException("The shipworks module version was not specified.  The module may be missing or corrupt.", ex);
            }

            // Error
            if (errorCode.Length > 0)
            {
                errorDescription = XPathUtility.Evaluate(xpath, "//Error/Description", "");

                // Remove extra white spaces
                errorDescription = Regex.Replace(errorDescription, @"\s+", " ");
            }
        }
    }
}
