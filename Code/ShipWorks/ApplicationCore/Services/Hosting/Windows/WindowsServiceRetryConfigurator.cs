using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Sets ShipWorks service to restart if failure.
    /// </summary>
    internal static class WindowsServiceRetryConfigurator
    {
        private const int serviceConfigFailureActions = 0x2;

        private const int errorAccessDenied = 5;

        const int numActions = 3;

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
            
            var actions = new int[numActions * 2];

            actions[0] = 1; // First Failure - Restart
            actions[1] = 0; // First Failure - Immediately
            actions[2] = 1; // Second Failure - Restart
            actions[3] = 0; // Second Failure - Immediately
            actions[4] = 1; // Third Failure - Restart
            actions[5] = 0; // Third Failure - Immediately

            IntPtr actionMemoryAllocation = Marshal.AllocHGlobal(numActions * 8);
            IntPtr serviceHandle = serviceController.ServiceHandle.DangerousGetHandle();

            try
            {
                Marshal.Copy(actions, 0, actionMemoryAllocation, numActions * 2);
                var serviceFailureActions = new ServiceFailureActions
                {
                    cActions = 3,
                    dwResetPeriod = 0,
                    lpCommand = null,
                    lpRebootMsg = null,
                    lpsaActions = new IntPtr(actionMemoryAllocation.ToInt32())
                };

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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct ServiceFailureActions
        {
            public int dwResetPeriod;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpRebootMsg;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpCommand;

            public int cActions;

            public IntPtr lpsaActions;
        }

        /// <summary>
        /// Methods used to change credentials for service.
        /// 
        /// Includes methods using P/Invokes, Code analysis states it needs to be in a seperate class.
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ChangeServiceFailureActions(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref ServiceFailureActions lpInfo);
        }
    }
}