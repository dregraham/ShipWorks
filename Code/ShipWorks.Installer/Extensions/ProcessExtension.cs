using System.Diagnostics;

namespace ShipWorks.Installer.Extensions
{
    public static class ProcessExtension
    {
        public static void StartWebProcess(string url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
