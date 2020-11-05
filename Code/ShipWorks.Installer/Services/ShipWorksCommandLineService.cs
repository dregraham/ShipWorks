using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShipWorks.Installer.Extensions;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for calling ShipWorks command line options
    /// </summary>
    public class ShipWorksCommandLineService : IShipWorksCommandLineService
    {
        private const string AutoInstallShipWorksFileName = "AutoInstallShipWorks.config";

        /// <summary>
        /// Call ShipWorks to install LocalDB, create the db, create the user, link the warehouse, etc...
        /// </summary>
        public async Task<int> AutoInstallShipWorks(InstallSettings installSettings)
        {
            var swExeFilename = Path.Combine(installSettings.InstallPath, "ShipWorks.exe");
            var autoInstallConfigPathAndFilename = Path.Combine(installSettings.InstallPath, AutoInstallShipWorksFileName);

            var autoInstallShipWorksConfigJson = JsonConvert.SerializeObject(installSettings);
            await File.WriteAllTextAsync(autoInstallConfigPathAndFilename, autoInstallShipWorksConfigJson).ConfigureAwait(false);

            var info = new ProcessStartInfo(swExeFilename)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Arguments = $"/c=autoinstallsw {installSettings.TangoPassword}",
                Verb = "runas"
            };

            int exitCode = 0;
            using (var process = Process.Start(info))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                if (process?.ExitCode != null)
                {
                    exitCode = (int) process?.ExitCode;
                }
            }

            if (exitCode != 0)
            {
                string autoInstallShipWorksJson = await File.ReadAllTextAsync(autoInstallConfigPathAndFilename).ConfigureAwait(false);
                var autoInstallShipWorksConfig = JsonConvert.DeserializeObject<InstallSettings>(autoInstallShipWorksJson);
                installSettings.AutoInstallErrorMessage = autoInstallShipWorksConfig.AutoInstallErrorMessage;
            }

            return exitCode;
        }
    }
}
