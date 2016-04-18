using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ExceedingChannelLimitEnforcerTest
    {
        [Fact]
        public void Enforce_ErrorReferencesWillExceedChannelLimit_WhenContextIsExceedingChannelLimitAndActiveChannelsEqualsChannelLimits()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingChannelLimitEnforcer testObject = mock.Create<ExceedingChannelLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.ExceedingChannelLimit);

                Assert.Contains("will exceed your channel limit", result.Message);
            }
        }

        [Fact]
        public void Enforce_ErrorReferencesWillExceedChannelLimit_WhenContextIsOnAddingStoreAndActiveChannelsEqualsChannelLimits()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingChannelLimitEnforcer testObject = mock.Create<ExceedingChannelLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.ExceedingChannelLimit);

                Assert.Contains("will exceed your channel limit", result.Message);
            }
        }

        [Fact]
        public void Enforce_Compliant_WhenContextIsExceedingChannelLimitAndActiveChannelsLessThanChannelLimits()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingChannelLimitEnforcer testObject = mock.Create<ExceedingChannelLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(6);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.ExceedingChannelLimit);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }
    }
}
