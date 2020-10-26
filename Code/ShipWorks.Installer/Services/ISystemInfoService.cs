namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Interface for SystemInfoWrapper
    /// </summary>
    public interface ISystemInfoService
    {
        string GetCPUInfo();
        IDriveInfo[] GetDriveInfo();
        string GetOsDescription();
        string GetRamInfo();
    }
}