using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;
using log4net;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Manages the starting and stopping of a Windows service.
    /// </summary>
    public class WindowsServiceController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceController));

        private readonly ServiceBase service;

        private const int timeoutMilliseconds = 30000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksServiceManager" /> class.
        /// </summary>
        /// <param name="shipWorksService">The service to manage.</param>
        public WindowsServiceController(ServiceBase service)
        {
            this.service = service;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
            try
            {
                using (ServiceController controller = new ServiceController(service.ServiceName))
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't start service " + service.ServiceName, ex);

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
                using (ServiceController controller = new ServiceController(service.ServiceName))
                {
                    int beforeStopService = Environment.TickCount;
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    ServiceControllerStatus serviceControllerStatus = GetServiceStatus();
                    if (serviceControllerStatus != ServiceControllerStatus.Stopped && serviceControllerStatus != ServiceControllerStatus.StopPending)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, timeout);    
                    }

                    // count the rest of the timeout
                    int afterStopService = Environment.TickCount;
                    timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (afterStopService - beforeStopService));

                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't restart service " + service.ServiceName, ex);

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
                using (ServiceController controller = new ServiceController(service.ServiceName))
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't stop service " + service.ServiceName, ex);
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
                using (var controller = new ServiceController(service.ServiceName))
                {
                    try
                    {
                        // This variable isn't used. The only reason it is here is to see if .Status raises an exception.
                        ServiceControllerStatus serviceControllerStatus = controller.Status;

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
            using (var controller = new ServiceController(service.ServiceName))
            {
                return controller.Status;
            }
        }

        /// <summary>
        /// Gets the service display name.
        /// </summary>
        public string GetServiceDisplayName()
        {
            using (var controller = new ServiceController(service.ServiceName))
            {
                return controller.DisplayName;
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

                    ChangeServiceCredentials.ServicePasswordChange(getWindowsCredentialsDlg.Domain, getWindowsCredentialsDlg.UserName, getWindowsCredentialsDlg.Password, service.ServiceName);

                    RestartService();
                }
            }
        }
    }
}