using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Service for updating shipworks
    /// </summary>
    public class UpdateService : IUpdateService
    {
        private NamedPipeClientStream updaterPipe;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService(IShipWorksSession shipWorksSession)
        {
            updaterPipe = new NamedPipeClientStream(".", shipWorksSession.InstanceID.ToString(), PipeDirection.Out);
        }

        /// <summary>
        /// Check if the updater is available
        /// </summary>
        public bool IsAvailable()
        {
            if (!updaterPipe.IsConnected)
            {
                // Give it 1 second to connect
                try
                {
                    updaterPipe.Connect(1000);
                }
                catch (Exception)
                {
                    // Connection can fail if something else is connected
                    // or if the timeout has elapsed
                    return false;
                }
            }

            return updaterPipe.IsConnected;
        }

        /// <summary>
        /// Kick off the update
        /// </summary>
        public Result Update(Version version)
        {
            // before we send the update message make sure we can start a splash screen 
            // because the update process will kill us
            string fileName = "ShipWorks.SplashScreen.exe";
            string sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), fileName);
            string destinationPath = Path.Combine(DataPath.InstanceRoot, fileName);
            File.Copy(sourcePath, destinationPath, true);

            var result = SendMessage(version.ToString());

            if (result.Success && File.Exists(destinationPath))
            {
                // start the splash screen here
                Process.Start(destinationPath);
            }

            return result;
        }
            
        
        /// <summary>
        /// Send the update window information to the updater pipe
        /// </summary>
        public Result SendMessage(string message)
        {
            if (IsAvailable())
            {
                try
                {
                    updaterPipe.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
                }
                catch (Exception ex)
                {
                    return Result.FromError(ex);
                }
                return Result.FromSuccess();
            }

            return Result.FromError("Could not connect to update service.");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            updaterPipe.Dispose();
        }

    }
}
