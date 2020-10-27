using System.Diagnostics;

namespace ShipWorks.Installer.Extensions
{
    /// <summary>
    /// Extension class for System.Diagnostics.Process
    /// </summary>
    public static class ProcessExtension
    {
        /// <summary>
        /// Starts a new process from a browser url. .NET Core introduced a breaking change that
        /// no longer allows the normal Process.Start from starting with a url.
        /// </summary>
        /// <param name="url"></param>
        public static void StartWebProcess(string url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
