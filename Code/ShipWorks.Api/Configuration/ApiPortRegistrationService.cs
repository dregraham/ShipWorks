using System;
using System.Diagnostics;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore.CommandLineOptions;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// service for registering a port for the api
    /// </summary>
    [Component]
    public class ApiPortRegistrationService : IApiPortRegistrationService
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiPortRegistrationService(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(ApiPortRegistrationService));
        }

        /// <summary>
        /// Register the given port
        /// </summary>
        public bool Register(long portNumber, bool useHttps)
        {
            if (useHttps)
            {
                return RegisterWithHttps(portNumber);
            }
            else
            {
                string command = $"http add urlacl url=http://+:{portNumber}/ user=Everyone";

                return NetshCommand.Execute(command) == 0;
            }
        }

        /// <summary>
        /// Register the given port running the process as admin
        /// </summary>
        public bool RegisterAsAdmin(long portNumber, bool useHttps)
        {
            try
            {
                // We need to launch the process to elevate ourselves
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, $"/cmd:{new RegisterApiPortCommandLineOption().CommandName} {portNumber} {useHttps}");
                process.StartInfo.Verb = "runas";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while attempting to register port as admin.", ex);
                return false;
            }

            return true;
        }

        private bool RegisterWithHttps(long portNumber)
        {
            string scriptFilePath = "./RegisterApiWithHttps.ps1";

            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-File \"{scriptFilePath}\" {portNumber}",
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while attempting to register port with https.", ex);
                return false;
            }

            return true;
        }
    }
}
