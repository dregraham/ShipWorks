using System.Xml;
using Interapptive.Shared.Utility;
using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Response from TangoWebClient.AddStore 
    /// </summary>
    public class AddStoreResponse : IAddStoreResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreResponse(XmlDocument xmlResponse)
        {
            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlResponse);

            int error = XPathUtility.Evaluate(xpath, "//Error/Code", 0);

            try
            {
                Result = EnumHelper.GetEnumByApiValue<LicenseActivationState>(error.ToString());
            }
            catch (InvalidOperationException)
            {
                Result = LicenseActivationState.UnknownError;
            }

            Key = XPathUtility.Evaluate(xpath, "//LicenseKey", "");

            Success = Result == LicenseActivationState.Active;
        }

        /// <summary>
        /// License Key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Error Message
        /// </summary>
        public LicenseActivationState Result { get; }

        /// <summary>
        /// Success!
        /// </summary>
        public bool Success { get; }
    }
}