using System;
using System.Windows;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using ShipWorks.UI;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ApproachingShipmentLimitEnforcerTest
    {
        [Fact]
        public void Priority_ReturnsLow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Assert.Equal(EnforcementPriority.Low, testObject.Priority);
            }
        }

        [Fact]
        public void EditionFeature_ReturnsShipmentCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Assert.Equal(EditionFeature.ShipmentCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliantWithWarningMessage_WhenOverShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);
                licenseCapabilities.Setup(l => l.BillingEndDate).Returns(DateTime.Parse("2/22/2016"));

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
                Assert.Equal("You are nearing your shipment limit for the current billing cycle ending 2/22.", result.Message);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliantWithWarningMessage_WhenAtShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(8);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);
                licenseCapabilities.Setup(l => l.BillingEndDate).Returns(DateTime.Parse("2/22/2016"));

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
                Assert.Equal("You are nearing your shipment limit for the current billing cycle ending 2/22.", result.Message);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliantWithNoMessage_WhenUnderShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(7);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliantWithNoMessage_WhenIsInTrialAndOverShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(true);
                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliantWithEmptyMessage_WhenUnderShipmentLimitIsNegativeOne()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(7);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(-1);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_ShowsBrowserDialogWithShipmentLimitWarningContent_WhenContextIsLoginAndOverShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWebBrowserFactory> dlgFactory = mock.Mock<IWebBrowserFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<IWin32Window>(), It.IsAny<Size>())).Returns(dlg.Object);

                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login, null);

                dlg.Verify(d => d.ShowDialog(), Times.Once);
                dlgFactory.Verify(d =>
                    d.Create(new Uri("https://www.interapptive.com/shipworks/notifications/shipment-limit/approaching/259854_ShipWorks_Nudge_ShipmentLimit_Approching.html"),
                    "Approaching Shipment Limit",
                    (IWin32Window) null,
                    new Size(1000, 1000)),
                    Times.Once);
            }
        }

        [Fact]
        public void Enforce_DoesNotShowsDialog_WhenContextIsNotLogin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Enforce_DoesNotShowsDialog_WhenUnderShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ApproachingShipmentLimitEnforcer testObject = mock.Create<ApproachingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(7);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.Login, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }
    }
}