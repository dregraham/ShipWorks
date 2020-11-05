namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for interacting with the registry
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Get the last-used installation path if there is one, or the default path if not
        /// </summary>
        string GetInstallPath();

        /// <summary>
        /// Get the path to the INNO uninstaller
        /// </summary>
        string GetUninstallerPath();
    }
}