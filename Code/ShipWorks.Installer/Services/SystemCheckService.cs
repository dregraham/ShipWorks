using System;
using System.Linq;
using log4net;
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
        private const long kbInGigaByte = 1024 * 1024;
        private const long minRamInKb = 4194304;
        private const int minSpaceInGb = 20;
        private const int minCpuSpeed = 1500;
        private const int minCpuCores = 2;
        private ISystemInfoService systemInfo;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public SystemCheckService(ISystemInfoService systemInfo, Func<Type, ILog> logFactory)
        {
            this.systemInfo = systemInfo;
            log = logFactory(typeof(SystemCheckService));
        }

        /// <summary>
        /// Check the system requirements
        /// </summary>
        public SystemCheckResult CheckSystem()
        {
            log.Info("Beginning system check");

            SystemCheckResult result = new SystemCheckResult();

            try
            {
                CheckOS(result);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get Windows version", ex);
                result.OsMeetsRequirement = false;
                result.OsDescription = "Failed to read Operating System Version.";
            }

            try
            {
                CheckCPU(result);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get CPU info", ex);
                result.CpuMeetsRequirement = false;
                result.CpuDescription = "Failed to validate CPU speed and core count.";
            }

            try
            {
                CheckHDD(result);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get HDD info", ex);
                result.HddMeetsRequirement = false;
                result.HddDescription = "Failed to validate available disk space.";
            }

            try
            {
                CheckRAM(result);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get RAM info", ex);
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
            log.Info("Checking Windows version");

            var osDescription = systemInfo.GetOsDescription();

            log.Info($"Got OS description: {osDescription}");

            var osDescParts = osDescription.Split('.');

            if (long.TryParse(osDescParts[2], out long windowsVersion))
            {
                log.Info($"Got Windows version {windowsVersion}, minimum version is {windows2012R2MinVersion}");
                if (windowsVersion < windows2012R2MinVersion)
                {
                    result.OsMeetsRequirement = false;
                    result.OsDescription = $"ShipWorks requires Windows 10 Version 1607 (and newer) or Windows Server 2012 R2 (and newer).";
                }
            }
            else
            {
                log.Error("Unable to parse Windows version");
            }
        }

        /// <summary>
        /// Checks that the CPU has sufficient clock speed and number of cores
        /// </summary>
        /// <param name="result"></param>
        private void CheckCPU(SystemCheckResult result)
        {
            log.Info("Checking CPU");

            var cpuInfo = systemInfo.GetCPUInfo();

            log.Info($"Got CPU Info: {cpuInfo}");

            var cpuLines = cpuInfo.Split("\n");

            if (int.TryParse(cpuLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1], out var cpuMaxSpeed))
            {
                log.Info($"Got CPU max speed of {cpuMaxSpeed}, minimum required is {minCpuSpeed}");

                if (cpuMaxSpeed < minCpuSpeed)
                {
                    result.CpuMeetsRequirement = false;
                    result.CpuDescription = $"ShipWorks requires at a minimum 1.5 GHz processor.";
                }
            }
            else
            {
                log.Error("Unable to parse CPU speed");
            }

            if (result.CpuMeetsRequirement &&
                int.TryParse(cpuLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1], out var numberOfCores))
            {
                log.Info($"Got {numberOfCores} CPU cores, minimum is {minCpuCores}");

                if (numberOfCores < minCpuCores)
                {
                    result.CpuMeetsRequirement = false;
                    result.CpuDescription = $"ShipWorks requires at least 2 processor cores.";
                }
            }
            else
            {
                log.Error("Unable to parse CPU cores");
            }
        }

        /// <summary>
        /// Checks that the HDD has sufficient available space
        /// </summary>
        /// <param name="result"></param>
        private void CheckHDD(SystemCheckResult result)
        {
            log.Info("Checking drives");
            var drives = systemInfo.GetDriveInfo();

            string driveString = string.Join('\n', drives.Select(x => $"Name: {x.Name}, Free space: {GetAvailibleFreeSpace(x) / bytesInGigaByte}GB"));

            log.Info($"Got drive info, minimum free space required is {minSpaceInGb}GB: {driveString}");

            result.HddMeetsRequirement = drives
                .Any(d => (GetAvailibleFreeSpace(d) / bytesInGigaByte) > minSpaceInGb);

            if (!result.HddMeetsRequirement)
            {
                log.Error("No drives meet minimum space requirement");
                result.HddDescription = $"ShipWorks requires at least 20 GB of free storage.";
            }
        }

        /// <summary>
        /// Gets the available free space of the drive and
        /// handles any errors that might occur
        /// </summary>
        private long GetAvailibleFreeSpace(IDriveInfo drive)
        {
            try
            {
                return drive.AvailableFreeSpace;
            }
            catch (Exception ex)
            {
                log.Info($"An error occured getting available space for drive ${drive.Name}: ${ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Checks that the system has sufficient RAM
        /// </summary>
        /// <param name="result"></param>
        private void CheckRAM(SystemCheckResult result)
        {
            log.Info("Checking system RAM");

            var ramInfo = systemInfo.GetRamInfo();

            log.Info($"Got RAM info: {ramInfo}");

            var memoryLines = ramInfo.Split("\n");

            var totalMemoryText = memoryLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];

            if (long.TryParse(totalMemoryText, out var totalMemory))
            {
                log.Info($"Got total RAM of {totalMemory / kbInGigaByte}GB, minimum is {minRamInKb / kbInGigaByte}GB");
                if (totalMemory < minRamInKb)
                {
                    result.RamMeetsRequirement = false;
                    result.RamDescription = $"ShipWorks requires at least {minRamInKb / kbInGigaByte}GB RAM.";
                }
            }
            else
            {
                log.Error("Unable to parse RAM info");
            }
        }

        /// <summary>
        /// Checks a drive letter to see if it meets the minimum size requirements
        /// </summary>
        /// <param name="driveLetter"></param>
        public bool DriveMeetsRequirements(string driveLetter)
        {
            log.Info($"Checking free space on drive {driveLetter}");

            var freeSpace = systemInfo.GetDriveInfo().FirstOrDefault(d => d.Name.Equals(driveLetter, StringComparison.OrdinalIgnoreCase))?.AvailableFreeSpace;

            log.Info($"Got {freeSpace / bytesInGigaByte}GB free space, minimum is {minSpaceInGb}GB");

            if (freeSpace == null)
            {
                return false;
            }

            return freeSpace / bytesInGigaByte > minSpaceInGb;
        }
    }
}
