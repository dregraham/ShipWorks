using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Interapptive.Shared.Win32;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Sets ShipWorks service to restart if failure.
    /// </summary>
    internal static class WindowsServiceConfiguration
    {
        private const int serviceConfigFailureActions = 0x2;
        private const int errorAccessDenied = 5;
        private const int retryActions = 3;

        /// <summary>
        /// Sets the recovery options.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Access denied while setting failure actions.
        /// or
        /// Unknown error while setting failure actions.
        /// </exception>
        public static void SetRecoveryOptions(ServiceController serviceController)
        {
            var actions = new int[retryActions * 2];

            actions[0] = 1; // First Failure - Restart
            actions[1] = 60000; // First Failure - Restart after 1 minute
            actions[2] = 1; // Second Failure - Restart
            actions[3] = 300000; // Second Failure - Restart after 5 minutes
            actions[4] = 1; // Third Failure - Restart
            actions[5] = 600000; // Third Failure - Restart after 10 minutes

            IntPtr actionMemoryAllocation = Marshal.AllocHGlobal(retryActions * 8);
            IntPtr serviceHandle = serviceController.ServiceHandle.DangerousGetHandle();

            try
            {
                // Create the data object to pInvoke with
                Marshal.Copy(actions, 0, actionMemoryAllocation, retryActions * 2);
                var serviceFailureActions = new NativeMethods.ServiceFailureActions
                {
                    cActions = 3,
                    dwResetPeriod = 86400,  // Reset failure count after 1 day
                    lpCommand = null,
                    lpRebootMsg = null,
                    lpsaActions = new IntPtr(actionMemoryAllocation.ToInt32())
                };

                // Use pInvoke to setup the service retry options
                bool success = NativeMethods.ChangeServiceFailureActions(serviceHandle, serviceConfigFailureActions, ref serviceFailureActions);
                if (!success)
                {
                    if (Marshal.GetLastWin32Error() == errorAccessDenied)
                    {
                        throw new ShipWorksServiceException("Access denied while setting failure actions.");
                    }
                    else
                    {
                        throw new ShipWorksServiceException("Unknown error while setting failure actions.");
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(actionMemoryAllocation);
                Marshal.FreeHGlobal(serviceHandle);
            }
        }

                /// <summary>
        /// Services the password change.
        /// </summary>
        /// <param name="domain">The domain - null if none.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        /// <exception cref="ShipWorksServiceException">
        /// Open Service Manager Error
        /// or
        /// Open Service Error
        /// or
        /// Could not change password :  + win32Exception.Message
        /// </exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Open Service Manager Error
        /// or
        /// Open Service Error
        /// or
        /// Could not change password :  + win32Exception.Message</exception>
        public static bool SetServiceAccount(string domain, string userName, string password, string serviceName)
        {
            string domainAndUser = string.Format(@"{0}\{1}",
                string.IsNullOrWhiteSpace(domain) ? "." : domain,
                userName);

            IntPtr databaseHandle = NativeMethods.OpenSCManager(null, null, NativeMethods.SC_MANAGER_ALL_ACCESS);
            if (databaseHandle == IntPtr.Zero)
            {
                throw new ShipWorksServiceException("Open Service Manager Error");
            }

            IntPtr serviceHandlePointer = NativeMethods.OpenService(databaseHandle, serviceName, NativeMethods.SERVICE_QUERY_CONFIG | NativeMethods.SERVICE_CHANGE_CONFIG);
            if (serviceHandlePointer == IntPtr.Zero)
            {
                throw new ShipWorksServiceException("Open Service Error");
            }

            if (!NativeMethods.ChangeServiceConfig(serviceHandlePointer, NativeMethods.SERVICE_NO_CHANGE, NativeMethods.SERVICE_NO_CHANGE, NativeMethods.SERVICE_NO_CHANGE, null, null,
                IntPtr.Zero, null, domainAndUser, password, null))
            {
                int nError = Marshal.GetLastWin32Error();
                Win32Exception win32Exception = new Win32Exception(nError);
                throw new ShipWorksServiceException("Could not change password : " + win32Exception.Message);
            }

            return true;
        }
    }
}