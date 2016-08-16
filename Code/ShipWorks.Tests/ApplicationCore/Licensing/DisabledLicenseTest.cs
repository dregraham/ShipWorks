using ShipWorks.ApplicationCore.Licensing;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class DisabledLicenseTest
    {
        [Fact]
        public void DisabledReason_SetInConstructor()
        {
            string reason = "my Reason";
            DisabledLicense testObject = new DisabledLicense(reason);

            Assert.Equal(reason, testObject.DisabledReason);
        }

        [Fact]
        public void Refresh_DoesNotThrow()
        {
            DisabledLicense testObject = new DisabledLicense(string.Empty);
            testObject.Refresh();
        }

        [Fact]
        public void IsDisabled_IsTrue()
        {
            DisabledLicense testObject = new DisabledLicense(string.Empty);

            Assert.True(testObject.IsDisabled);
        }

        [Fact]
        public void Key_ThrowsShipWorksLicenseException()
        {
            DisabledLicense testObject = new DisabledLicense(string.Empty);

            Assert.Throws<ShipWorksLicenseException>(() => testObject.Key);
        }
    }
}