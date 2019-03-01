﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Service for updating shipworks
    /// </summary>
    public class UpdateService : IUpdateService
    {
        private const string UpdateInProgressFileName = "UpdateInProcess.txt";
        private NamedPipeClientStream updaterPipe;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;
        private readonly ISqlSession sqlSession;
        private readonly ITangoGetReleaseByCustomerRequest tangoGetReleaseByCustomerRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService(
            IShipWorksSession shipWorksSession, 
            IAutoUpdateStatusProvider autoUpdateStatusProvider, 
            ISqlSession sqlSession, 
            ITangoGetReleaseByCustomerRequest tangoGetReleaseByCustomerRequest)
        {
            updaterPipe = new NamedPipeClientStream(".", shipWorksSession.InstanceID.ToString(), PipeDirection.Out);
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            this.sqlSession = sqlSession;
            this.tangoGetReleaseByCustomerRequest = tangoGetReleaseByCustomerRequest;
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
            Version databaseVersion = SqlSchemaUpdater.GetInstalledSchemaVersion();
            
            // Check to see if we just launched shipworks after attempting to update the exe
            if (File.Exists(UpdateInProgressFileName))
            {
                string version = File.ReadAllText(UpdateInProgressFileName);
                File.Delete(UpdateInProgressFileName);

                // if the version we are currently running is lower than the version we were trying to update to then something went wrong
                // skip the rest of the update process here because we dont want to get stuck in a loop where we keep attempting
                // to update and fail
                if (Version.TryParse(version, out Version updateInProcessVersion) && databaseVersion > updateInProcessVersion)
                {
                    return Result.FromError("The previous ShipWorks auto update failed. Restart ShipWorks to try again.");
                }
            }

            // For localdb we manually check to see if a new version is available, this is because
            // the windows service that is running our update service does not have access to localdb
            if (sqlSession.Configuration.IsLocalDb())
            {
                GenericResult<ShipWorksReleaseInfo> releaseInfo = tangoGetReleaseByCustomerRequest.GetReleaseInfo();

                if (releaseInfo.Success)
                {
                    Version versionToUpgradeTo = releaseInfo.Value.MinAllowedReleaseVersion;

                    if (versionToUpgradeTo > typeof(UpdateService).Assembly.GetName().Version)
                    {
                        return Update(releaseInfo.Value.MinAllowedReleaseVersion);
                    }
                }
            }

            // Check see if the database has been updated and we need to update
            if (databaseVersion > SqlSchemaUpdater.GetRequiredSchemaVersion())
            {
                // need to update
                return Update(databaseVersion);
            }

            // No updates return an empty error so the consumer doesnt think that an update is kicking off
            return Result.FromError(string.Empty);
        }

        /// <summary>
        /// Kick off the update
        /// </summary>
        public Result Update(Version version)
        {
            Result result = SendMessage(version.ToString());
            
            if (result.Success)
            {
                File.WriteAllText("UpdateInProcess.txt", version.ToString());
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
