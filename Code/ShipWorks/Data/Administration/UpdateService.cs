using System;
using System.IO.Pipes;
using System.Text;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Service for updating shipworks
    /// </summary>
    public class UpdateService : IUpdateService
    {
        private NamedPipeClientStream updaterPipe;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService(IShipWorksSession shipWorksSession, IAutoUpdateStatusProvider autoUpdateStatusProvider)
        {
            updaterPipe = new NamedPipeClientStream(".", shipWorksSession.InstanceID.ToString(), PipeDirection.Out);
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
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
        /// Try to update ShipWorks
        /// </summary>
        /// <returns></returns>
        public Result TryUpdate()
        {

            // Check to see if last update failed(if file exists and not the version we are on) and alert user.Then delete file
            // Write file saying we are upgrading to version x.x.x.x
            // get current logic from Mainform.AutoUpdate
            //Add local DB check and see if there is a new version from ITangoGetReleaseByCustomer

            // Check see if the database has been updated and we need to update
            if (SqlSession.IsConfigured && SqlSession.Current.CanConnect())
            {
                Version databaseVersion = SqlSchemaUpdater.GetInstalledSchemaVersion();

                if (databaseVersion > SqlSchemaUpdater.GetRequiredSchemaVersion())
                {
                    // need to update
                    return Update(databaseVersion);
                }
            }

            return Result.FromError("");

        }

        /// <summary>
        /// Kick off the update
        /// </summary>
        public Result Update(Version version)
        {
            Result result = SendMessage(version.ToString());

            if (result.Success)
            {
                autoUpdateStatusProvider.ShowSplashScreen();
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
