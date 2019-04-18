using System;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data.Connection;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Service for updating shipworks
    /// </summary>
    public class UpdateService : IUpdateService
    {
        private readonly string updateInProgressFilePath;
        private IShipWorksCommunicationBridge autoUpdateStartPipe;
        private readonly IShipWorksSession shipWorksSession;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;
        private readonly ISqlSession sqlSession;
        private readonly ITangoGetReleaseByUserRequest tangoGetReleaseByUserRequest;
        private readonly Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public UpdateService(
            IShipWorksSession shipWorksSession,
            IAutoUpdateStatusProvider autoUpdateStatusProvider,
            ISqlSession sqlSession,
            ITangoGetReleaseByUserRequest tangoGetReleaseByUserRequest,
            Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory,
            IDataPath dataPath)
        {
            this.shipWorksSession = shipWorksSession;
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            this.sqlSession = sqlSession;
            this.tangoGetReleaseByUserRequest = tangoGetReleaseByUserRequest;
            this.communicationBridgeFactory = communicationBridgeFactory;
            updateInProgressFilePath = Path.Combine(dataPath.InstanceSettings, "UpdateInProcess.txt");
        }

        /// <summary>
        /// Try to update ShipWorks
        /// </summary>
        /// <returns></returns>
        public Result TryUpdate()
        {
            if (!sqlSession.CanConnect())
            {
                return Result.FromError(string.Empty);
            }

            // Check to see if we just launched shipworks after attempting to update the exe
            if (File.Exists(updateInProgressFilePath))
            {
                string version = File.ReadAllText(updateInProgressFilePath);

                try
                {
                    File.Delete(updateInProgressFilePath);
                }
                catch (IOException)
                {
                    // this technically should never happen but if the file is in use
                    // deleting it will fail, in that case ignore the error
                }

                // We are starting back up after installing an update, dont update again because that
                // could get us stuck in an update loop
                return Result.FromError("An update was just installed, skipping updates.");
            }

            // For localdb we manually check to see if a new version is available, this is because
            // the windows service that is running our update service does not have access to localdb
            if (sqlSession.Configuration.IsLocalDb() && !SqlSchemaUpdater.IsUpgradeRequired() && !AutoUpdateSettings.IsAutoUpdateDisabled)
            {
                DataProvider.InitializeForApplication();
                StoreManager.InitializeForCurrentSession(SecurityContext.EmptySecurityContext);
                UserSession.InitializeForCurrentDatabase();
                GenericResult<ShipWorksReleaseInfo> releaseInfo = tangoGetReleaseByUserRequest.GetReleaseInfo();

                if (releaseInfo.Success)
                {
                    Version versionToUpgradeTo = releaseInfo.Value.ReleaseVersion;

                    if (versionToUpgradeTo > typeof(UpdateService).Assembly.GetName().Version)
                    {
                        return Update(versionToUpgradeTo);
                    }
                }
            }

            // Check see if the database has been updated and we need to update
            if (SqlSchemaUpdater.GetInstalledSchemaVersion() > SqlSchemaUpdater.GetRequiredSchemaVersion() && !AutoUpdateSettings.IsAutoUpdateDisabled)
            {
                // need to update
                return Update(SqlSchemaUpdater.GetBuildVersion());
            }

            // No updates return an empty error so the consumer doesn't think that an update is kicking off
            return Result.FromError(string.Empty);
        }

        /// <summary>
        /// Kick off the update
        /// </summary>
        public Result Update(Version version)
        {
            try
            {
                File.WriteAllText(updateInProgressFilePath, version.ToString());
            }
            catch (Exception)
            {
                // If we cant write a file to disk to signify that we are attempting
                // upgrade then fail because it could get us stuck in an upgrade loop
                return Result.FromError($"unable to write to {updateInProgressFilePath}.");
            }

            IShipWorksCommunicationBridge communicationBridge = communicationBridgeFactory(shipWorksSession.InstanceID.ToString());
            Result result = communicationBridge.SendMessage(version.ToString());

            if (result.Success)
            {
                autoUpdateStatusProvider.ShowSplashScreen(shipWorksSession.InstanceID.ToString("B"));
            }
            else
            {
                try
                {
                    File.Delete(updateInProgressFilePath);
                }
                catch (Exception)
                {
                    // this failure will be ignored because we created the file
                    // above and it shall not fail
                }
            }

            return result;
        }

        /// <summary>
        /// Listen for an auto update starting message
        /// </summary>
        public void ListenForAutoUpdateStart(IMainForm mainForm)
        {
            autoUpdateStartPipe = communicationBridgeFactory($"{shipWorksSession.InstanceID.ToString()}_AutoUpdateStart");
            autoUpdateStartPipe.OnMessage += (s) => OnAutoUpdateStartmessage(s, mainForm);

            autoUpdateStartPipe.StartPipeServer();
        }

        /// <summary>
        /// Handle shipworks auto update starting
        /// </summary>
        private void OnAutoUpdateStartmessage(string message, IMainForm mainForm)
        {
            if (!message.StartsWith("CloseShipWorks"))
            {
                return;
            }

            if (!mainForm.AdditionalFormsOpen())
            {
                if (mainForm.InvokeRequired)
                {
                    mainForm.BeginInvoke(new MethodInvoker(mainForm.Close));
                } else
                {
                    mainForm.Close();
                }
            }
            else
            {
               var timer = new System.Timers.Timer();
                timer.Elapsed += (a, b) =>
                {
                    if (!mainForm.AdditionalFormsOpen())
                    {
                        if (mainForm.InvokeRequired)
                        {
                            mainForm.BeginInvoke(new MethodInvoker(mainForm.Close));
                        }
                        else
                        {
                            mainForm.Close();
                        }
                    }
                };
                timer.Interval = 2000;
                timer.Start();
            }
        }
    }
}
