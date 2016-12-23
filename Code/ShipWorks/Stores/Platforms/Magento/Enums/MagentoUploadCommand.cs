using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Magento.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum MagentoUploadCommand
    {
        [Description("")]
        None = -1,

        [Description("With Comments...")]
        Comments = 0,

        [Description("Complete")]
        Complete = 1,

        [Description("Cancel")]
        Cancel = 2,

        [Description("Hold")]
        Hold = 3,

        [Description("Unhold")]
        Unhold = 4
    }
}