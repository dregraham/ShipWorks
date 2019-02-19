using System;
using System.IO.Pipes;
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
        public Result Update(Version version) =>
            SendMessage(version.ToString());
        
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
