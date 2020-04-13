using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Exclude = true)]
    public enum FedExMaskedDataType
    {
        [Description("Secondary Barcode")]
        SecondaryBarcode = 0
    }
}
