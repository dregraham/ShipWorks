using System.Reflection;

namespace ShipWorks.AutoInstall
{
    /// <summary>
    /// Enum of auto installer exit codes
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public enum AutoInstallerExitCodes
    {
        Unknown = -1,
        Success = 0,
        LocalDbInstallFailed = 1,
        InstallFailed = 2
    }
}
