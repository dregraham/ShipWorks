using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Exposes license capabilities from tango XML.
    /// </summary>
    public class LicenseCapabilities : ILicenseCapabilities
    {
        private readonly List<StoreTypeCode> forbiddenChannels;
        private readonly XmlNode userLevelsNode;
        private readonly XmlNode capabilitiesNode;

        /// <summary>
        /// Constructor - Sets capabilities based on the xml response.
        /// </summary>
        public LicenseCapabilities(XmlDocument xmlResponse)
        {
            MethodConditions.EnsureArgumentIsNotNull(xmlResponse, nameof(xmlResponse));

            ShipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();
            ShipmentTypeShippingPolicy = new Dictionary<ShipmentTypeCode, Dictionary<ShippingPolicyType, string>>();
            forbiddenChannels = new List<StoreTypeCode>();
            
            // Check the response for errors and throw a ShipWorksLicenseException
            CheckResponseForErrors(xmlResponse);


            // Determine the capabilities node to use and extract the current user levels
            userLevelsNode = xmlResponse.SelectSingleNode("//UserLevels");
            capabilitiesNode = GetPricingCapabilitiesNode(xmlResponse);
            ValidateCapabilitiesAndUserLevels(xmlResponse);

            // parse the ShipmentTypeFunctionality node from the response
            ShipmentTypeFunctionality(xmlResponse);

            // parse the license capabilities
            SetPricingPlanCapabilities(xmlResponse);

            // parse the stamps specific capabilities
            SetStampsCapabilities(xmlResponse);

            // parse the endicia specific capabilities
            SetEndiciaCapabilities(xmlResponse);

            // parse the ups specific capabilities
            SetUpsCapabilities(xmlResponse);
        }

        #region Properties
        /// <summary>
        /// Controls if DHL is enabled for Endicia users
        /// </summary>
        public bool EndiciaDhl { get; set; }

        /// <summary>
        /// Controls if using Endicia insurance is enabled for Endicia users
        /// </summary>
        public bool EndiciaInsurance { get; set; }

        /// <summary>
        /// ShipmentType, can be forbidden or just restricted to upgrade
        /// </summary>
        public Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> ShipmentTypeRestriction { get; }

        /// <summary>
        /// ShipmentType, can be forbidden or just restricted to upgrade
        /// </summary>
        public Dictionary<ShipmentTypeCode, Dictionary<ShippingPolicyType, string>> ShipmentTypeShippingPolicy { get; }

        /// <summary>
        /// Restricted to a specific number of UPS accounts
        /// </summary>
        public int UpsAccountLimit { get; set; }

        /// <summary>
        ///  Restricted to a specific UPS account number
        ///  </summary>
        public IEnumerable<string> UpsAccountNumbers { get; set; }

        /// <summary>
        /// Restricted to using only postal APO\FPO\POBox services
        /// </summary>
        public BrownPostalAvailability PostalAvailability { get; set; }

        /// <summary>
        /// UPS SurePost service type can be restricted
        /// </summary>
        public bool UpsSurePost { get; set; }

        /// <summary>
        /// Gets or sets the ups status.
        /// </summary>
        public UpsStatus UpsStatus { get; set; }

        /// <summary>
        /// Endicia consolidator
        /// </summary>
        public bool EndiciaConsolidator { get; set; }

        /// <summary>
        /// Endicia Scan Based Returns can be Restricted
        /// </summary>
        public bool EndiciaScanBasedReturns { get; set; }

        /// <summary>
        /// Constrols if using Stamps insurance is enabled for Usps users
        /// </summary>
        public bool StampsInsurance { get; set; }

        /// <summary>
        /// Controls if DHL is enabled for Stamps users
        /// </summary>
        public bool StampsDhl { get; set; }

        /// <summary>
        /// Stamps Ascendia consolidator
        /// </summary>
        public bool StampsAscendiaConsolidator { get; set; }

        /// <summary>
        /// Stamps DHL consolidator
        /// </summary>
        public bool StampsDhlConsolidator { get; set; }

        /// <summary>
        /// Stamps Globegistics consolidator
        /// </summary>
        public bool StampsGlobegisticsConsolidator { get; set; }

        /// <summary>
        /// Stamps Ibc consolidator
        /// </summary>
        public bool StampsIbcConsolidator { get; set; }

        /// <summary>
        /// Stamps RrDonnelley consolidator
        /// </summary>
        public bool StampsRrDonnelleyConsolidator { get; set; }

        /// <summary>
        /// Custom data source restriction
        /// </summary>
        public bool CustomDataSources { get; set; }

        /// <summary>
        /// Number of selling channels the license allows
        /// </summary>
        public int ChannelLimit { get; set; }

        /// <summary>
        ///Number of shipments the license allows
        /// </summary>
        public int ShipmentLimit { get; set; }

        /// <summary>
        /// Gets or sets the billing end date.
        /// </summary>
        public DateTime BillingEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in trial.
        /// </summary>
        public bool IsInTrial { get; set; }

        /// <summary>
        /// The number of Active Channels in tango
        /// </summary>
        public int ActiveChannels { get; private set; }

        /// <summary>
        /// The number of processed shipments in tango
        /// </summary>
        public int ProcessedShipments { get; private set; }

        #endregion Properties

        /// <summary>
        /// Determines whether [is channel allowed] [the specified store type].
        /// </summary>
        public bool IsChannelAllowed(StoreTypeCode storeType)
        {
            return !forbiddenChannels.Contains(storeType);
        }

        /// <summary>
        /// Checks for ShipmentType Functionality restrictions
        /// </summary>
        private void ShipmentTypeFunctionality(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();
            XPathNodeIterator shipmentTypeFunctionality = xpath.Select("//ShipmentTypeFunctionality/ShipmentType");

            // Iterate over each ShipmentType in ShipmentTypeFunctionality
            while (shipmentTypeFunctionality.MoveNext())
            {
                XPathNavigator shipmentXpath = shipmentTypeFunctionality.Current;
                int shipmentTypeCode;

                if (int.TryParse(shipmentXpath.GetAttribute("TypeCode", ""), out shipmentTypeCode))
                {
                    // Empty list of restrictions for the shipmenttypecode we are on
                    List<ShipmentTypeRestrictionType> restrictionsList = new List<ShipmentTypeRestrictionType>();

                    XPathNodeIterator restrictions = shipmentXpath.Select("Restriction");
                    while (restrictions.MoveNext())
                    {
                        // Add the restriction to our list of restrictions for the carrier
                        AddRestrictionToList(restrictions.Current, restrictionsList);
                    }

                    ShipmentTypeRestriction.Add((ShipmentTypeCode)shipmentTypeCode, restrictionsList);

                    // Create an empty dictionary of ShippingPolicyType, string to keep track of features
                    // for the shipmenttypecode we are on
                    Dictionary<ShippingPolicyType, string> featureDictionary = new Dictionary<ShippingPolicyType, string>();

                    XPathNodeIterator features = shipmentXpath.Select("Feature");
                    while (features.MoveNext())
                    {
                        // Add the feature to our list of features
                        AddFeatureToDictionary(features.Current, featureDictionary);
                    }

                    ShipmentTypeShippingPolicy.Add((ShipmentTypeCode)shipmentTypeCode, featureDictionary);
                }
            }
        }

        /// <summary>
        /// Takes the given XPathNavigator feature and adds it to the featureDictionary
        /// </summary>
        private void AddFeatureToDictionary(XPathNavigator feature, Dictionary<ShippingPolicyType, string> featureDictionary)
        {
            try
            {
                string shippingPolicyNode = feature.SelectSingleNode("Type")?.Value;
                if (!string.IsNullOrWhiteSpace(shippingPolicyNode))
                {
                    ShippingPolicyType policy = EnumHelper.GetEnumByApiValue<ShippingPolicyType>(shippingPolicyNode);
                    featureDictionary.Add(policy, feature.SelectSingleNode("Config")?.Value);
                }
            }
            catch (InvalidOperationException)
            {
                // The value could not be found in the given enum
            }
        }

        /// <summary>
        /// Takes the give XPathNavigator restriction and adds it to the restrictionList
        /// </summary>
        private void AddRestrictionToList(XPathNavigator restriction, List<ShipmentTypeRestrictionType> restrictionsList)
        {
            try
            {
                string restrictionNode = restriction.SelectSingleNode(".")?.Value;
                if (!string.IsNullOrWhiteSpace(restrictionNode))
                {
                    // Try to get the enum value
                    ShipmentTypeRestrictionType restrictionType = EnumHelper.GetEnumByApiValue<ShipmentTypeRestrictionType>(restrictionNode);
                    restrictionsList.Add(restrictionType);
                }
            }
            catch (InvalidOperationException)
            {
                // The value could not be found in the given enum
            }
        }

        /// <summary>
        /// Throw error if fault in XML.
        /// </summary>
        /// <summary>
        /// Checks the response for errors.
        /// </summary>
        private void CheckResponseForErrors(XmlNode xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();
            string error = XPathUtility.Evaluate(xpath, "//Error", string.Empty);

            if (!error.Equals(string.Empty))
            {
                throw new ShipWorksLicenseException(error);
            }
        }

        /// <summary>
        /// Validates the capabilities and user levels from the XmlNode provided.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        /// <exception cref="ShipWorksLicenseException">The license associated with this account is invalid.</exception>
        private void ValidateCapabilitiesAndUserLevels(XmlNode xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            // Grab the nodes that are vital to ShipWorks functioning 
            string channelLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfChannels", capabilitiesNode);
            string shipmentLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfShipments", capabilitiesNode);


            string userChannelLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfChannels", userLevelsNode);
            string userShipmentLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfShipments", userLevelsNode);

            string customerStatus = XPathUtility.Evaluate(xpath, "//CustomerStatus/Valid", "");

            if (string.IsNullOrWhiteSpace(shipmentLimitSanityCheck) ||
                string.IsNullOrWhiteSpace(channelLimitSanityCheck) ||
                string.IsNullOrWhiteSpace(userShipmentLimitSanityCheck) ||
                string.IsNullOrWhiteSpace(userChannelLimitSanityCheck) ||
                string.IsNullOrWhiteSpace(customerStatus))
            {
                throw new ShipWorksLicenseException("The license associated with this account is invalid.");
            }
        }

        /// <summary>
        /// Set Ups Capabilities
        /// </summary>
        private void SetUpsCapabilities(XmlNode xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            UpsStatus = (UpsStatus)XPathUtility.Evaluate(xpath, "//UpsOnly/@status", 0);

            // Only subsidized accounts are limited to specific ups accounts
            if (UpsStatus == UpsStatus.Subsidized)
            {
                UpsAccountNumbers = XPathUtility.Evaluate(xpath, "//UpsOnly", "").Split(';')
                                .Where(a => !string.IsNullOrWhiteSpace(a))
                                .Select(a => a.Trim().ToLower())
                                .ToArray();
            }
            else
            {
                UpsAccountNumbers = Enumerable.Empty<string>();
            }

            UpsAccountLimit = UpsStatus == UpsStatus.Discount || UpsStatus == UpsStatus.Tier1 ?
                1 :
                UpsAccountNumbers.Count();

            UpsSurePost = XPathUtility.Evaluate(xpath, "//UpsSurePostEnabled/@status", 0) == 1;
            PostalAvailability = (BrownPostalAvailability)XPathUtility.Evaluate(xpath, "//UpsOnly/@postal", (int)BrownPostalAvailability.AllServices);
        }

        /// <summary>
        /// Extracts the appropriate pricing capabilities to use from the response.
        /// </summary>
        /// <param name="sourceNode">The source node.</param>
        /// <returns>The XmlNode containing the capabilities data.</returns>
        /// <exception cref="ShipWorksLicenseException">The license associated with this account is invalid.</exception>
        private XmlNode GetPricingCapabilitiesNode(XmlNode sourceNode)
        {
            // When changing plans in the middle of a billing cycle there will be two groups of capabilities 
            // that need to be inspected. When upgrading a plan, the increased capabilities should take effect 
            // immediately while capabilities of downgraded plans should not take effect until the next 
            // billing cycle (i.e. always choose the plan with the higher capabilities). The plan with the 
            // higher capabilities can be determined by shipment limit.

            try
            {
                const int unlimitedShipments = -1;
                XmlNode pricingCapabilitiesNode = sourceNode.SelectSingleNode("//UserCapabilities");
                int shipmentLimit = GetIntValueFromNameValuePair("NumberOfShipments", pricingCapabilitiesNode);

                // There's no need to inspect pending capabilities if the current plan offers unlimited shipments
                if (shipmentLimit != unlimitedShipments)
                {
                    // The shipment limit is not unlimited, so we need to check the pending capabilities
                    XmlNode pendingNode = sourceNode.SelectSingleNode("//PendingUserCapabilities");
                    if (pendingNode != null && pendingNode.HasChildNodes)
                    {
                        // The pending node is not empty, so the customer has changed plans.
                        int pendingShipmentLimit = GetIntValueFromNameValuePair("NumberOfShipments", pendingNode);
                        if (pendingShipmentLimit == unlimitedShipments || pendingShipmentLimit > shipmentLimit)
                        {
                            // The plan has been upgraded, so use the capabilities associated with the
                            // pending plan
                            pricingCapabilitiesNode = pendingNode;
                        }
                    }
                }

                return pricingCapabilitiesNode;
            }
            catch (Exception exception)
            {
                throw new ShipWorksLicenseException("The license associated with this account is invalid.", exception);
            }
        }

        /// <summary>
        /// Set capabilities typically associated with a pricing plan
        /// </summary>
        private void SetPricingPlanCapabilities(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();

            IsInTrial = XPathUtility.Evaluate(xpath, "//IsInTrial", false);
            
            CustomDataSources = GetBoolValueFromNameValuePair("CustomDataSources", capabilitiesNode);

            // Generic file channel capability
            if (!GetBoolValueFromNameValuePair("CustomDataSources", capabilitiesNode))
            {
                // Custom data sources is disabled. Add GenericFile to the list of stores that are not allowed.
                forbiddenChannels.Add(StoreTypeCode.GenericFile);
            }
            
            // Generic module channel capability
            if (!GetBoolValueFromNameValuePair("CustomDataSourcesAPI", capabilitiesNode))
            {
                // Custom data sources API is disabled. Add GenericModule to the list of stores that are not allowed.
                forbiddenChannels.Add(StoreTypeCode.GenericModule);
            }

            // Channel and shipment limits
            ChannelLimit = GetIntValueFromNameValuePair("NumberOfChannels", capabilitiesNode);
            ShipmentLimit = GetIntValueFromNameValuePair("NumberOfShipments", capabilitiesNode);

            // Get the current number of channels and shipments
            XmlNode userLevels = response.SelectSingleNode("//UserLevels");
            ActiveChannels = GetIntValueFromNameValuePair("NumberOfChannels", userLevels);
            ProcessedShipments = GetIntValueFromNameValuePair("NumberOfShipments", userLevels);

            // Grab the billing date
            string date = XPathUtility.Evaluate(xpath, "//BillingEndDate", DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
            BillingEndDate = DateTime.Parse(date);
        }

        /// <summary>
        /// Set Stamps Capabilities
        /// </summary>
        private void SetStampsCapabilities(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();

            StampsInsurance = XPathUtility.Evaluate(xpath, "//StampsInsuranceEnabled/@status", 0) == 1;
            StampsDhl = XPathUtility.Evaluate(xpath, "//StampsDhlEnabled/@status", 0) == 1;
            StampsAscendiaConsolidator = XPathUtility.Evaluate(xpath, "//StampsAscendiaEnabled/@status", 0) == 1;
            StampsDhlConsolidator = XPathUtility.Evaluate(xpath, "//StampsDhlConsolidatorEnabled/@status", 0) == 1;
            StampsGlobegisticsConsolidator = XPathUtility.Evaluate(xpath, "//StampsGlobegisticsEnabled/@status", 0) == 1;
            StampsIbcConsolidator = XPathUtility.Evaluate(xpath, "//StampsIbcEnabled/@status", 0) == 1;
            StampsRrDonnelleyConsolidator = XPathUtility.Evaluate(xpath, "//StampsRrDonnelleyEnabled/@status", 0) == 1;
        }

        /// <summary>
        /// Set Endicia Capabilities
        /// </summary>
        private void SetEndiciaCapabilities(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();

            EndiciaDhl = XPathUtility.Evaluate(xpath, "//EndiciaDhlEnabled/@status", 0) == 1;
            EndiciaInsurance = XPathUtility.Evaluate(xpath, "//EndiciaInsuranceEnabled/@status", 0) == 1;
            EndiciaConsolidator = XPathUtility.Evaluate(xpath, "//EndiciaConsolidator/@status", 0) == 1;
            EndiciaScanBasedReturns = XPathUtility.Evaluate(xpath, "//EndiciaScanBasedReturns/@status", 0) == 1;
        }

        /// <summary>
        /// Gets the string value for the given name from the XmlNode provided.
        /// </summary>
        /// <returns>Returns empty string if the name or value are not found.</returns>
        private static string GetStringValueFromNameValuePair(string name, XmlNode document)
        {
            XElement xDoc = XElement.Parse(document.OuterXml);
            IEnumerable<XElement> nameValuePairElements = xDoc.Descendants().Where(e => e.Name.LocalName == "NameValuePair");
            foreach (XElement element in nameValuePairElements)
            {
                if (element.Descendants().FirstOrDefault(d => d.Name.LocalName == "Name")?.Value == name)
                {
                    return element.Descendants().FirstOrDefault(d => d.Name.LocalName == "Value")?.Value ?? string.Empty;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <returns>returns 0 if the name or value are not found or not an int</returns>
        private static int GetIntValueFromNameValuePair(string name, XmlNode document)
        {
            string value = GetStringValueFromNameValuePair(name, document);
            int result;

            return int.TryParse(value, out result) ? result : 0;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <returns>returns false if the name or value are not found or not an int</returns>
        private static bool GetBoolValueFromNameValuePair(string name, XmlNode document)
        {
            string value = GetStringValueFromNameValuePair(name, document);

            return value.ToLower() == "yes";
        }
    }
}