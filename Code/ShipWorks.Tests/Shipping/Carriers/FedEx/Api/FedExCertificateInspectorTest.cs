﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    [TestClass]
    public class FedExCertificateInspectorTest
    {
        private FedExCertificateInspector testObject;
        
        private Mock<ICounterRatesCredentialStore> credentialStore;
        private Mock<ICertificateRequest> request;

        private X509Certificate noMatchCertificate;
        private X509Certificate singleMatchCertificate;
        private X509Certificate multiMatchCertificate;

        [TestInitialize]
        public void Initialize()
        {
            credentialStore = new Mock<ICounterRatesCredentialStore>();
            credentialStore.Setup(s => s.FedExCertificateVerificationData).Returns("<Subject><Value>FedEx</Value></Subject>");

            request = new Mock<ICertificateRequest>();
            request.Setup(r => r.Certificate).Returns(new X509Certificate());
            
            // Load the certificates from the embedded resources
            noMatchCertificate = CreateCertificate("ShipWorks.Tests.Shipping.Carriers.FedEx.Api.FedExNoMatch.cer");
            singleMatchCertificate = CreateCertificate("ShipWorks.Tests.Shipping.Carriers.FedEx.Api.FedExSingleMatch.cer");
            multiMatchCertificate = CreateCertificate("ShipWorks.Tests.Shipping.Carriers.FedEx.Api.FedExMultipleMatch.cer");
        }

        /// <summary>
        /// Helper method to create a certificate from the given resource
        /// </summary>
        private X509Certificate CreateCertificate(string certificateResource)
        {
            byte[] bytes;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream stream = thisAssembly.GetManifestResourceStream(certificateResource))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }

            return new X509Certificate(bytes);
        }

        [TestMethod]
        public void Inspect_ReturnsTrusted_WhenFedExCertificateVerificationDataIsEmpty_Test()
        {
            credentialStore.Setup(s => s.FedExCertificateVerificationData).Returns(string.Empty);
            testObject = new FedExCertificateInspector(credentialStore.Object);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.AreEqual(CertificateSecurityLevel.Trusted, securityLevel);
        }
        
        [TestMethod]
        public void Inspect_ReturnsNone_WhenCertificateIsNull_Test()
        {
            request.Setup(r => r.Certificate).Returns<X509Certificate>(null);
            testObject = new FedExCertificateInspector(credentialStore.Object);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.AreEqual(CertificateSecurityLevel.None, securityLevel);
        }

        [TestMethod]
        public void Inspect_ReturnsSpoofed_WhenCertificateSubjectDoesNotMatchExpectedValues_Test()
        {
            credentialStore.Setup(s => s.FedExCertificateVerificationData).Returns("<Subject><Value>FedEx</Value></Subject>");
            request.Setup(r => r.Certificate).Returns(noMatchCertificate);
            testObject = new FedExCertificateInspector(credentialStore.Object);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.AreEqual(CertificateSecurityLevel.Spoofed, securityLevel);
        }

        [TestMethod]
        public void Inspect_ReturnsTrusted_WhenCertificateSubjectMatchesAllExpectedValues_WithSingleExpectedValue_Test()
        {
            credentialStore.Setup(s => s.FedExCertificateVerificationData).Returns("<Subject><Value>FedEx</Value></Subject>");
            request.Setup(r => r.Certificate).Returns(singleMatchCertificate);

            testObject = new FedExCertificateInspector(credentialStore.Object);
            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.AreEqual(CertificateSecurityLevel.Trusted, securityLevel);
        }

        [TestMethod]
        public void Inspect_ReturnsTrusted_WhenCertificateSubjectMatchesAllExpectedValues_WithMultipleExpectedValues_Test()
        {
            credentialStore.Setup(s => s.FedExCertificateVerificationData).Returns("<Subject><Value>FedEx</Value><Value>banana hammock</Value></Subject>");
            request.Setup(r => r.Certificate).Returns(multiMatchCertificate);

            testObject = new FedExCertificateInspector(credentialStore.Object);
            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.AreEqual(CertificateSecurityLevel.Trusted, securityLevel);
        }
    }
}
