﻿using System;
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
    public class ExceedingShipmentLimitEnforcerTest
    {
        [Fact]
        public void Priority_ReturnsHigh()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Assert.Equal(EnforcementPriority.High, testObject.Priority);
            }
        }

        [Fact]
        public void EditionFeature_ReturnsShipmentCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Assert.Equal(EditionFeature.ShipmentCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliant_WhenContextLogin()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                EnumResult<ComplianceLevel> result = testObject.Enforce(null, EnforcementContext.Login);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsNotCompliant_WhenProcessedShipmentsHigherThanShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(4);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsNotCompliant_WhenProcessedShipmentsEqualToShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.NotCompliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliant_WhenProcessedShipmentsLessThanShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(4);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliant_WhenShipmentLimitIsNegativeOne()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(4);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(-1);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnsNoErrorMessage_WhenCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(3);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(string.Empty, result.Message);
            }
        }

        [Fact]
        public void Enforce_ReturnsErrorMessage_WhenNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal("You have reached your shipment limit for this billing cycle. Please upgrade your plan to create labels.", result.Message);
            }
        }

        [Fact]
        public void Enforce_ShowsDialog_WhenNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWebBrowserFactory> dlgFactory = mock.Mock<IWebBrowserFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<IWin32Window>(), It.IsAny<Size>())).Returns(dlg.Object);

                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Enforce_DoesNotShowsDialog_WhenCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IUpgradePlanDlgFactory> dlgFactory = mock.Mock<IUpgradePlanDlgFactory>();
                Mock<IDialog> dlg = mock.Mock<IDialog>();

                dlgFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<IWin32Window>())).Returns(dlg.Object);

                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(3);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Enforce_CallsCreateOnWebBrowserFactoryWithErrorMessage_WhenNotCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDialog> dlg = mock.Mock<IDialog>();
                Mock<IWebBrowserFactory> dlgFactory = mock.Mock<IWebBrowserFactory>();
                dlgFactory.Setup(c => c.Create(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<IWin32Window>(), It.IsAny<Size>())).Returns(dlg.Object);

                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(5);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel, null);

                dlgFactory.Verify(d => d.Create(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<IWin32Window>(), It.IsAny<Size>()), Times.Once);
            }
        }

        [Fact]
        public void Enforce_ReturnsCompliant_WhenOverLimitsAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ExceedingShipmentLimitEnforcer testObject = mock.Create<ExceedingShipmentLimitEnforcer>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(10);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(5);

                licenseCapabilities.Setup(l => l.ActiveChannels).Returns(10);
                licenseCapabilities.Setup(l => l.ChannelLimit).Returns(5);

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(true);

                EnumResult<ComplianceLevel> result = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.CreateLabel);

                Assert.Equal(ComplianceLevel.Compliant, result.Value);
            }
        }
    }
}