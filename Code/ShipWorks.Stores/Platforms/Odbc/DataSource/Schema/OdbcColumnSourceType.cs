using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    [Obfuscation(Exclude = true)]
    public enum OdbcColumnSourceType
    {
        [Description("The column source is a table.")]
        Table = 0,

        [Description("The column source is a custom query.")]
        CustomQuery = 1
    }
}