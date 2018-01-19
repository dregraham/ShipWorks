using System.Reflection;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Used to get an IOnDemandDownloader via KeyedComponent
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OnDemandDownloaderType
    {
        OnDemandDownloader,

        SingleScanOnDemandDownloader
    }
}