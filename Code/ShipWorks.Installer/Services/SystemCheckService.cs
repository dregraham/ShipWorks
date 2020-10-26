using System;
using System.Linq;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for checking system requirements
    /// </summary>
    public class SystemCheckService : ISystemCheckService
    {
        private const int windows2012R2MinVersion = 9600;
        private const long bytesInGigaByte = 1024 * 1024 * 1024;
        private const long minRamInKb = 4194304;
        private const int minSpaceInGb = 20;
        private ISystemInfoService systemInfo;

        public SystemCheckService(ISystemInfoService systemInfo)
        {
            this.systemInfo = systemInfo;
        }

        /// <summary>
        /// Check the system requirements
        /// </summary>
        public SystemCheckResult CheckSystem()
        {
            SystemCheckResult result = new SystemCheckResult();

            try
            {
                CheckOS(result);
            }
            catch
            {
                result.OsMeetsRequirement = false;
                result.OsDescription = "Failed to read Operating System Version.";
            }

            try
            {
                CheckCPU(result);
            }
            catch
            {
                result.CpuMeetsRequirement = false;
                result.CpuDescription = "Failed to validate CPU speed and core count.";
            }

            try
            {
                CheckHDD(result);
            }
            catch
            {
                result.HddMeetsRequirement = false;
                result.HddDescription = "Failed to validate available disk space.";
            }

            try
            {
                CheckRAM(result);
            }
            catch
            {
                result.RamMeetsRequirement = false;
                result.RamDescription = "Failed to validate RAM size.";
            }

            return result;
        }

        /// <summary>
        /// Checks that the OS is a supported varsion
        /// </summary>
        /// <param name="result"></param>
        private void CheckOS(SystemCheckResult result)
        {
            var osDescParts = systemInfo.GetOsDescription().Split('.');

            if ((long.TryParse(osDescParts[2], out long windowsVersion) && windowsVersion < windows2012R2MinVersion))
            {
                result.OsMeetsRequirement = false;
                result.OsDescription = $"ShipWorks requires Windows 10 Version 1607 (and newer) or Windows Server 2012 R2 (and newer).";
            }
        }

        /// <summary>
        /// Checks that the CPU has sufficient clock speed and number of cores
        /// </summary>
        /// <param name="result"></param>
        private void CheckCPU(SystemCheckResult result)
        {
            var cpuLines = systemInfo.GetCPUInfo().Split("\n");
            if (int.TryParse(cpuLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1], out var cpuMaxSpeed) &&
                cpuMaxSpeed < 1500)
            {
                result.CpuMeetsRequirement = false;
                result.CpuDescription = $"ShipWorks requires at a minimum 1.5 GHz processor.";
            }

            if (result.CpuMeetsRequirement &&
                int.TryParse(cpuLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1], out var numberOfCores) &&
                numberOfCores < 2)
            {
                result.CpuMeetsRequirement = false;
                result.CpuDescription = $"ShipWorks requires at least 2 processor cores.";
            }
        }

        /// <summary>
        /// Checks that the HDD has sufficient available space
        /// </summary>
        /// <param name="result"></param>
        private void CheckHDD(SystemCheckResult result)
        {
            var drives = systemInfo.GetDriveInfo();
            result.HddMeetsRequirement = systemInfo.GetDriveInfo()
                .Any(d => (d.AvailableFreeSpace / bytesInGigaByte) > 20);

            if (!result.HddMeetsRequirement)
            {
                result.HddDescription = $"ShipWorks requires at least 20 GB of free storage.";
            }
        }

        /// <summary>
        /// Checks that the system has sufficient RAM
        /// </summary>
        /// <param name="result"></param>
        private void CheckRAM(SystemCheckResult result)
        {
            var memoryLines = systemInfo.GetRamInfo().Split("\n");
            var totalMemoryText = memoryLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];
            if (long.TryParse(totalMemoryText, out var totalMemory) && totalMemory < minRamInKb)
            {
                result.RamMeetsRequirement = false;
                result.RamDescription = $"ShipWorks requires at least 2 processor cores.";
            }
        }

        /// <summary>
        /// Checks a drive letter to see if it meets the minimum size requirements
        /// </summary>
        /// <param name="driveLetter"></param>
        public bool DriveMeetsRequirements(string driveLetter)
        {
            var freeSpace = systemInfo.GetDriveInfo().FirstOrDefault(d => d.Name.Equals(driveLetter, StringComparison.OrdinalIgnoreCase))?.AvailableFreeSpace;

            if (freeSpace == null)
            {
                return false;
            }

            return freeSpace / bytesInGigaByte > minSpaceInGb;
        }
    }
}
