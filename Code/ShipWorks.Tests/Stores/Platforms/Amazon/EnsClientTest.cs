﻿using Interapptive.Shared.Net;
using ShipWorks.Tests.Shared;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Net;
using Xunit;

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
            // dump the contents of the file to disk
            string contents = typeof(EnsClientTest).Assembly.GetEmbeddedResourceString(resource);
            File.WriteAllText(targetFile, contents);
        }

        static EnsClientTest()
        {
            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;

            // prep the certificates to test with
            string certFile = Path.GetTempFileName();
            string keyFile = Path.GetTempFileName();

            ExtractFile(@"Stores.Platforms.Amazon.TestCerts.cert-QKORKMU6UWH23ZR63Q3ET4F5XFZKN4SU.pem", certFile);
            ExtractFile(@"Stores.Platforms.Amazon.TestCerts.pk-QKORKMU6UWH23ZR63Q3ET4F5XFZKN4SU.pem", keyFile);

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
        /// Adding a certificate to the system cert store
        /// </summary>
        [Fact]
        public void InstallCertificate()
        {
            clientCert.AddToCertificateStore(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
        }
    }
}
