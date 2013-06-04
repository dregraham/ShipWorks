using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Enums
{
    /// <summary>
    /// Valid FedEx drop off type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExDropoffType
    {
        [Description("Business Service Center")]
        BusinessServiceCenter = 0,

        [Description("Drop Box")]
        DropBox = 1,

        [Description("Regular Pickup")]
        RegularPickup = 2,

        [Description("Request Courier")]
        RequestCourier = 3,

        [Description("Station")]
        Station = 4
    }
}
