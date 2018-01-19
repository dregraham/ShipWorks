using System.Reflection;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Used to get an IOnDemandDownloader via KeyedComponent
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OnDemandDownloaderType
    {
        OnDemandDownloader,

        SingleScanOnDemandDownloader
    }
}