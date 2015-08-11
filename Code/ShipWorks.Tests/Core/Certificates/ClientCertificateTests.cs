using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Common.Net;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.Net;

namespace ShipWorks.Tests.Core.Certificates
{
    /// <summary>
    /// Fixture for exercising SSL client certificate mangement and conversions.
    /// </summary>
    public class ClientCertificateTests
    {
        delegate void TestBodyDelegate(string certificateFile);

        /// <summary>
        /// Validates that a certificate with the given subject exists
        /// </summary>
        private void VerifyAndCleanCertificate(string subject, StoreName storeName, StoreLocation storeLocation)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadWrite);
            try
            {
                for (int x = 0; x < store.Certificates.Count; x++)
                {
                    if (store.Certificates[x].Subject == subject)
                    {
                        // good, now delete and exit
                        store.Remove(store.Certificates[x]);
                        return;
                    }
                }

                Assert.Fail("Unable to locate certificate {0} in the Certificate Store.", subject);
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        /// Reads in the test certificate, writes it to a temp file, and returns that file path
        /// </summary>
        /// <returns></returns>
        private void UsingTestCertificate(TestBodyDelegate function)
        {
            string contents = "";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ClientCertificateTests), "cert_key_pem.txt"))
            {
                Assert.IsNotNull(stream, "Unable to load test certificate data.");

                using (StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            string fileName = Path.GetTempFileName();

            File.WriteAllText(fileName, contents);
            try
            {
                // execute the actual test method
                function(fileName);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }


        [Fact]
        public void FromPemFile()
        {
            UsingTestCertificate(pemFile =>
            {
                ClientCertificate certificate = new ClientCertificate();
                certificate.LoadFromPemFile(pemFile);

                Assert.IsNotNull(certificate.X509Certificate);
            });
        }

        [Fact]
        public void AddToStore()
        {
            UsingTestCertificate(pemFile =>
                {
                    ClientCertificate certificate = new ClientCertificate();
                    certificate.LoadFromPemFile(pemFile);

                    string subject = certificate.AddToCertificateStore(StoreName.My, StoreLocation.CurrentUser);
                    //VerifyAndCleanCertificate(subject, StoreName.My, StoreLocation.CurrentUser);
                });
        }

        [Fact]
        public void SerializationTest()
        {
            UsingTestCertificate(pemFile =>
                {
                    ClientCertificate certificate = new ClientCertificate();
                    certificate.LoadFromPemFile(pemFile);

                    byte[] savedBytes = certificate.Export();

                    ClientCertificate copy = new ClientCertificate();
                    copy.Import(savedBytes);

                    Assert.AreEqual(certificate, copy);
                });
        }

        [Fact]
        public void PrivateKeyImportExportSuccessTest()
        {
            UsingTestCertificate(pemFile =>
            {
                ClientCertificate certificate = new ClientCertificate();
                certificate.LoadFromPemFile(pemFile);

                byte[] savedBytes = certificate.Export();

                ClientCertificate copy = new ClientCertificate();
                copy.Import(savedBytes);

                Assert.AreEqual(certificate, copy);

                Assert.IsNotNull(copy.X509Certificate.PrivateKey);
                Assert.IsTrue(copy.X509Certificate.PrivateKey.KeySize > 0);
            });
        }
    }
}
