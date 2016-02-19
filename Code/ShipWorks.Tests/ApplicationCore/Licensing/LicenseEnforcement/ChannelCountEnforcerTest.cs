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
        public void Priority_ReturnsMedium()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Assert.Equal(EnforcementPriority.Medium, testObject.Priority);
            }
        }

        [Fact]
        public void EditionFeature_ReturnsChannelCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Assert.Equal(EditionFeature.ChannelCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_ReturnsNotCompliant_WhenActiveChannelsHigherThanChannelLimit()
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
        public void Enforce_ReturnsCompliant_WhenActiveChannelsEqualToChannelLimit()
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
        public void Enforce_ReturnsCompliant_WhenActiveChannelsLessThanChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(4);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsNoErrorMessage_WhenCompliant()
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
        public void Enforce_ReturnsErrorMessage_WhenNotCompliant()
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
        public void Enforce_ShowsDialog_WhenNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitDlgFactory> dlgFactory = mock.Mock<IChannelLimitDlgFactory>();
                Mock<IChannelLimitDlg> dlg = mock.Mock<IChannelLimitDlg>();

                dlgFactory.Setup(f => f.GetChannelLimitDlg(It.IsAny<IWin32Window>(), It.IsAny<EditionFeature>())).Returns(dlg.Object);

                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(1);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Enforce_DoesNotShowsDialog_WhenCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitDlgFactory> dlgFactory = mock.Mock<IChannelLimitDlgFactory>();
                Mock<IChannelLimitDlg> dlg = mock.Mock<IChannelLimitDlg>();

                dlgFactory.Setup(f => f.GetChannelLimitDlg(It.IsAny<IWin32Window>(), It.IsAny<EditionFeature>())).Returns(dlg.Object);

                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Enforce_ErrorReferencesAddingANewStore_WhenContextIsOnAddingStoreAndNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(6);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.OnAddingStore);

                Assert.Contains("adding a new store", result.Message);
            }
        }

        [Fact]
        public void Enforce_ErrorReferencesAddingANewStore_WhenContextIsExceedingChannelLimitErrorAndNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(6);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.ExceedingChannelLimit);

                Assert.Contains("adding a new store", result.Message);
            }
        }

        [Fact]
        public void Enforce_ErrorReferencesWillExceedChannelLimit_WhenContextIsAddingStoreOverLimitErrorThrownAndActiveChannelsEqualsChannelLimits()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ChannelCountEnforcer testObject = mock.Create<ChannelCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(5);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.ExceedingChannelLimit);

                Assert.Contains("will exceed your channel limit", result.Message);
            }
        }
    }
}