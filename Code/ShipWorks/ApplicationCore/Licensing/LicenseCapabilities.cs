using System;
using System.Xml;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class LicenseCapabilities
    {
        public LicenseCapabilities(XmlDocument xmlResponse)
        {
            MethodConditions.EnsureArgumentIsNotNull(xmlResponse, nameof(xmlResponse));

            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlResponse);
            xpath.Namespaces.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            xpath.Namespaces.AddNamespace("a", "http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.CustomerLicenseInfo");
            xpath.Namespaces.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
            xpath.Namespaces.AddNamespace("", "http://stamps.com/xml/namespace/2015/09/shipworks/activationv1");

            None = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='None']/Value", 0) == 1;
            ActionLimit = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='ActionLimit']/Value", 0) == 1;
            FilterLimit = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='FilterLimit']/Value", 0) == 1;
            MyFilters = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='MyFilters']/Value", 0) == 1;
            SelectionLimit = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='SelectionLimit']/Value", 0) == 1;
            AddOrderCustomer = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='AddOrderCustomer']/Value", 0) == 1;
            EndiciaScanForm = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaScanForm']/Value", 0) == 1;
            EndiciaAccountLimit = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaAccountLimit']/Value", 0) == 1;
            EndiciaAccountNumber = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaAccountNumber']/Value", 0) == 1;
            EndiciaDhl = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaDhl']/Value", 0) == 1;
            EndiciaInsurance = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaInsurance']/Value", 0) == 1;
            ShipmentType = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='ShipmentType']/Value", 0) == 1;
            SingleStore = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='SingleStore']/Value", 0) == 1;
            UpsAccountLimit = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='UpsAccountLimit']/Value", 0) == 1;
            UpsAccountNumbers = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='UpsAccountNumbers']/Value", 0) == 1;
            PostalApoFpoPoboxOnly = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='PostalApoFpoPoboxOnly']/Value", 0) == 1;
            UpsSurePost = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='UpsSurePost']/Value", 0) == 1;
            EndiciaConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaConsolidator']/Value", 0) == 1;
            EndiciaScanBasedReturns = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='EndiciaScanBasedReturns']/Value", 0) == 1;
            ShipmentTypeRegistration = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='ShipmentTypeRegistration']/Value", 0) == 1;
            ProcessShipment = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='ProcessShipment']/Value", 0) == 1;
            PurchasePostage = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='PurchasePostage']/Value", 0) == 1;
            RateDiscountMessaging = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='RateDiscountMessaging']/Value", 0) == 1;
            ShippingAccountConversion = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='ShippingAccountConversion']/Value", 0) == 1;
            StampsInsurance = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsInsurance']/Value", 0) == 1;
            StampsDhl = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsDhl']/Value", 0) == 1;
            StampsAscendiaConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsAscendiaConsolidator']/Value", 0) == 1;
            StampsDhlConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsDhlConsolidator']/Value", 0) == 1;
            StampsGlobegisticsConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsGlobegisticsConsolidator']/Value", 0) == 1;
            StampsIbcConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsIbcConsolidator']/Value", 0) == 1;
            StampsRrDonnelleyConsolidator = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='StampsRrDonnelleyConsolidator']/Value", 0) == 1;
            AdvancedShipping = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='AdvancedShippingFeatures']/Value", string.Empty) == "Yes";
            Crm = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='Crm']/Value", string.Empty) == "Yes";
            CustomDataSources = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='CustomDataSources']/Value", string.Empty) == "Yes";
            TemplateCustomization = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='TemplateCustomization']/Value", string.Empty) == "Yes";
            NumberOfChannels = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='NumberOfChannels']/Value", 0);
            NumberOfShipments = XPathUtility.Evaluate(xpath, "//NameValuePair[Name ='NumberOfShipments']/Value", 0);
        }

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
        public bool ShipmentType { get; set; }

        /// <summary>
        /// Restricted to a single store
        /// </summary>
        public bool SingleStore { get; set; }

        /// <summary>
        /// Restricted to a specific number of UPS accounts
        /// </summary>
        public bool UpsAccountLimit { get; set; }

        /// <summary>
        ///  Restricted to a specific UPS account number
        ///  </summary>
        public bool UpsAccountNumbers { get; set; }

        /// <summary>
        /// Restricted to using only postal APO\FPO\POBox services
        /// </summary>
        public bool PostalApoFpoPoboxOnly { get; set; }

        /// <summary>
        /// UPS SurePost service type can be restricted
        /// </summary>
        public bool UpsSurePost { get; set; }

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
        public int NumberOfChannels { get; set; }

        /// <summary>
        ///Number of shipments the license allows
        /// </summary>
        public int NumberOfShipments { get; set; }

        /// <summary>
        /// Gets or sets the billing end date.
        /// </summary>
        public DateTime BillingEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in trial.
        /// </summary>
        public bool IsInTrial { get; set; }
    }
}
