using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// The possible Update Online Status options
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum EbayOnlineAction
    {
        Paid,
        Shipped,
        NotPaid,
        NotShipped
    }
}