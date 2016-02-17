using System;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface ILicenseCapabilities
    {
        /// <summary>
        /// No specific feature
        /// </summary>
        bool None { get; set; }

        /// <summary>
        /// Action count limitation
        /// </summary>
        bool ActionLimit { get; set; }

        /// <summary>
        /// Filter count limitation
        /// </summary>
        bool FilterLimit { get; set; }

        /// <summary>
        /// Can't create 'My' (private) filters when filters are being limited
        /// </summary>
        bool MyFilters { get; set; }

        /// <summary>
        /// Selection count limitation
        /// </summary>
        bool SelectionLimit { get; set; }

        /// <summary>
        /// Can't add new orders\customers
        /// </summary>
        bool AddOrderCustomer { get; set; }

        /// <summary>
        /// Create \ prbool Endicia scan forms
        /// </summary>
        bool EndiciaScanForm { get; set; }

        /// <summary>
        /// Restricted to a specific number of Endicia accounts
        /// </summary>
        bool EndiciaAccountLimit { get; set; }

        /// <summary>
        /// Restricted to a specific Endicia account number
        /// </summary>
        bool EndiciaAccountNumber { get; set; }

        /// <summary>
        /// Controls if DHL is enabled for Endicia users
        /// </summary>
        bool EndiciaDhl { get; set; }

        /// <summary>
        /// Constrols if using Endicia insurance is enabled for Endicia users
        /// </summary>
        bool EndiciaInsurance { get; set; }

        /// <summary>
        /// ShipmentType, can be forbidden or just restricted to upgrade
        /// </summary>
        bool ShipmentType { get; set; }

        /// <summary>
        /// Restricted to a single store
        /// </summary>
        bool SingleStore { get; set; }

        /// <summary>
        /// Restricted to a specific number of UPS accounts
        /// </summary>
        bool UpsAccountLimit { get; set; }

        /// <summary>
        ///  Restricted to a specific UPS account number
        ///  </summary>
        bool UpsAccountNumbers { get; set; }

        /// <summary>
        /// Restricted to using only postal APO\FPO\POBox services
        /// </summary>
        bool PostalApoFpoPoboxOnly { get; set; }

        /// <summary>
        /// UPS SurePost service type can be restricted
        /// </summary>
        bool UpsSurePost { get; set; }

        /// <summary>
        /// Endicia consolidator
        /// </summary>
        bool EndiciaConsolidator { get; set; }

        /// <summary>
        /// Endicia Scan Based Returns can be Restricted
        /// </summary>
        bool EndiciaScanBasedReturns { get; set; }

        /// <summary>
        /// The ability to add shipping accounts can be restricted.
        /// </summary>
        bool ShipmentTypeRegistration { get; set; }

        /// <summary>
        /// The ability to process shipments for specific carriers can be restricted.
        /// </summary>
        bool ProcessShipment { get; set; }

        /// <summary>
        /// The ability to purchase postage for specific carriers can be restricted.
        /// </summary>
        bool PurchasePostage { get; set; }

        /// <summary>
        /// The ability to display discount messaging for specific carriers can be restricted.
        /// </summary>
        bool RateDiscountMessaging { get; set; }

        /// <summary>
        /// The ability to display a conversion promo/message for a shipping provider can be restricted.
        /// This is sort of out of place and pertains only to USPS. This is a result of a problem
        /// on the USPS side when USPS customers have multi-user accounts where they don't
        /// want to allow these customers to convert through ShipWorks. After USPS has reached
        /// out to these customers and converted their accounts this can be removed.
        /// </summary>
        bool ShippingAccountConversion { get; set; }

        /// <summary>
        /// Constrols if using Stamps insurance is enabled for Usps users
        /// </summary>
        bool StampsInsurance { get; set; }

        /// <summary>
        /// Controls if DHL is enabled for Stamps users
        /// </summary>
        bool StampsDhl { get; set; }

        /// <summary>
        /// Stamps Ascendia consolidator
        /// </summary>
        bool StampsAscendiaConsolidator { get; set; }

        /// <summary>
        /// Stamps DHL consolidator
        /// </summary>
        bool StampsDhlConsolidator { get; set; }

        /// <summary>
        /// Stamps Globegistics consolidator
        /// </summary>
        bool StampsGlobegisticsConsolidator { get; set; }

        /// <summary>
        /// Stamps Ibc consolidator
        /// </summary>
        bool StampsIbcConsolidator { get; set; }

        /// <summary>
        /// Stamps RrDonnelley consolidator
        /// </summary>
        bool StampsRrDonnelleyConsolidator { get; set; }

        /// <summary>
        /// Advanced shipping features restriction
        /// </summary>
        bool AdvancedShipping { get; set; }

        /// <summary>
        /// CRM features restriction
        /// </summary>
        bool Crm { get; set; }

        /// <summary>
        /// Custom data source restriction
        /// </summary>
        bool CustomDataSources { get; set; }

        /// <summary>
        /// Template customization restriction
        /// </summary>
        bool TemplateCustomization { get; set; }

        /// <summary>
        /// Number of selling channels the license allows
        /// </summary>
        int ChannelLimit { get; set; }

        /// <summary>
        ///Number of shipments the license allows
        /// </summary>
        int ShipmentLimit { get; set; }

        /// <summary>
        /// Gets or sets the billing end date.
        /// </summary>
        DateTime BillingEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in trial.
        /// </summary>
        bool IsInTrial { get; set; }

        /// <summary>
        /// The number of Active Channels in tango
        /// </summary>
        int ActiveChannels { get; }

        /// <summary>
        /// The number of processed shipments in tango
        /// </summary>
        int ProcessedShipments { get; }

        /// <summary>
        /// Determines whether [is channel allowed] [the specified store type].
        /// </summary>
        bool IsChannelAllowed(StoreTypeCode storeType);
    }
}