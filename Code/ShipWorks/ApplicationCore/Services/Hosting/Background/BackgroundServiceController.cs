using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.AddressValidation;

namespace ShipWorks.ApplicationCore.Services.Hosting.Background
{
    /// <summary>
    /// Provides control operations of the ShipWorks service when its running as a background process
    /// </summary>
    public static class BackgroundServiceController
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceController));

        /// <summary>
        /// Self-hosts a ShipWorks service object as an instance-specific background process instead of via the SCM.
        /// If the background process is already running, it is signaled to stop and will be superseded by this instance.
        /// This is a blocking call for the lifetime of the running service.
        /// </summary>
        /// <returns>true if the service ran; otherwise, false.</returns>
        /// <returns>true if the service ran to completion in the background; false if the service was already running and could not be stopped.</returns>
        public static bool RunInBackground(ShipWorksServiceBase service)
        {
            if (null == service)
            {
                throw new ArgumentNullException("service");
            }

            // Suppress the nappy-headed Windows error dialogs
            NativeMethods.SetErrorMode(NativeMethods.SEM_FAILCRITICALERRORS | NativeMethods.SEM_NOGPFAULTERRORBOX);

            bool createdNew;
            using (var stopSignal = new EventWaitHandle(false, EventResetMode.ManualReset, service.ServiceName, out createdNew))
            {
                if (!createdNew)
                {
                    log.Warn("Service is already running in the background; sending stop signal.");
                    if (!stopSignal.Set())
                    {
                        // MSDN gives no indication why or when this could happen...
                        log.Error("Stop signal failed!");
                        return false;
                    }
                }

                service.InternalHostStart();

                stopSignal.Reset();
                stopSignal.WaitOne();
                log.Info("Stop signal received; shutting down service.");

                service.InternalHostStop();

                return true;
            }
        }

        /// <summary>
        /// Hosts all ShipWorks services as new instance-specific background processes.
        /// If a background process is already running, it is signaled to stop and will be superseded by the new process.
        /// This is a non-blocking call as all running services are started in new processes.
        /// </summary>
        public static void RunAllInBackground()
        {
            foreach (var entry in EnumHelper.GetEnumList<ShipWorksServiceType>())
            {
                Process.Start(Program.AppFileName, "/s=" + entry.ApiValue).Dispose();
            }
        }

        /// <summary>
        /// Signals an instance-specific ShipWorks background service process that it should shut down.
        /// </summary>
        /// <returns>true if the service was not running, or was running and signaled; false if the signal fails.</returns>
        public static bool StopInBackground(ShipWorksServiceType serviceType)
        {
            return StopInBackground(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Signals an instance-specific ShipWorks background service process that it should shut down.
        /// </summary>
        /// <returns>true if the service was not running, or was running and signaled; false if the signal fails.</returns>
        public static bool StopInBackground(ShipWorksServiceType serviceType, Guid instanceID)
        {
            var serviceName = ShipWorksServiceManager.GetServiceName(serviceType, instanceID);

            bool createdNew;
            using (var stopSignal = new EventWaitHandle(false, EventResetMode.ManualReset, serviceName, out createdNew))
            {
                if (!createdNew && !stopSignal.Set())
                {
                    // MSDN gives no indication why or when this could happen...
                    log.Error("Stop signal failed!");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Signals all running instance-specific ShipWorks background processes to shut down.
        /// </summary>
        /// <returns>true if the all services were not running, or were running and signaled; false if any signal fails.</returns>
        public static bool StopAllInBackground()
        {
            return StopAllInBackground(ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Signals all running instance-specific ShipWorks background processes to shut down.
        /// </summary>
        /// <returns>true if the all services were not running, or were running and signaled; false if any signal fails.</returns>
        public static bool StopAllInBackground(Guid instanceID)
        {
            return EnumHelper.GetEnumList<ShipWorksServiceType>().Select(e => e.Value)
                .Select(serviceType => StopInBackground(serviceType, instanceID))
                .Aggregate((all, stopped) => all && stopped);
        }

    }
}
