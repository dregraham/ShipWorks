using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Type of Column Source
    /// </summary>
    [Obfuscation(Exclude = true)]
    public enum OdbcColumnSourceType
    {
        [ApiValue("Table")]
        [Description("The column source is a table.")]
        Table = 0,

        [ApiValue("CustomQuery")]
        [Description("The column source is a custom query.")]
        CustomQuery = 1
    }
}