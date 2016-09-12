using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Configuration option for how the Store downloads order items from the Odbc Source
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]

    public enum OdbcImportOrderItemStrategy
    {
        /// <summary>
        /// Download all orders from the ODBC data source.
        /// </summary>
        [Description("Orders are on a single line.")]
        [ApiValue("SingleLine")]
        SingleLine = 0,

        /// <summary>
        /// Download orders from the ODBC data source based on last modified date of the order
        /// </summary>
        [ApiValue("MulitLine")]
        [Description("Each order item is on its own line.")]
        MultiLine = 1
    }
}
