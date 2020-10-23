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
    }
}
