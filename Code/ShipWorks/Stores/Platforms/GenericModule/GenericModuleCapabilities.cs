using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using System.Xml.XPath;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Data structure for storing the capabilities of a generic module
    /// </summary>
    public class GenericModuleCapabilities
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleCapabilities()
        {

        }

        /// <summary>
        /// The download strategy
        /// </summary>
        public GenericStoreDownloadStrategy DownloadStrategy { get; set; }

        /// <summary>
        /// The level of support for online status
        /// </summary>
        public GenericOnlineStatusSupport OnlineStatusSupport { get; set; }

        /// <summary>
        /// DataType of online status code, if supported
        /// </summary>
        public GenericVariantDataType OnlineStatusDataType { get; set; }

        /// <summary>
        /// Indiciates if online customer ID's are supported
        /// </summary>
        public bool OnlineCustomerSupport { get; set; }

        /// <summary>
        /// DataType of online customer ID's, if supported
        /// </summary>
        public GenericVariantDataType OnlineCustomerDataType { get; set; }

        /// <summary>
        /// Indiciates if uploading online shipment details is supported
        /// </summary>
        public bool OnlineShipmentDetails { get; set; }

        /// <summary>
        /// Indicates if milliseconds should be included in dates
        /// </summary>
        public bool IncludeMilliseconds { get; set; }

        /// <summary>
        /// Read the values from the module response
        /// </summary>
        public void ReadModuleResponse(XPathNavigator xpath)
        {
            // validate that we have a Capabilities element
            if (XPathUtility.Evaluate(xpath, "count(//Capabilities)", 0) == 0)
            {
                throw new GenericModuleConfigurationException("Generic Store Module Capabilities are missing from the response.");
            }

            string downloadStrategy = XPathUtility.Evaluate(xpath, "//Capabilities/DownloadStrategy", "");

            bool onlineCustomerSupported = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/OnlineCustomerID/@supported", false);
            string onlineCustomerDataType = XPathUtility.Evaluate(xpath, "//Capabilities/OnlineCustomerID/@dataType", "numeric");

            bool onlineStatusSupported = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/OnlineStatus/@supported", false);
            bool onlineStatusComments = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/OnlineStatus/@supportsComments", false);
            bool onlineStatusDownloadOnly = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/OnlineStatus/@downloadOnly", false);
            string onlineStatusDataType = XPathUtility.Evaluate(xpath, "//Capabilities/OnlineStatus/@dataType", "numeric");

            bool onlineShipmentSupported = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/OnlineShipmentUpdate/@supported", false);
            IncludeMilliseconds = XPathUtility.EvaluateXsdBoolean(xpath, "//Capabilities/IncludeMilliseconds/@supported", false);

            // Apply download strategy
            switch (downloadStrategy)
            {
                case "ByModifiedTime": DownloadStrategy = GenericStoreDownloadStrategy.ByModifiedTime; break;
                case "ByOrderNumber": DownloadStrategy = GenericStoreDownloadStrategy.ByOrderNumber; break;
                default: throw new GenericModuleConfigurationException(string.Format("Unhandled DownloadStrategy: '{0}'", downloadStrategy));
            }

            // Apply online customer support
            OnlineCustomerSupport = onlineCustomerSupported;
            OnlineCustomerDataType = GetVariantDataType(onlineCustomerSupported, onlineCustomerDataType);

            // Apply online shipment detail support
            OnlineShipmentDetails = onlineShipmentSupported;

            // Apply online status support
            GenericOnlineStatusSupport statusSupport = onlineStatusSupported ?
                (onlineStatusComments ? GenericOnlineStatusSupport.StatusWithComment : GenericOnlineStatusSupport.StatusOnly) :
                GenericOnlineStatusSupport.None;

            if (statusSupport != GenericOnlineStatusSupport.None && onlineStatusDownloadOnly)
            {
                statusSupport = GenericOnlineStatusSupport.DownloadOnly;
            }

            OnlineStatusSupport = statusSupport;
            OnlineStatusDataType = GetVariantDataType(statusSupport != GenericOnlineStatusSupport.None, onlineStatusDataType);
        }

        /// <summary>
        /// Convert the attribute text that was supplied in the XML to the strongly typed version to be save to the database
        /// </summary>
        private static GenericVariantDataType GetVariantDataType(bool supported, string dataType)
        {
            // If the feature isnt supported, just use numeric
            if (!supported)
            {
                return GenericVariantDataType.Numeric;
            }

            switch (dataType)
            {
                case "numeric": return GenericVariantDataType.Numeric;
                case "text": return GenericVariantDataType.Text;
                default: throw new GenericModuleConfigurationException(string.Format("Unhandled dataType: {0}", dataType));
            }
        }
    }
}
