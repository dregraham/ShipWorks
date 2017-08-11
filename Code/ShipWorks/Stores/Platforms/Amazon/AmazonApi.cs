using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// The Amazon API platform to use when retrieving and updating orders
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonApi
    {
        MarketplaceWebService
    }
}
