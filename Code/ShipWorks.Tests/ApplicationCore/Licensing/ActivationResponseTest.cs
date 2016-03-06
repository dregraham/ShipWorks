using System;
using System.Xml;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.ApplicationCore.Licensing.Activation.WebServices;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class ActivationResponseTest
    {
        [Fact]
        public void ActivationResponse_WithNull_XmlDocument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActivationResponse(null));
        }

        [Fact]
        public void ActivationResponse_SetsKey_FromXmlDocument()
        {
            CustomerLicenseInfoV1 licenseInfo = new CustomerLicenseInfoV1()
            {
                CustomerLicenseKey = "blah"
            };

            ActivationResponse activationResponse = new ActivationResponse(licenseInfo);

            Assert.Equal("blah", activationResponse.Key);
        }

        [Fact]
        public void ActivationResponse_SetsAssociatedStampsUserName_FromXmlDocument()
        {
            CustomerLicenseInfoV1 licenseInfo = new CustomerLicenseInfoV1()
            {
                AssociatedStampsUserName = "bob"
            };

            ActivationResponse activationResponse = new ActivationResponse(licenseInfo);

            Assert.Equal("bob", activationResponse.AssociatedStampsUserName);
        }
    }
}