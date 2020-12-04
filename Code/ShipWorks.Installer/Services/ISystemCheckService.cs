using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for checking system requirements
    /// </summary>
    public interface ISystemCheckService
    {
        /// <summary>
        /// Check the system requirements
        /// </summary>
        SystemCheckResult CheckSystem();

        /// <summary>
        /// Checks a drive letter to see if it meets the minimum size requirements
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        bool DriveMeetsRequirements(string driveLetter);
    }
}
