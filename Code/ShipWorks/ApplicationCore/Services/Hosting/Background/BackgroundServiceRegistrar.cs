using System.Security;
using Interapptive.Shared.Utility;
using log4net;
using Microsoft.Win32;


namespace ShipWorks.ApplicationCore.Services.Hosting.Background
{
    /// <summary>
    /// Registration of the background service version of ShipWorks, as opposed to the Windows Service
    /// </summary>
    public class BackgroundServiceRegistrar : IServiceRegistrar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceRegistrar));

        const string RunKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Register all ShipWorks services as background services.  Basically this just adds the services to the run key
        /// </summary>
        public void RegisterAll()
        {
            log.Info("Registering all services as background processes.");

            try
            {
                using (var runKey = Registry.LocalMachine.OpenSubKey(RunKeyName, true))
                {
                    foreach (var service in ShipWorksServiceManager.GetAllServices())
                    {
                        runKey.SetValue(service.ServiceName, Program.AppFileName + " /s=" + EnumHelper.GetApiValue(service.ServiceType));
                    }
                }

                // After registration, immediately start running
                BackgroundServiceController.RunAllInBackground();
            }
            catch (SecurityException ex)
            {
                throw new ShipWorksServiceException("ShipWorks is not running with the appropriate permissions to install or uninstall services.", ex);
            }
        }

        /// <summary>
        /// Unregister all ShipWorks services from being background services.  Basically this just removes the services from the Run key
        /// </summary>
        public void UnregisterAll()
        {
            log.Info("Unregistering all background processes.");

            // If the services are running, stop them now
            BackgroundServiceController.StopAllInBackground();

            // Go through each service shipworks supports
            foreach (var service in ShipWorksServiceManager.GetAllServices())
            {
                // First try to read they key to see if it even exists - if it doesn't, we don't have anything to do
                using (RegistryKey readOnly = Registry.LocalMachine.OpenSubKey(RunKeyName, false))
                {
                    if (readOnly.GetValue(service.ServiceName) != null)
                    {
                        try
                        {
                            using (var runKey = Registry.LocalMachine.OpenSubKey(RunKeyName, true))
                            {
                                runKey.DeleteValue(service.ServiceName, false);
                            }
                        }
                        catch (SecurityException ex)
                        {
                            throw new ShipWorksServiceException("ShipWorks is not running with the appropriate permissions to install or uninstall services.", ex);
                        }
                    }
                }
            }
        }
    }
}
