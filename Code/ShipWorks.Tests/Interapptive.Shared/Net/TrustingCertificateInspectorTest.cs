using System.Net;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ShipWorks.Tests.Interapptive.Shared.Net
{
    [TestClass]
    public class TrustingCertificateInspectorTest
    {
        private readonly TrustingCertificateInspector testObject;

        public TrustingCertificateInspectorTest()
        {
            testObject = new TrustingCertificateInspector();
        }

        [TestMethod]
        public void Inspect_ReturnsTrusted_WhenRequestIsNull_Test()
        {
            Assert.AreEqual(CertificateSecurityLevel.Trusted, testObject.Inspect(null));
        }

        [TestMethod]
        public void Inspect_ReturnsTrusted_WhenRequestIsNotNull_Test()
        {
            Mock<HttpWebRequest> request = new Mock<HttpWebRequest>();

            Assert.AreEqual(CertificateSecurityLevel.Trusted, testObject.Inspect(request.Object));
        }
    }
}
