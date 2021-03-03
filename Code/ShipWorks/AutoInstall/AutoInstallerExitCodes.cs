using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AutoInstall
{
    /// <summary>
    /// Enum of auto installer exit codes
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public enum AutoInstallerExitCodes
    {
        [Description("Unknown")]
        Unknown = -1,

        [Description("Success")]
        Success = 0,

        [Description("LocalDbInstallFailed")]
        LocalDbInstallFailed = 1,

        [Description("Install Failed")]
        InstallFailed = 2
    }
}
