using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.CommandLineOptions;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// service for registering a port for the api
    /// </summary>
    [Component]
    public class ApiPortRegistrationService : IApiPortRegistrationService
    {
        private readonly IShipWorksSession shipWorksSession;
        private const string ShipWorksApiCertificateName = "ShipWorksAPI";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiPortRegistrationService(IShipWorksSession shipWorksSession, Func<Type, ILog> logFactory)
        {
            this.shipWorksSession = shipWorksSession;
            log = logFactory(typeof(ApiPortRegistrationService));
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

        /// <summary>
        /// Register the given port
        /// </summary>
        public bool Register(ApiSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.LastSuccessfulUrl))
            {
                // We need to remove the previous registrations so they don't conflict with new ones
                RemoveOldRegistrations(settings.LastSuccessfulUrl);
            }

            // register the new url
            string s = settings.UseHttps ? "s" : string.Empty;
            string command = $"http add urlacl url=http{s}://+:{settings.Port}/ user=Everyone";

            if (NetshCommand.Execute(command) != 0)
            {
                // if the add url failed, re-add the old one
                command = $"http add urlacl url={settings.LastSuccessfulUrl} user=Everyone";
                NetshCommand.Execute(command);
                return false;
            }

            if (settings.UseHttps)
            {
                Result result = RegisterWithHttps(settings.Port);
                if (result.Failure)
                {
                    log.Error("Failed to retrieve ShipWorksAPI certificate");

                    // If we failed to add the sslcert to the new url, add it back to the old one
                    RegisterWithHttps(new Uri(settings.LastSuccessfulUrl).Port);
                    return false;
                }

                return true;
            }

            return true;
        }

        /// <summary>
        /// Remove any old url and sslcert registrations
        /// </summary>
        private void RemoveOldRegistrations(string oldUrl)
        {
            Uri oldUri = new Uri(oldUrl.Replace("+", "localhost"));

            log.Info("Removing old url registration");
            bool oldUseHttps = oldUri.Scheme == Uri.UriSchemeHttps;

            string command = $"http delete urlacl url={oldUrl}";

            if (NetshCommand.Execute(command) != 0)
            {
                log.Error("Failed to remove old url registration");
            }

            if (oldUseHttps)
            {
                command = $"http delete sslcert ipport=0.0.0.0:{oldUri.Port}";

                if (NetshCommand.Execute(command) != 0)
                {
                    log.Error("Failed to remove ssl cert from old url");
                }
            }
        }

        /// <summary>
        /// Register the given port and add an ssl certificate to it
        /// </summary>
        private Result RegisterWithHttps(long portNumber)
        {
            try
            {
                string thumbprint = GetShipWorksApiCertificateThumbprint();
                if (string.IsNullOrWhiteSpace(thumbprint))
                {
                    return Result.FromError("Failed to retrieve ShipWorksAPI certificate");
                }

                // add the ssl cert to the port
                string command = $"http add sslcert ipport=0.0.0.0:{portNumber} certhash={thumbprint} appid={{{shipWorksSession.InstanceID}}} certstorename=Root";

                return NetshCommand.Execute(command) == 0 ?
                    Result.FromSuccess() :
                    Result.FromError("Failed to add ssl cert to url");
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Get the thumbprint of the ShipWorksAPI certificate. If a certificate does not already exist, this will create one.
        /// </summary>
        private string GetShipWorksApiCertificateThumbprint()
        {
            X509Store rootStore = new X509Store("Root", StoreLocation.LocalMachine);
            rootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2 cert = GetShipWorksApiCertFromStore(rootStore) ?? CreateShipWorksApiCertificate();

            return cert?.Thumbprint;
        }

        /// <summary>
        /// Create a ShipWorksAPI certificate and move it to the root certificate store
        /// </summary>
        private X509Certificate2 CreateShipWorksApiCertificate()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments =
                    $@"New-SelfSignedCertificate -FriendlyName {ShipWorksApiCertificateName} -NotAfter (Get-Date -Year 2038 -Month 1 -Day 19) -Subject {ShipWorksApiCertificateName} -CertStoreLocation Cert:\LocalMachine\My",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            process.Start();
            process.WaitForExit();

            // New-SelfSignedCertificate can only store them in the "My" store. Get the thumbprint so we can move it
            X509Store myStore = new X509Store("My", StoreLocation.LocalMachine);
            myStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var cert = GetShipWorksApiCertFromStore(myStore);

            process.StartInfo.Arguments = $"Move-Item -Path Cert:\\LocalMachine\\My\\{cert.Thumbprint} -Destination Cert:\\LocalMachine\\Root";
            process.Start();
            process.WaitForExit();

            return cert;
        }

        /// <summary>
        /// Get the ShipWorksAPI certificate from the given store. Returns null if not found.
        /// </summary>
        private static X509Certificate2 GetShipWorksApiCertFromStore(X509Store store)
        {
            X509Certificate2Collection certCollection =
                store.Certificates.Find(X509FindType.FindBySubjectName, ShipWorksApiCertificateName, false);

            return certCollection.Count > 0 ? certCollection[0] : null;
        }
    }
}
