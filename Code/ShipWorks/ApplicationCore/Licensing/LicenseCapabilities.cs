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
        /// No specific feature
        /// </summary>
        public bool None { get; set; }

        /// <summary>
        /// Action count limitation
        /// </summary>
        public bool ActionLimit { get; set; }

        /// <summary>
        /// Filter count limitation
        /// </summary>
        public bool FilterLimit { get; set; }

        /// <summary>
        /// Can't create 'My' (private) filters when filters are being limited
        /// </summary>
        public bool MyFilters { get; set; }

        /// <summary>
        /// Selection count limitation
        /// </summary>
        public bool SelectionLimit { get; set; }

        /// <summary>
        /// Can't add new orders\customers
        /// </summary>
        public bool AddOrderCustomer { get; set; }

        /// <summary>
        /// Create \ prbool Endicia scan forms
        /// </summary>
        public bool EndiciaScanForm { get; set; }

        /// <summary>
        /// Restricted to a specific number of Endicia accounts
        /// </summary>
        public bool EndiciaAccountLimit { get; set; }

        /// <summary>
        /// Restricted to a specific Endicia account number
        /// </summary>
        public bool EndiciaAccountNumber { get; set; }

        /// <summary>
        /// Controls if DHL is enabled for Endicia users
        /// </summary>
        public bool EndiciaDhl { get; set; }

        /// <summary>
        /// Constrols if using Endicia insurance is enabled for Endicia users
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
        /// Restricted to a single store
        /// </summary>
        public bool SingleStore { get; set; }

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
        /// The ability to add shipping accounts can be restricted.
        /// </summary>
        public bool ShipmentTypeRegistration { get; set; }

        /// <summary>
        /// The ability to process shipments for specific carriers can be restricted.
        /// </summary>
        public bool ProcessShipment { get; set; }

        /// <summary>
        /// The ability to purchase postage for specific carriers can be restricted.
        /// </summary>
        public bool PurchasePostage { get; set; }

        /// <summary>
        /// The ability to display discount messaging for specific carriers can be restricted.
        /// </summary>
        public bool RateDiscountMessaging { get; set; }

        /// <summary>
        /// The ability to display a conversion promo/message for a shipping provider can be restricted.
        /// This is sort of out of place and pertains only to USPS. This is a result of a problem
        /// on the USPS side when USPS customers have multi-user accounts where they don't
        /// want to allow these customers to convert through ShipWorks. After USPS has reached
        /// out to these customers and converted their accounts this can be removed.
        /// </summary>
        public bool ShippingAccountConversion { get; set; }

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
        /// Advanced shipping features restriction
        /// </summary>
        public bool AdvancedShipping { get; set; }

        /// <summary>
        /// CRM features restriction
        /// </summary>
        public bool Crm { get; set; }

        /// <summary>
        /// Custom data source restriction
        /// </summary>
        public bool CustomDataSources { get; set; }

        /// <summary>
        /// Template customization restriction
        /// </summary>
        public bool TemplateCustomization { get; set; }

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

                if (Int32.TryParse(shipmentXpath.GetAttribute("TypeCode", ""), out shipmentTypeCode))
                {
                    List<ShipmentTypeRestrictionType> restrictionsList = new List<ShipmentTypeRestrictionType>();

                    XPathNodeIterator restrictions = shipmentXpath.Select("Restriction");
                    while (restrictions.MoveNext())
                    {
                        XPathNavigator restriction = restrictions.Current;
                        restrictionsList.Add(EnumHelper.GetEnumByApiValue<ShipmentTypeRestrictionType>(restriction.SelectSingleNode(".")?.Value));
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
        }

        /// <summary>
        /// Set Ups Capabilities
        /// </summary>
        private void SetUpsCapabilities(XmlNode xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            UpsStatus = (UpsStatus)XPathUtility.Evaluate(xpath, "//UpsOnly/@status", 0);
            UpsAccountNumbers = XPathUtility.Evaluate(xpath, "//UpsOnly", "").Split(';')
                                            .Select(a => a.Trim().ToLower())
                                            .ToArray();

            UpsAccountLimit = UpsStatus == UpsStatus.Discount ? 
                1 : 
                UpsAccountNumbers.Count();

            UpsSurePost = XPathUtility.Evaluate(xpath, "//UpsSurePostEnabled/@status", 0) == 1;
            PostalAvailability = (BrownPostalAvailability)XPathUtility.Evaluate(xpath, "//UpsOnly/@postal", (int)BrownPostalAvailability.ApoFpoPobox);
        }

        /// <summary>
        /// Set capabilities typically associated with a pricing plan
        /// </summary>
        private void SetPricingPlanCapabilties(XmlNode response)
        {
            XPathNavigator xpath = response.CreateNavigator();

            IsInTrial = XPathUtility.Evaluate(xpath, "//IsInTrial", false);

            CustomDataSources = GetBoolValueFromNameValuePair("CustomDataSources", GetUserCapabilities(response));

            // If custom data sources is disabled we add GenericFile to the list of stores that are not allowed
            if (!GetBoolValueFromNameValuePair("CustomDataSources", GetUserCapabilities(response)))
            {
                forbiddenChannels.Add(StoreTypeCode.GenericFile);
            }

            // If custom data sources api is disabled we add GenericModule to the list of stores that are not allowed
            if (!GetBoolValueFromNameValuePair("CustomDataSourcesAPI", GetUserCapabilities(response)))
            {
                forbiddenChannels.Add(StoreTypeCode.GenericModule);
            }

            ChannelLimit = GetIntValueFromNameValuePair("NumberOfChannels", GetUserCapabilities(response));

            ShipmentLimit = GetIntValueFromNameValuePair("NumberOfShipments", GetUserCapabilities(response));

            ActiveChannels = GetIntValueFromNameValuePair("NumberOfShipments", GetUserLevels(response));
            ProcessedShipments = GetIntValueFromNameValuePair("NumberOfShipments", GetUserLevels(response));

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
        private static string GetStringValueFromNameValuePair(string name, XmlNode document)
        {
            // Find the node with the given name 
            XmlNode namedNode = document.SelectSingleNode($"//*[local-name()='{name}']");

            // Grab its parent and then find the value node inside of the parent
            XmlNode parentNode = namedNode?.ParentNode?.SelectSingleNode("//*[local-name()='Value']");

            // return the value
            return parentNode?.InnerText ?? string.Empty;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <remarks>returns 0 if the name or value are not found or not an int</remarks>
        private static int GetIntValueFromNameValuePair(string name, XmlNode document)
        {
            string value = GetStringValueFromNameValuePair(name, document);
            int result;

            return Int32.TryParse(value, out result) ? result : 0;
        }

        /// <summary>
        /// Gets the int value for the given name
        /// </summary>
        /// <remarks>returns 0 if the name or value are not found or not an int</remarks>
        private static bool GetBoolValueFromNameValuePair(string name, XmlNode document)
        {
            string value = GetStringValueFromNameValuePair(name, document);

            return value.ToLower() == "yes";
        }

        /// <summary>
        /// Returns the UserCapabilities node from the response
        /// </summary>
        private static XmlNode GetUserCapabilities(XmlNode response)
        {
            return response.SelectSingleNode("//*[local-name()='UserCapabilities']");
        }

        /// <summary>
        /// Returns the UserLevels node from the response
        /// </summary>
        private static XmlNode GetUserLevels(XmlNode response)
        {
            return response.SelectSingleNode("//*[local-name()='UserLevels']");
        }
    }
}
