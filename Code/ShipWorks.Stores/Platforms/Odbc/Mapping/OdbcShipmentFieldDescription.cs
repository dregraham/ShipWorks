using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcShipmentFieldDescription
    {
        [Description("Tracking Number")]
        TrackingNumber = 0,

        [Description("Ship Date")]
        ShipDate = 1,

        [Description("Provider")]
        Provider = 2,

        [Description("Total Weight")]
        TotalWeight = 3,

        [Description("Shipment Cost")]
        ShipmentCost = 4
    }
}