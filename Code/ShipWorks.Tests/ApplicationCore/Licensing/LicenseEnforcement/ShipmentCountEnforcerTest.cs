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
    public class ShipmentCountEnforcerTest
    {
        [Fact]
        public void Priortity_Returns_1()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Assert.Equal(EnforcerPriority.High, testObject.Priortity);
            }
        }

        [Fact]
        public void EditionFeature_Returns_ShipmentCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Assert.Equal(EditionFeature.ShipmentCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_WhenContextLogin_ReturnsCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                EnumResult<ComplianceLevel> result = testObject.Enforce(null, EnforcementContext.Login);
                
                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WithProcessedShipmentsHigherThanShipmentLimit_ReturnsNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(4);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WithProcessedShipmentsEqualToShipmentLimit_ReturnsNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WithProcessedShipmentsLessThanShipmentLimit_ReturnsCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(4);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_WhenCompliant_ReturnsNoErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(3);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_WhenNotCompliant_ReturnsErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal("You have reached your shipment limit for this billing cycle. Please upgrade your plan to create labels.", result.Message);
            }
        }

        [Fact]
        public void Enforce_WhenNotCompliant_ShowsDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<IWin32Window>())).Returns(dlg.Object);
                
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Enforce_WhenCompliant_DoesNotShowsDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(3);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Enforce_WhenNotCompliant_CallsCreateOnDialogFactory_WithErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDialog> dlg = mock.Mock<IDialog>();
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                dlgFactory.Setup(c => c.Create(It.IsAny<string>(), null)).Returns(dlg.Object);
                
                ShipmentCountEnforcer testObject = mock.Create<ShipmentCountEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlgFactory.Verify(d => d.Create("You have reached your shipment limit for this billing cycle. Please upgrade your plan to create labels.", null), Times.Once);
            }
        }
    }
}