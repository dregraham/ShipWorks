using System.ComponentModel;

namespace ShipWorks.Installer.Enums
{
    /// <summary>
    /// The type of error that occurred during installation
    /// </summary>
    public enum InstallError
    {
        None,
        SystemCheck,

        [Description("There was a problem installing ShipWorks.")]
        ShipWorks,

        [Description("There was a problem installing or connecting to your database.")]
        Database,

        [Description("An unknown error occurred.")]
        Unknown
    }
}
