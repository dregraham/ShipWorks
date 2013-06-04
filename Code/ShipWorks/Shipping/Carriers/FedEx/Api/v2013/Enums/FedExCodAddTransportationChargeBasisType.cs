using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Enums
{
    /// <summary>
    /// Valid COD Charge Bases types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCodAddTransportationChargeBasisType
    {
        [Description("Cod Surcharge")]
        CodSurcharge = 0,

        [Description("Net Charge")]
        NetCharge = 1,

        [Description("Net Freight")]
        NetFreight = 2,

        [Description("Total Customer Charge")]
        TotalCustomerCharge = 3
    }
}
