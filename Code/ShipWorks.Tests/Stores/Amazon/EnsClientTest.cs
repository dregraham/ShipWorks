using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Common.Net;
using ShipWorks.Stores.Platforms.Amazon;
using System.Net;
using System.DirectoryServices.ActiveDirectory;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Reflection;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Net;

namespace ShipWorks.Tests.Stores.Amazon
{
    /// <summary>
    /// Testing of the Amazon Event Notification Service communication via WCF.
    /// </summary>
    public class EnsClientTest
    {
        static ClientCertificate clientCert;

        /// <summary>
        /// Extracts the resource with the given name into the target file
        /// </summary>
        private static void ExtractFile(string resource, string targetFile)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(String.Format("Unable to locate resource '{0}'.", resource));
                }

                // dump the contents of the file to disk
                using (StreamReader reader = new StreamReader(stream))
                {
                    File.WriteAllText(targetFile, reader.ReadToEnd());
                }
            }
        }
        
        static EnsClientTest()
        {
            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;

            // prep the certificates to test with
            string certFile = Path.GetTempFileName();
            string keyFile = Path.GetTempFileName();

            ExtractFile(@"ShipWorks.Tests.Stores.Amazon.TestCerts.cert-QKORKMU6UWH23ZR63Q3ET4F5XFZKN4SU.pem", certFile);
            ExtractFile(@"ShipWorks.Tests.Stores.Amazon.TestCerts.pk-QKORKMU6UWH23ZR63Q3ET4F5XFZKN4SU.pem", keyFile);

            clientCert = new ClientCertificate();
            clientCert.LoadFromPemFiles(certFile, keyFile);
        }

        /// <summary>
        /// Testing a method that was failing deep in WCF for no reason.  This stopped after a reboot, keeping here for
        /// future diagnostics.
        /// 
        /// Failed with a COMException "logon failure"
        /// </summary>
        [Fact]
        public void DomainTest()
        {
            Domain domain = Domain.GetCurrentDomain();

            Console.WriteLine(domain.Name);
        }

        /// <summary>
        /// Adding a certificate to the sytem cert store
        /// </summary>
        [Fact]
        public void InstallCertificate()
        {
            clientCert.AddToCertificateStore(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
        }
    }
}
