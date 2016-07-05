using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Configuration option for how the Store downloads orders from the Odbc Source
    /// </summary>
    [Obfuscation(Exclude = true)]
    public enum OdbcDownloadStrategy
    {
        /// <summary>
        /// Download all orders from the ODBC data source.
        /// </summary>
        [Description("Download all orders from the ODBC data source.")]
        All = 0,

        /// <summary>
        /// Download orders from the ODBC data source based on last modified date of the order
        /// </summary>
        [Description("Download orders from the ODBC data source based on last modified date of the order")]
        ByModifiedTime = 1
    }
}
