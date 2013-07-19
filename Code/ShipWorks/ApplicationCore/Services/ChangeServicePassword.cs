using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Change credentials of a service.  Code found on StackOverflow
    /// </summary>
    internal static class ChangeServiceCredentials
    {
        private const int SC_MANAGER_ALL_ACCESS = 0x000F003F;

        private const uint SERVICE_NO_CHANGE = 0xffffffff; //this value is found in winsvc.h

        private const uint SERVICE_QUERY_CONFIG = 0x00000001;

        private const uint SERVICE_CHANGE_CONFIG = 0x00000002;

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
        public static bool ServicePasswordChange(string domain, string userName, string password, string serviceName)
        {
            string domainAndUser = string.Format(@"{0}\{1}",
                string.IsNullOrWhiteSpace(domain) ? "." : domain,
                userName);

            IntPtr databaseHandle = NativeMethods.OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
            if (databaseHandle == IntPtr.Zero)
            {
                throw new ShipWorksServiceException("Open Service Manager Error");
            }

            IntPtr serviceHandlePointer = NativeMethods.OpenService(databaseHandle, serviceName, SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG);
            if (serviceHandlePointer == IntPtr.Zero)
            {
                throw new ShipWorksServiceException("Open Service Error");
            }

            if (!NativeMethods.ChangeServiceConfig(serviceHandlePointer, SERVICE_NO_CHANGE, SERVICE_NO_CHANGE, SERVICE_NO_CHANGE, null, null,
                IntPtr.Zero, null, domainAndUser, password, null))
            {
                int nError = Marshal.GetLastWin32Error();
                Win32Exception win32Exception = new Win32Exception(nError);
                throw new ShipWorksServiceException("Could not change password : " + win32Exception.Message);
            }
            return true;
        }

        /// <summary>
        /// Methods used to change credentials for service.
        /// 
        /// Includes methods using P/Invokes, Code analysis states it needs to be in a seperate class.
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean ChangeServiceConfig(IntPtr hService, UInt32 nServiceType, UInt32 nStartType, UInt32 nErrorControl, String lpBinaryPathName, String lpLoadOrderGroup, IntPtr lpdwTagId, [In] char[] lpDependencies, String lpServiceStartName, String lpPassword, String lpDisplayName);

            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr OpenService(IntPtr hSCManager, [MarshalAs(UnmanagedType.LPTStr)] string lpServiceName, uint dwDesiredAccess);

            [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwAccess);
        }
    }
}