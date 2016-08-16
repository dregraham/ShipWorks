using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Magento.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum MagentoVersion
    {
        [Description("PHP File")]
        PhpFile = 0,

        [Description("Magento Version One using MagentoConnect")]
        MagentoConnect = 1,

        [Description("Magento Version Two")]
        MagentoTwo = 2,
    }
}