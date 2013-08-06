﻿using log4net;
using Microsoft.Win32;


namespace ShipWorks.ApplicationCore.Services.Hosting.Background
{
    public class BackgroundServiceRegistrar : IServiceRegistrar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceRegistrar));

        const string RunKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public void RegisterAll()
        {
            log.Info("Registering all services as background processes.");

            using (var runKey = Registry.LocalMachine.OpenSubKey(RunKeyName, true))
            {
                foreach(var service in ShipWorksServiceBase.GetAllServices())
                {
                    runKey.SetValue(service.ServiceName, Program.AppFileName + " /s=" + service.ServiceType);
                }
            }

            ShipWorksServiceBase.RunAllInBackground();
        }

        public void UnregisterAll()
        {
            log.Info("Unregistering all background processes.");

            ShipWorksServiceBase.StopAllInBackground();

            using (var runKey = Registry.LocalMachine.OpenSubKey(RunKeyName, true))
            {
                foreach (var service in ShipWorksServiceBase.GetAllServices())
                {
                    runKey.DeleteValue(service.ServiceName, false);
                }
            }
        }
    }
}
