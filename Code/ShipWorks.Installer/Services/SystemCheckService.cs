using System;
using System.Diagnostics;
using System.Linq;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for checking system requirements
    /// </summary>
    public class SystemCheckService : ISystemCheckService
    {
        private const long windows10MinVersion = 14393;
        private const int windows2012R2MinVersion = 9600;
        private const long bytesInGigaByte = 1024 * 1024 * 1024;
        private const long minRamInKb = 4194304;
        private const int minSpaceInGb = 20;

        /// <summary>
        /// Check the system requirements
        /// </summary>
        public SystemCheckResult CheckSystem()
        {
            SystemCheckResult result = new SystemCheckResult();

            var osDescParts = System.Runtime.InteropServices.RuntimeInformation.OSDescription.Split('.');

            if (long.TryParse(osDescParts[2], out long windowsVersion) && windowsVersion < windows2012R2MinVersion)
            {
                result.OsMeetsRequirement = false;
                result.OsDescription = $"ShipWorks requires Windows 10 Version 1607 (and newer) or Windows Server 2012 R2 (and newer).";
            }

            // Check CPU
            var cpuLines = GetWmicOutput("CPU get MaxClockSpeed, NumberOfCores /Value").Split("\n");
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

            // Check data storage
            result.HddMeetsRequirement = System.IO.DriveInfo.GetDrives()
                .Any(d => (d.AvailableFreeSpace / bytesInGigaByte) > 20);

            if (!result.HddMeetsRequirement)
            {
                result.HddDescription = $"ShipWorks requires at least 20 GB of free storage.";
            }

            // Get and check RAM
            var memoryLines = GetWmicOutput("OS get TotalVisibleMemorySize /Value").Split("\n");
            var totalMemoryText = memoryLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];
            if (long.TryParse(totalMemoryText, out var totalMemory) && totalMemory < minRamInKb)
            {
                result.RamMeetsRequirement = false;
                result.RamDescription = $"ShipWorks requires at least 2 processor cores.";
            }

            return result;
        }

        /// <summary>
        /// Checks a drive letter to see if it meets the minimum size requirements
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        public bool DriveMeetsRequirements(string driveLetter)
        {
            var freeSpace = System.IO.DriveInfo.GetDrives().FirstOrDefault(d => d.Name.Equals(driveLetter, StringComparison.OrdinalIgnoreCase))?.AvailableFreeSpace;

            if (freeSpace == null)
            {
                return false;
            }

            return freeSpace / bytesInGigaByte > minSpaceInGb;
        }

        /// <summary>
        /// Call WMIC and return its output
        /// </summary>
        /// <param name="query"></param>
        /// <param name="redirectStandardOutput"></param>
        private string GetWmicOutput(string query, bool redirectStandardOutput = true)
        {
            var info = new ProcessStartInfo("wmic")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Arguments = query,
                RedirectStandardOutput = redirectStandardOutput
            };

            var output = "";
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }
            return output.Trim();
        }
    }
}
