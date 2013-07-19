using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;
using log4net;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Manages the starting and stopping of a ShipWorks service.
    /// </summary>
    public class ShipWorksServiceManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceManager));

        private readonly ShipWorksServiceBase shipWorksService;

        private const int timeoutMilliseconds = 30000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksServiceManager" /> class.
        /// </summary>
        /// <param name="shipWorksService">The service to manage.</param>
        public ShipWorksServiceManager(ShipWorksServiceBase shipWorksService)
        {
            this.shipWorksService = shipWorksService;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
            try
            {
                using (ServiceController service = new ServiceController(shipWorksService.ServiceName))
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't start service " + shipWorksService.ServiceName, ex);

                throw;
            }
        }

        /// <summary>
        /// Restarts the service.
        /// </summary>
        public void RestartService()
        {
            try
            {
                using (ServiceController service = new ServiceController(shipWorksService.ServiceName))
                {
                    int beforeStopService = Environment.TickCount;
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    ServiceControllerStatus serviceControllerStatus = GetServiceStatus();
                    if (serviceControllerStatus != ServiceControllerStatus.Stopped && serviceControllerStatus != ServiceControllerStatus.StopPending)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);    
                    }

                    // count the rest of the timeout
                    int afterStopService = Environment.TickCount;
                    timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (afterStopService - beforeStopService));

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't restart service " + shipWorksService.ServiceName, ex);

                throw;
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void StopService()
        {
            try
            {
                using (ServiceController service = new ServiceController(shipWorksService.ServiceName))
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't stop service " + shipWorksService.ServiceName, ex);
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is service installed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is service installed]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsServiceInstalled()
        {
            // According to Microsoft documentation, if the service doesn't exist, ArgumentException is thrown
            // during construction.
            // While testing, no exception was thrown at construction and InvalidOperationException is thrown
            // when checking any field in serviceController. I'm leaving both in case Microsoft turns out to be right.
            bool serviceExists = false;
            try
            {
                using (var serviceController = new ServiceController(shipWorksService.ServiceName))
                {
                    try
                    {
                        // This variable isn't used. The only reason it is here is to see if .Status raises an exception.
                        ServiceControllerStatus serviceControllerStatus = serviceController.Status;

                        serviceExists = true;
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
            }
            catch (ArgumentException)
            {
            }

            return serviceExists;
        }

        /// <summary>
        /// Gets the service status.
        /// </summary>
        /// <returns>Status of the service.</returns>
        public ServiceControllerStatus GetServiceStatus()
        {
            using (var service = new ServiceController(shipWorksService.ServiceName))
            {
                return service.Status;
            }
        }

        /// <summary>
        /// Gets the service display name.
        /// </summary>
        public string GetServiceDisplayName()
        {
            using (var service = new ServiceController(shipWorksService.ServiceName))
            {
                return service.DisplayName;
            }
        }

        /// <summary>
        /// Prompts user for new credentials and updates the service with new credentials.
        /// </summary>
        public void ChangeCredentials()
        {
            using (GetWindowsCredentialsDlg getWindowsCredentialsDlg = new GetWindowsCredentialsDlg())
            {
                DialogResult dialogResult = getWindowsCredentialsDlg.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    AddUserRight.SetRight(getWindowsCredentialsDlg.Domain, getWindowsCredentialsDlg.UserName, "SeServiceLogonRight");

                    ChangeServiceCredentials.ServicePasswordChange(getWindowsCredentialsDlg.Domain, getWindowsCredentialsDlg.UserName, getWindowsCredentialsDlg.Password, shipWorksService.ServiceName);

                    RestartService();
                }
            }
        }
    }
}