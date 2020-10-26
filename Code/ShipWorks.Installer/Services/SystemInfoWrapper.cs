
using System.Diagnostics;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Wrapper class for calls made to get system info
    /// </summary>
    public class SystemInfoWrapperService : ISystemInfoService
    {
        private readonly IDriveInfo driveInfo;
        public SystemInfoWrapperService(IDriveInfo driveInfo)
        {
            this.driveInfo = driveInfo;
        }

        /// <summary>
        /// Gets the description of the OS
        /// </summary>
        public string GetOsDescription() => System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        /// <summary>
        /// Gets the max clock speed and number of cores in the cpu
        /// </summary>
        public string GetCPUInfo() => GetWmicOutput("CPU get MaxClockSpeed, NumberOfCores /Value");

        /// <summary>
        /// Gets drive info for all drives in the system
        /// </summary>
        public IDriveInfo[] GetDriveInfo() => driveInfo.GetDrives();

        /// <summary>
        /// Gets the amount of memory in the system
        /// </summary>
        public string GetRamInfo() => GetWmicOutput("OS get TotalVisibleMemorySize /Value");

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
