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
        public bool Register(long portNumber)
        {
            string command = $"http add urlacl url=http://+:{portNumber}/ user=Everyone";

            return NetshCommand.Execute(command) == 0;
        }

        /// <summary>
        /// Register the given port running the process as admin
        /// </summary>
        public bool RegisterAsAdmin(long portNumber)
        {
            try
            {
                // We need to launch the process to elevate ourselves
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, $"/cmd:{new RegisterApiPortCommandLineOption().CommandName} {portNumber}");
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

        public bool RegisterWithHttpsAsAdmin(long portNumber)
        {
            throw new NotImplementedException();
        }
    }
}
