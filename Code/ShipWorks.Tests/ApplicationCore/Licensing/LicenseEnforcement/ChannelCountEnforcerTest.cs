using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ChannelCountEnforcerTest
    {
        [Fact]
        public void Priortity_Returns_2()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Assert.Equal(2, testObject.Priortity);
            }
        }

        [Fact]
        public void EditionFeature_Returns_ChannelCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Assert.Equal(EditionFeature.ChannelCount, testObject.EditionFeature);
            }
        }
        
        [Fact]
        public void Enforce_WithActiveChannelsHigherThanChannelLimit_ReturnsNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(4);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WithActiveChannelsEqualToChannelLimit_ReturnsCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WithActiveChannelsLessThanChannelLimit_ReturnsCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WhenCompliant_ReturnsNoErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_WhenNotCompliant_ReturnsErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(8);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal("You have exceeded your channel limit. Please upgrade your plan or delete 3 channels to continue downloading orders and creating shipment labels.", result.Message);
            }
        }
        
        [Fact]
        public void Enforce_WhenNotCompliant_ShowsDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitDlgFactory> dlgFactory = mock.Mock<IChannelLimitDlgFactory>();
                Mock<IChannelLimitDlg> dlg = mock.Mock<IChannelLimitDlg>();

                dlgFactory.Setup(f => f.GetChannelLimitDlg(It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(1);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Enforce_WhenCompliant_DoesNotShowsDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitDlgFactory> dlgFactory = mock.Mock<IChannelLimitDlgFactory>();
                Mock<IChannelLimitDlg> dlg = mock.Mock<IChannelLimitDlg>();

                dlgFactory.Setup(f => f.GetChannelLimitDlg(It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }
    }
}