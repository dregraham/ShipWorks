using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Enums
{
    /// <summary>
    /// Valid FedEx location type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExLocationType
    {
        [Description("FEDEX_EXPRESS_STATION")]
        FedExExpressStation = 0,

        [Description("FEDEX_FREIGHT_SERVICE_CENTER")]
        FedExFreightServiceCenter = 1,

        [Description("FEDEX_GROUND_TERMINAL")]
        FedExGroundTerminal = 2,

        [Description("FEDEX_HOME_DELIVERY_STATION")]
        FedExHomeDeliveryStation = 3,

        [Description("FEDEX_OFFICE")]
        FedExOffice = 4,

        [Description("FEDEX_SMART_POST_HUB")]
        FedExSmartPostHub = 5
    }
}
