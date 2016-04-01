using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Reason we are currently prompting a user to upgrade
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EditionFeature
    {
        /// <summary>
        /// No specific feature
        /// </summary>
        [Description("Not restricted.")]
        None,

        /// <summary>
        /// Action count limitation
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to have more than {0} actions.")]
        ActionLimit,

        /// <summary>
        /// Filter count limitation
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to have more than {0} filters.")]
        FilterLimit,

        /// <summary>
        /// Can't create 'My' (private) filters when filters are being limited
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to use 'My Filters'.")]
        MyFilters,

        /// <summary>
        /// Selection count limitation
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to select more than {0} items.")]
        SelectionLimit,

        /// <summary>
        /// Can't add new orders\customers
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to add new orders and customers.")]
        AddOrderCustomer,

        /// <summary>
        /// Create \ print Endicia scan forms
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to use Endicia SCAN forms.")]
        EndiciaScanForm,

        /// <summary>
        /// Restricted to a specific number of Endicia accounts
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to use additional Endicia accounts.")]
        EndiciaAccountLimit,

        /// <summary>
        /// Restricted to a specific Endicia account number
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to enable use of Endicia account '{1}'.")]
        EndiciaAccountNumber,

        /// <summary>
        /// Controls if DHL is enabled for Endicia users
        /// </summary>
        [Description("Your ShipWorks account does not support shipping with DHL through Endicia.")]
        EndiciaDhl,

        /// <summary>
        /// Controls if using Endicia insurance is enabled for Endicia users
        /// </summary>
        [Description("Your ShipWorks account does not support using Endicia insurance.")]
        EndiciaInsurance,

        /// <summary>
        /// ShipmentType, can be forbidden or just restricted to upgrade
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to use {0}.|You must contact Interapptive to use additional shipping carriers.")]
        ShipmentType,

        /// <summary>
        /// Restricted to a single store
        /// </summary>
        [Description("You must upgrade your ShipWorks edition to add additional stores.")]
        SingleStore,

        /// <summary>
        /// Restricted to a specific number of UPS accounts
        /// </summary>
        [Description("You must contact Interapptive to use additional UPS accounts.")]
        UpsAccountLimit,

        /// <summary>
        /// Restricted to a specific UPS account number
        /// </summary>
        [Description("You must contact Interapptive to enable use of UPS account '{1}'.")]
        UpsAccountNumbers,

        /// <summary>
        /// Restricted to using only postal APO\FPO\POBox services
        /// </summary>
        [Description("Your ShipWorks account is only enabled for using APO, FPO, and P.O. Box postal services.  Please contact Interapptive to enable use of all postal services.")]
        PostalApoFpoPoboxOnly,

        /// <summary>
        /// UPS SurePost service type can be restricted
        /// </summary>
        [Description("Your ShipWorks account does not support using the UPS SurePost service.")]
        UpsSurePost,

        /// <summary>
        /// Endicia consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using consolidators through Endicia.")]
        EndiciaConsolidator,

        /// <summary>
        /// Endicia Scan Based Returns can be Restricted
        /// </summary>
        [Description("You must contact Interapptive to use Endicia Scan Based Returns.")]
        EndiciaScanBasedReturns,

        /// <summary>
        /// The ability to add shipping accounts can be restricted.
        /// </summary>
        [Description("You must contact Interapptive to be able to add a new account to ShipWorks.")]
        ShipmentTypeRegistration,

        /// <summary>
        /// The ability to process shipments for specific carriers can be restricted.
        /// </summary>
        [Description("Processing shipments using the selected carrier is no longer supported.")]
        ProcessShipment,

        /// <summary>
        /// The ability to purchase postage for specific carriers can be restricted.
        /// </summary>
        [Description("Purchasing postage using the selected carrier is no longer supported.")]
        PurchasePostage,

        /// <summary>
        /// The ability to display discount messaging for specific carriers can be restricted.
        /// </summary>
        [Description("Discount messaging using the selected carrier is disabled.")]
        RateDiscountMessaging,

        /// <summary>
        /// The ability to display a conversion promo/message for a shipping provider can be restricted.
        /// This is sort of out of place and pertains only to USPS. This is a result of a problem
        /// on the USPS side when USPS customers have multi-user accounts where they don't
        /// want to allow these customers to convert through ShipWorks. After USPS has reached
        /// out to these customers and converted their accounts, this can be removed.
        /// </summary>
        [Description("Converting an account using the selected carrier is disabled.")]
        ShippingAccountConversion,

        /// <summary>
        /// Constrols if using Stamps insurance is enabled for Usps users
        /// </summary>
        [Description("Your ShipWorks account does not support using Stamps insurance.")]
        StampsInsurance,

        /// <summary>
        /// Controls if DHL is enabled for Stamps users
        /// </summary>
        [Description("Your ShipWorks account does not support shipping with DHL through Stamps.")]
        StampsDhl,

        /// <summary>
        /// Stamps Ascendia consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using Ascendia as a consolidator through Stamps.")]
        StampsAscendiaConsolidator,

        /// <summary>
        /// Stamps DHL consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using DHL as a consolidator through Stamps.")]
        StampsDhlConsolidator,

        /// <summary>
        /// Stamps Globegistics consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using Globegistics as a consolidator through Stamps.")]
        StampsGlobegisticsConsolidator,

        /// <summary>
        /// Stamps Ibc consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using IBC as a consolidator through Stamps.")]
        StampsIbcConsolidator,

        /// <summary>
        /// Stamps RrDonnelley consolidator
        /// </summary>
        [Description("Your ShipWorks account does not support using RR Donnelley as a consolidator through Stamps.")]
        StampsRrDonnelleyConsolidator,

        /// <summary>
        /// The channel count
        /// </summary>
        [Description("Your ShipWorks account does not support the amount of installed channels")]
        ChannelCount,

        /// <summary>
        /// The shipment count
        /// </summary>
        [Description("You have reached your Shipment Limit under your current plan. Please upgrade.")]
        ShipmentCount,

        /// <summary>
        /// The generic module
        /// </summary>
        [Description("Your Shipworks account does not support a Generic Module")]
        GenericModule,

        /// <summary>
        /// The generic file
        /// </summary>
        [Description("Your ShipWorks account does not support a Generic File")]
        GenericFile,

        /// <summary>
        /// All the stores in Shipworks match a store in Tango.
        /// </summary>
        [Description("You must upgrade ShipWorks or delete these channel(s) to use Shipworks.")]
        ClientChannelsAccountedFor
    }
}
