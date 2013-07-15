using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Manages the starting and stopping of a ShipWorks service.
    /// </summary>
    public class ShipWorksServiceManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceManager));

        private readonly ShipWorksServiceBase shipWorksService;

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
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

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
        /// Prompts user for new credentials and updates the service with new credentials.
        /// </summary>
        public void ChangeCredentials()
        {
            using (GetWindowsCredentialsDlg getWindowsCredentialsDlg = new GetWindowsCredentialsDlg())
            {
                DialogResult dialogResult = getWindowsCredentialsDlg.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string usernameWithDomain = string.Format(@"{0}\{1}",
                        getWindowsCredentialsDlg.Domain ?? ".",
                        getWindowsCredentialsDlg.UserName);

                    ChangeServiceCredentials.ServicePasswordChange(usernameWithDomain, getWindowsCredentialsDlg.Password, shipWorksService.ServiceName);
                }
            }
        }
    }
}