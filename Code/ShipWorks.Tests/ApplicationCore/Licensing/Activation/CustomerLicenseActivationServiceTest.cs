using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Activation
{
    public class CustomerLicenseActivationServiceTest
    {
        [Fact]
        public void Activate_DelegatesToCustomerLicenseActivationActivity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICustomerLicenseActivationActivity> licenseActivity = mock.Mock<ICustomerLicenseActivationActivity>();
                licenseActivity.Setup(a => a.Execute(It.IsAny<string>(), It.IsAny<string>())).Returns(mock.Mock<ICustomerLicense>().Object);

                Mock<IUspsAccountSetupActivity> uspsAccountActivity = mock.Mock<IUspsAccountSetupActivity>();
                uspsAccountActivity.Setup(a => a.Execute(It.IsAny<ICustomerLicense>(), It.IsAny<string>()));
                
                CustomerLicenseActivationService testObject = mock.Create<CustomerLicenseActivationService>();
                testObject.Activate("bob", "some password");

                licenseActivity.Verify(a => a.Execute("bob", "some password"), Times.Once());
            }
        }

        [Fact]
        public void Activate_DelegatesToUspsAccountSetupActivity_WithAssociatedStampsUserNameAndPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICustomerLicense> customerLicense = new Mock<ICustomerLicense>();
                customerLicense.Setup(cl => cl.AssociatedStampsUsername).Returns("stampsUserName");

                Mock<ICustomerLicenseActivationActivity> licenseActivity = mock.Mock<ICustomerLicenseActivationActivity>();
                licenseActivity.Setup(a => a.Execute(It.IsAny<string>(), It.IsAny<string>())).Returns(customerLicense.Object);

                Mock<IUspsAccountSetupActivity> uspsAccountActivity = mock.Mock<IUspsAccountSetupActivity>();
                uspsAccountActivity.Setup(a => a.Execute(It.IsAny<ICustomerLicense>(), It.IsAny<string>()));
                
                CustomerLicenseActivationService testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "some password");

                uspsAccountActivity.Verify(a => a.Execute(customerLicense.Object, "some password"), Times.Once());
            }
        }
    }
}