using System;
using System.IO;
using System.IO.Pipes;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService()
        {
            updaterPipe = new NamedPipeClientStream(ShipWorksSession.InstanceID.ToString());
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
                    catch (TimeoutException)
                    {
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
                using (StreamWriter writer = new StreamWriter(updaterPipe))
                {
                    writer.Write(SqlSchemaUpdater.GetRequiredSchemaVersion());
                }
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
