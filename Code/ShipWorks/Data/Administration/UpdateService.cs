using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Service for updating shipworks
    /// </summary>
    public class UpdateService : IDisposable
    {
        private NamedPipeClientStream updaterPipe;
        private readonly ISqlSchemaUpdater sqlSchemaUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService(IShipWorksSession shipWorksSession, ISqlSchemaUpdater sqlSchemaUpdater)
        {
            updaterPipe = new NamedPipeClientStream(shipWorksSession.InstanceID.ToString());
            this.sqlSchemaUpdater = sqlSchemaUpdater;
        }

        /// <summary>
        /// Check if the updater is available
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                if (!updaterPipe.IsConnected)
                {
                    // Give it 5 seconds to connect
                    try
                    {
                        updaterPipe.Connect(5000);
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
        }

        /// <summary>
        /// Kick off the update
        /// </summary>
        public Result Update()
        {
            if (IsAvailable)
            {
                string version = sqlSchemaUpdater.GetInstalledSchemaVersion().ToString();
                updaterPipe.Write(Encoding.UTF8.GetBytes(version), 0, version.Length);
                return Result.FromSuccess();
            }

            return Result.FromError("Could not connect to update service.");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            updaterPipe?.Dispose();
        }
    }
}
