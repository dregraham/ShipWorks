using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
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
        private string UpdateInProgressFilePath;
        private NamedPipeClientStream updaterPipe;
        private IMainForm mainForm;
        private IShipWorksCommunicationBridge autoUpdateStartPipe;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;
        private readonly ISqlSession sqlSession;
        private readonly ITangoGetReleaseByCustomerRequest tangoGetReleaseByCustomerRequest;
        private readonly Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateService(
            IShipWorksSession shipWorksSession,
            IAutoUpdateStatusProvider autoUpdateStatusProvider,
            ISqlSession sqlSession,
            ITangoGetReleaseByCustomerRequest tangoGetReleaseByCustomerRequest,
            Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory,
            IDataPath dataPath)
        {
            updaterPipe = new NamedPipeClientStream(".", shipWorksSession.InstanceID.ToString(), PipeDirection.Out);
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            this.sqlSession = sqlSession;
            this.tangoGetReleaseByCustomerRequest = tangoGetReleaseByCustomerRequest;
            this.communicationBridgeFactory = communicationBridgeFactory;
            UpdateInProgressFilePath = Path.Combine(dataPath.InstanceSettings, "UpdateInProcess.txt");
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
            if (File.Exists(UpdateInProgressFilePath))
            {
                string version = File.ReadAllText(UpdateInProgressFilePath);

                try
                {
                    File.Delete(UpdateInProgressFilePath);
                }
                catch (IOException)
                {
                    // this technically should never happen but if the file is in use
                    // deleting it will fail, in that case ignore the error
                }

                // if the version we are currently running is lower than the version we were trying to update to then something went wrong
                // skip the rest of the update process here because we dont want to get stuck in a loop where we keep attempting
                // to update and fail
                if (Version.TryParse(version, out Version updateInProcessVersion) && databaseVersion < updateInProcessVersion)
                {
                    return Result.FromError("The previous ShipWorks auto update failed. Restart ShipWorks to try again.");
                }
            }

            // For localdb we manually check to see if a new version is available, this is because
            // the windows service that is running our update service does not have access to localdb
            if (sqlSession.Configuration.IsLocalDb())
            {
                DataProvider.InitializeForApplication();
                StoreManager.InitializeForCurrentSession(SecurityContext.EmptySecurityContext);
                UserSession.InitializeForCurrentDatabase();
                GenericResult<ShipWorksReleaseInfo> releaseInfo = tangoGetReleaseByCustomerRequest.GetReleaseInfo();

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
            try
            {
                File.WriteAllText(UpdateInProgressFilePath, version.ToString());
            }
            catch (Exception)
            {
                // If we cant write a file to disk to signify that we are attempting
                // upgrade then fail because it could get us stuck in an upgrade loop
                return Result.FromError($"unable to write to {UpdateInProgressFilePath}.");
            }

            Result result = SendMessage(version.ToString());

            if (result.Success)
            {
                autoUpdateStatusProvider.ShowSplashScreen(ShipWorksSession.InstanceID.ToString("B"));
            }
            else
            {
                try
                {
                    File.Delete(UpdateInProgressFilePath);
                }
                catch (Exception)
                {
                    // this failure wil be ignored because we created the file
                    // above and it shall not fail
                }
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
        /// Listen for an auto update starting message
        /// </summary>
        public void ListenForAutoUpdateStart(IMainForm mainForm)
        {
            autoUpdateStartPipe = communicationBridgeFactory("AutoUpdateStart");
            autoUpdateStartPipe.OnMessage += (s) => OnAutoUpdateStartmessage(s, mainForm);

            autoUpdateStartPipe.StartPipeServer();
        }

        /// <summary>
        /// Handle shipworks auto update starting
        /// </summary>
        private void OnAutoUpdateStartmessage(string message, IMainForm mainForm)
        {
            if (!message.StartsWith("KillMe"))
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

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            updaterPipe.Dispose();
        }
    }
}
