using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ShipWorks.Installer.Extensions
{
    /// <summary>
    /// Extension class for System.Diagnostics.Process
    /// </summary>
    public static class ProcessExtensions
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

        /// <summary>
        /// Open Windows Explorer to given folder
        /// </summary>
        public static void OpenFolder(string folderPath)
        {
            Process.Start(new ProcessStartInfo("explorer", folderPath));
        }

        /// <summary>
        /// Wait for a process to exit asynchronously
        /// </summary>
        public static async Task<int> WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Process_Exited(object sender, EventArgs e)
            {
                tcs.TrySetResult(true);
            }

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            try
            {
                if (process.HasExited)
                {
                    return process.ExitCode;
                }

                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                {
                    await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                process.Exited -= Process_Exited;
            }

            return process.ExitCode;
        }
    }
}
