using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
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
        private const string ShipWorksApiCertificateName = "ShipWorksAPI";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiPortRegistrationService(Func<Type, ILog> logFactory)
        {
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
        public bool Register(long portNumber, bool useHttps, long oldPortNumber, bool oldUseHttps)
        {
            RemoveOldRegistrations(portNumber, useHttps, oldPortNumber, oldUseHttps);

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
        /// Remove any old url and sslcert registrations
        /// </summary>
        private void RemoveOldRegistrations(long portNumber, bool useHttps, long oldPortNumber, bool oldUseHttps)
        {
            if (portNumber != oldPortNumber || useHttps != oldUseHttps)
            {
                string s = oldUseHttps ? "s" : string.Empty;
                string command = $"http delete urlacl url=http{s}://+:{oldPortNumber}/";

                if (NetshCommand.Execute(command) != 0)
                {
                    log.Error("Failed to remove old url registration");
                }

                if (oldUseHttps)
                {
                    command = $"http delete sslcert ipport=0.0.0.0:{oldPortNumber}";

                    if (NetshCommand.Execute(command) != 0)
                    {
                        log.Error("Failed to remove ssl cert from old url");
                    }
                }
            }
        }

        /// <summary>
        /// Register the given port and add an ssl certificate to it
        /// </summary>
        private bool RegisterWithHttps(long portNumber)
        {
            try
            {
                string thumbprint = GetShipWorksApiCertificateThumbprint();
                if (string.IsNullOrWhiteSpace(thumbprint))
                {
                    log.Error("Failed to retrieve ShipWorksAPI certificate");
                }

                // register the port
                string command = $"http add urlacl url=https://+:{portNumber}/ user=Everyone";

                if (NetshCommand.Execute(command) == 0)
                {
                    // add the ssl cert to the port
                    command = $"http add sslcert ipport=0.0.0.0:{portNumber} certhash={thumbprint} appid={{214124cd-d05b-4309-9af9-9caa44b2b74a}} certstorename=Root";

                    return NetshCommand.Execute(command) == 0;
                }
                else
                {
                    log.Error($"Failed to register url https://+:{portNumber}/");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while attempting to register port with https.", ex);
                return false;
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
