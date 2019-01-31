using System;
using System.Reflection;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Diagnostics;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// The Escalator program file
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        string serviceName = ServiceName.Resolve();
                        ManagedInstallerClass.InstallHelper(new string[] {$"/ServiceName={serviceName}", Assembly.GetExecutingAssembly().Location });
                        SetRecoveryOptions(serviceName);
                        var sc = new ServiceController(serviceName);
                        sc.Start();                        
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                RunService();
            }
        }

        /// <summary>
        /// Method to run the actual service
        /// </summary>
        private static void RunService()
        {
            ServiceBase.Run(new Escalator());
        }

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails (the spacing in the below args looks weird but is correct)
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
