namespace ShipWorks.ApplicationCore.Licensing
{
    public class LicenseCapabilities
    {
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
    }
}
