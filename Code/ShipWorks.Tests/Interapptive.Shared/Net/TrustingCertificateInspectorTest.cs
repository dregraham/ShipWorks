using System.Net;
using Interapptive.Shared.Net;
using Xunit;
using Moq;

namespace ShipWorks.Tests.Interapptive.Shared.Net
{
    public class TrustingCertificateInspectorTest
    {
        private readonly TrustingCertificateInspector testObject;

        public TrustingCertificateInspectorTest()
        {
            testObject = new TrustingCertificateInspector();
        }

        [Fact]
        public void Inspect_ReturnsTrusted_WhenRequestIsNull()
        {
            Assert.Equal(CertificateSecurityLevel.Trusted, testObject.Inspect(null));
        }

        [Fact]
        public void Inspect_ReturnsTrusted_WhenRequestIsNotNull()
        {
            Mock<ICertificateRequest> request = new Mock<ICertificateRequest>();

            Assert.Equal(CertificateSecurityLevel.Trusted, testObject.Inspect(request.Object));
        }
    }
}
