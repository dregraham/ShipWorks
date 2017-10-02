using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// The possible Update Online Status options
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum EbayOnlineAction
    {
        [Description("Paid")]
        Paid,

        [Description("Shipped")]
        Shipped,

        [Description("Not Paid")]
        NotPaid,

        [Description("Not Shipped")]
        NotShipped
    }
}