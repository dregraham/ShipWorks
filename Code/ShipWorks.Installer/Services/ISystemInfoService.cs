namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Interface for SystemInfoWrapper
    /// </summary>
    public interface ISystemInfoService
    {
        /// <summary>
        /// Gets the max clock speed and number of cores in the cpu
        /// </summary>
        string GetCPUInfo();

        /// <summary>
        /// Gets drive info for all drives in the system
        /// </summary>
        IDriveInfo[] GetDriveInfo();

        /// <summary>
        /// Gets the description of the OS
        /// </summary>
        string GetOsDescription();

        /// <summary>
        /// Gets the amount of memory in the system
        /// </summary>
        string GetRamInfo();
    }
}