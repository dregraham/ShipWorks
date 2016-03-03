using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
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
        private const string userCapabilityNamespace = "http://ShipWorks.com/UserCapabilitiesV1.xsd";
        private const string userLevelNamespace = "http://ShipWorks.com/UserLevelsV1.xsd";

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

            // parse the ShipmentTypeFunctionality node from the response
            ShipmentTypeFunctionality(xmlResponse);

            // parse the license capabilities
            SetPricingPlanCapabilties(xmlResponse);

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

            while (shipmentTypeFunctionality.MoveNext())
            {
                XPathNavigator shipmentXpath = shipmentTypeFunctionality.Current;
                int shipmentTypeCode;

                if (int.TryParse(shipmentXpath.GetAttribute("TypeCode", ""), out shipmentTypeCode))
                {
                    List<ShipmentTypeRestrictionType> restrictionsList = new List<ShipmentTypeRestrictionType>();

                    XPathNodeIterator restrictions = shipmentXpath.Select("Restriction");
                    while (restrictions.MoveNext())
                    {
                        XPathNavigator restriction = restrictions.Current;

                        ShipmentTypeRestrictionType restrictionType;

                        if (Enum.TryParse(restriction.SelectSingleNode(".")?.Value, true, out restrictionType))
                        {
                            restrictionsList.Add(restrictionType);
                        }
                    }

                    ShipmentTypeRestriction.Add((ShipmentTypeCode)shipmentTypeCode, restrictionsList);


                    Dictionary<ShippingPolicyType, string> featureList = new Dictionary<ShippingPolicyType, string>();

                    XPathNodeIterator features = shipmentXpath.Select("Feature");
                    while (features.MoveNext())
                    {
                        XPathNavigator feature = features.Current;
                        
                        string type = feature.SelectSingleNode("Type")?.Value;
                        string value = feature.SelectSingleNode("Config")?.Value;

                        ShippingPolicyType policy;

                        if (Enum.TryParse(type, true, out policy))
                        {
                            featureList.Add(policy, value);
                        }
                    }

                    ShipmentTypeShippingPolicy.Add((ShipmentTypeCode)shipmentTypeCode, featureList);
                }
            }
        }

        /// <summary>
        /// Throw error if fault in XML.
        /// </summary>
        /// <summary>
        /// Checks the response for errors.
        /// </summary>
        private static void CheckResponseForErrors(XmlNode xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            string error = XPathUtility.Evaluate(xpath, "//Error", string.Empty);

            if (!error.Equals(string.Empty))
            {
                throw new ShipWorksLicenseException(error);
            }

            // Grab the nodes that are vital to shipworks functioning 
            string shipmentLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfChannels", xmlResponse, userCapabilityNamespace);
            string channelLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfShipments", xmlResponse, userCapabilityNamespace);
            string userShipmentLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfChannels", xmlResponse, userLevelNamespace);
            string userChannelLimitSanityCheck = GetStringValueFromNameValuePair("NumberOfShipments", xmlResponse, userLevelNamespace);
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
            UpsAccountNumbers = XPathUtility.Evaluate(xpath, "//UpsOnly", "").Split(';')
                                            .Where(a => !string.IsNullOrWhiteSpace(a))
                                            .Select(a => a.Trim().ToLower())
                                            .ToArray();

            UpsAccountLimit = UpsStatus == UpsStatus.Discount ? 
                1 : 
                UpsAccountNumbers.Count();

            UpsSurePost = XPathUtility.Evaluate(xpath, "//UpsSurePostEnabled/@status", 0) == 1;
            PostalAvailability = (BrownPostalAvailability)XPathUtility.Evaluate(xpath, "//UpsOnly/@postal", (int)BrownPostalAvailability.AllServices);
        }

        /// <summary>
        /// Set capabilities typically associated with a pricing plan
        /// </summary>
        private void SetPricingPlanCapabilties(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();

            IsInTrial = XPathUtility.Evaluate(xpath, "//IsInTrial", false);

            CustomDataSources = GetBoolValueFromNameValuePair("CustomDataSources", response, userCapabilityNamespace);

            // If custom data sources is disabled we add GenericFile to the list of stores that are not allowed
            if (!GetBoolValueFromNameValuePair("CustomDataSources", response, userCapabilityNamespace))
            {
                forbiddenChannels.Add(StoreTypeCode.GenericFile);
            }

            // If custom data sources api is disabled we add GenericModule to the list of stores that are not allowed
            if (!GetBoolValueFromNameValuePair("CustomDataSourcesAPI", response, userCapabilityNamespace))
            {
                forbiddenChannels.Add(StoreTypeCode.GenericModule);
            }

            ChannelLimit = GetIntValueFromNameValuePair("NumberOfChannels", response, userCapabilityNamespace);

            ShipmentLimit = GetIntValueFromNameValuePair("NumberOfShipments", response, userCapabilityNamespace);

            ActiveChannels = GetIntValueFromNameValuePair("NumberOfChannels", response, userLevelNamespace);
            ProcessedShipments = GetIntValueFromNameValuePair("NumberOfShipments", response, userLevelNamespace);

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
        /// Gets the string value for the given name 
        /// </summary>
        /// <remarks>returns empty string if the name or value are not found</remarks>
        private static string GetStringValueFromNameValuePair(string name, XmlNode document, string ns)
        {
            XPathNavigator xpath = document?.CreateNavigator();

            if (xpath?.NameTable != null)
            {
                XmlNamespaceManager nsmanager = new XmlNamespaceManager(xpath.NameTable);

                nsmanager.AddNamespace("x", ns);

                XPathNodeIterator iterator = xpath.Select("//x:NameValuePair", nsmanager);

                while (iterator.MoveNext())
                {
                    if (iterator.Current.NameTable != null)
                    {
                        if (iterator.Current?.SelectSingleNode("x:Name", nsmanager)?.Value == name)
                        {
                            return iterator.Current?.SelectSingleNode("x:Value", nsmanager)?.Value ?? string.Empty;
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <remarks>returns 0 if the name or value are not found or not an int</remarks>
        private static int GetIntValueFromNameValuePair(string name, XmlNode document, string ns)
        {
            string value = GetStringValueFromNameValuePair(name, document, ns);
            int result;

            return int.TryParse(value, out result) ? result : 0;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <remarks>returns 0 if the name or value are not found or not an int</remarks>
        private static bool GetBoolValueFromNameValuePair(string name, XmlNode document, string ns)
        {
            string value = GetStringValueFromNameValuePair(name, document, ns);

            return value.ToLower() == "yes";
        }
    }
}