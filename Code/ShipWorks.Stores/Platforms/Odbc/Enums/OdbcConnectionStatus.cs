using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Odbc.Enums
{
    /// <summary>
    /// The status of the Odbc Connection
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcConnectionStatus
    {
        [Description("Successfully connected to the Odbc data source.")]
        Success = 0,

        [Description("The credentials provided are not valid for the Odbc data source.")]
        InvalidCredentials = 1,

        [Description("The Odbc data source could not be found.")]
        DataSourceNotFound = 2,

        [Description("The connection string is not valid.")]
        InvalidConnectionString = 3,

        [Description("A connection to the Odbc data source could not be established.")]
        Failure = 4
    }
}
