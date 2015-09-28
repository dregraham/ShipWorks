using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.Net;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Tests.Interapptive.Shared.Net
{
    public class CertificateInspectorTest
    {
        private CertificateInspector testObject;

        private const string certificateVerificationData = "<Service><Subject><Value>FedEx</Value></Subject></Service>";
        private Mock<ICertificateRequest> request;

        private X509Certificate noMatchCertificate;
        private X509Certificate singleMatchCertificate;
        private X509Certificate multiMatchCertificate;

        public CertificateInspectorTest()
        {
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

        [Fact]
        public void Inspect_ReturnsTrusted_WhenFedExCertificateVerificationDataIsEmpty_Test()
        {
            testObject = new CertificateInspector(string.Empty);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.Equal(CertificateSecurityLevel.Spoofed, securityLevel);
        }
        
        [Fact]
        public void Inspect_ReturnsNone_WhenCertificateIsNull_Test()
        {
            request.Setup(r => r.Certificate).Returns<X509Certificate>(null);
            testObject = new CertificateInspector(certificateVerificationData);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.Equal(CertificateSecurityLevel.None, securityLevel);
        }

        [Fact]
        public void Inspect_ReturnsSpoofed_WhenCertificateSubjectDoesNotMatchExpectedValues_Test()
        {
            request.Setup(r => r.Certificate).Returns(noMatchCertificate);
            testObject = new CertificateInspector(certificateVerificationData);

            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.Equal(CertificateSecurityLevel.Spoofed, securityLevel);
        }

        [Fact]
        public void Inspect_ReturnsTrusted_WhenCertificateSubjectMatchesAllExpectedValues_WithSingleExpectedValue_Test()
        {
            request.Setup(r => r.Certificate).Returns(singleMatchCertificate);

            testObject = new CertificateInspector(certificateVerificationData);
            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.Equal(CertificateSecurityLevel.Trusted, securityLevel);
        }

        [Fact]
        public void Inspect_ReturnsTrusted_WhenCertificateSubjectMatchesAllExpectedValues_WithMultipleExpectedValues_Test()
        {
            request.Setup(r => r.Certificate).Returns(multiMatchCertificate);

            testObject = new CertificateInspector("<Service><Subject><Value>FedEx</Value><Value>banana hammock</Value></Subject></Service>");
            CertificateSecurityLevel securityLevel = testObject.Inspect(request.Object);

            Assert.Equal(CertificateSecurityLevel.Trusted, securityLevel);
        }
    }
}
