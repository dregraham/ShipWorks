using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Configuration option for how the Store downloads orders from the Odbc Source
    /// </summary>
    [Obfuscation(Exclude = true)]
    public enum OdbcDownloadStrategy
    {
        all = 0,
        ByModifiedTime = 1
    }
}
