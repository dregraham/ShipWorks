using System.ComponentModel;

namespace ShipWorks.Installer.Enums
{
    /// <summary>
    /// The type of error that occurred during installation
    /// </summary>
    public enum InstallError
    {
        None,

        [Description("Your system doesn't meet the minimum system requirements:")]
        SystemCheck,

        [Description("There was a problem downloading the ShipWorks installer.")]
        DownloadingShipWorks,

        [Description("There was a problem installing ShipWorks.")]
        InstallShipWorks,

        [Description("There was a problem installing or connecting to your database.")]
        Database,

        [Description("An unknown error occurred.")]
        Unknown
    }
}
