using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Configuration option for how the Store downloads orders from the Odbc Source
    /// </summary>
    [Obfuscation(Exclude = true)]
    public enum OdbcDownloadStrategy
    {
        /// <summary>
        /// Download all orders from the Odbc data source.
        /// </summary>
        [Description("Download all orders from the Odbc data source.")]
        All = 0,

        /// <summary>
        /// Download orders from the Odbc data source using the by last modified strategy.
        /// </summary>
        [Description("Download orders from the Odbc data source using the by last modified strategy.")]
        ByModifiedTime = 1
    }
}
