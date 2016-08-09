using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Configuration option for how the Store downloads orders from the Odbc Source
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcImportStrategy
    {
        /// <summary>
        /// Download all orders from the ODBC data source.
        /// </summary>
        [Description("Download all orders from the ODBC data source.")]
        [ApiValue("All")]
        All = 0,

        /// <summary>
        /// Download orders from the ODBC data source based on last modified date of the order
        /// </summary>
        [ApiValue("ByModifiedTime")]
        [Description("Download orders from the ODBC data source based on last modified date of the order")]
        ByModifiedTime = 1
    }
}
