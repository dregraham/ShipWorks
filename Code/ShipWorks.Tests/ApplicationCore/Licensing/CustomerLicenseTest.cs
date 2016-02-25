﻿using System.Xml;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseTest
    {
        [Fact]
        public void Constructor_NoErrorThrown_WhenFeatureRestrictionsHaveUniqueEditionFeatures()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature1 = mock1.Mock<IFeatureRestriction>();
                feature1.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IFeatureRestriction> feature2 = mock2.Mock<IFeatureRestriction>();
                feature2.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountNumber);

                var featureRestrictions = new List<IFeatureRestriction> {feature1.Object, feature2.Object};

                mock1.Create<CustomerLicense>(
                    new NamedParameter("key", "SomeKey"),
                    new TypedParameter(typeof (IEnumerable<IFeatureRestriction>), featureRestrictions));
            }
        }

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenFeatureRestrictionsHaveCommonEditionFeature()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature1 = mock1.Mock<IFeatureRestriction>();
                feature1.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IFeatureRestriction> feature2 = mock2.Mock<IFeatureRestriction>();
                feature2.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                var featureRestrictions = new List<IFeatureRestriction> {feature1.Object, feature2.Object};

                try
                {
                    mock1.Create<CustomerLicense>(
                        new NamedParameter("key", "SomeKey"),
                        new TypedParameter(typeof (IEnumerable<IFeatureRestriction>), featureRestrictions));
                }
                catch (Exception ex)
                {
                    Assert.IsType(typeof(InvalidOperationException), ex.InnerException);
                }
            }
        }
        

        [Fact]
        public void Refresh_DefersGettingLicenseCapabilitiesToTangoWebClient()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LicenseCapabilities licenseResponse = new LicenseCapabilities(new XmlDocument());

                var tangoWebClient =
                    mock.Mock<ITangoWebClient>();

                tangoWebClient.Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseResponse);

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                tangoWebClient.Verify(wc => wc.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()), Times.Once);
            }
        }

        [Fact]
        public void Refresh_SetsDisabledReasonFromLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                Assert.Equal("Disabled for some reason", customerLicense.DisabledReason);
            }
        }

        [Fact]
        public void Refresh_LogsDisabledReason_WhenTangoWebClientThrowsException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var log = mock.Mock<ILog>();

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                log.Verify(l => l.Warn(It.IsAny<TangoException>()), Times.Once);
            }
        }

        [Fact]
        public void Save_RethrowsExceptionFromLicenseWriter()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var writer = mock.Mock<ICustomerLicenseWriter>();

                writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Throws(new Exception("some random exception"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var ex = Assert.Throws<Exception>(() => customerLicense.Save());
                Assert.Equal("some random exception", ex.Message);
            }
        }

        [Fact]
        public void Activate_ReturnsActive_WhenTangoReturnsSuccess()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Active, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsInvalid_WhenTangoReturnsFailedResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("blah");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Invalid, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsOverChannelLimit_WhenTangoReturnsOverChannelLimitResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("OverChannelLimit");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.OverChannelLimit, result.Value);
            }
        }

        [Fact]
        public void EnforceCapabilitiesWithEditionFeature_CallsEnforceOnEnforcerWithMatchingEditionFeature()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                Mock<ILicenseEnforcer> enforcerTwo = mock2.Mock<ILicenseEnforcer>();
                enforcerTwo.SetupGet(e => e.EditionFeature).Returns(EditionFeature.EndiciaAccountLimit);
                enforcerTwo.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                        "enforcerTwo is not compliant."));

                Mock<ILicenseEnforcer> enforcerOne = mock1.Mock<ILicenseEnforcer>();
                enforcerOne.SetupGet(e => e.EditionFeature).Returns(EditionFeature.ChannelCount);
                enforcerOne.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                        "enforcerOne is not compliant."));

                CustomerLicense testObject = mock1.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"),
                    new TypedParameter(typeof (IEnumerable<ILicenseEnforcer>),
                        new[] {enforcerOne.Object, enforcerTwo.Object}));

                testObject.EnforceCapabilities(EditionFeature.ChannelCount, EnforcementContext.NotSpecified);

                enforcerOne.Verify(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()),
                    Times.Once);
                enforcerTwo.Verify(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()),
                    Times.Never);
            }
        }

        [Fact]
        public void EnforceCapabilitiesWithOwner_CallsEnforceeOnAllEnforcersWithOwner()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();
                Mock<ILicenseEnforcer> enforcerOne = mock.Mock<ILicenseEnforcer>();
                Mock<ILicenseEnforcer> enforcerTwo = mock.Mock<ILicenseEnforcer>();

                mock.Mock<ITangoWebClient>();
                mock.Provide(new List<ILicenseEnforcer> {enforcerOne.Object, enforcerTwo.Object});

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceCapabilities(EnforcementContext.NotSpecified, owner.Object);

                enforcerOne.Verify(
                    e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>(), owner.Object),
                    Times.Once);
                enforcerTwo.Verify(
                    e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>(), owner.Object),
                    Times.Once);
            }
        }

        [Fact]
        public void EnforceCapabilitiesWithOwner_RefreshesLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseEnforcer> enforcer = mock.Mock<ILicenseEnforcer>();
                Mock<ITangoWebClient> tangoWebClient = mock.Mock<ITangoWebClient>();
                mock.Provide(new List<ILicenseEnforcer> {enforcer.Object});

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceCapabilities(EnforcementContext.NotSpecified, null);

                tangoWebClient.Verify(w => w.GetLicenseCapabilities(testObject), Times.AtLeastOnce);
            }
        }

        [Fact]
        public void EnforceCapabilities_RefreshesLicense_WhenCapabilitiesIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseEnforcer> enforcer = mock.Mock<ILicenseEnforcer>();
                enforcer.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty));

                Mock<ITangoWebClient> tangoWebClient = mock.Mock<ITangoWebClient>();
                mock.Provide(new List<ILicenseEnforcer> {enforcer.Object});

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceCapabilities(EnforcementContext.NotSpecified);

                tangoWebClient.Verify(w => w.GetLicenseCapabilities(testObject), Times.Once);
            }
        }

        [Fact]
        public void EnforceCapabilities_CallsEnforceOnAllEnforcers()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseEnforcer> enforcerOne = mock.Mock<ILicenseEnforcer>();
                enforcerOne.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty));

                Mock<ILicenseEnforcer> enforcerTwo = mock.Mock<ILicenseEnforcer>();
                enforcerTwo.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty));

                mock.Mock<ITangoWebClient>();
                mock.Provide(new List<ILicenseEnforcer> {enforcerOne.Object, enforcerTwo.Object});

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceCapabilities(EnforcementContext.NotSpecified);

                enforcerOne.Verify(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()),
                    Times.Once);
                enforcerTwo.Verify(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()),
                    Times.Once);
            }
        }

        [Fact]
        public void EnforceCapabilities_ThrowsShipWorksLicenseExceptionWithFirstEnforcerError()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseEnforcer> enforcerOne = mock.Mock<ILicenseEnforcer>();
                enforcerOne.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty));

                Mock<ILicenseEnforcer> enforcerTwo = mock.Mock<ILicenseEnforcer>();
                enforcerTwo.Setup(e => e.Enforce(It.IsAny<ILicenseCapabilities>(), It.IsAny<EnforcementContext>()))
                    .Returns(new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                        "Something about not being compliant"));

                mock.Provide(new List<ILicenseEnforcer> {enforcerOne.Object, enforcerTwo.Object});

                mock.Mock<ITangoWebClient>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                ShipWorksLicenseException ex =
                    Assert.Throws<ShipWorksLicenseException>(
                        () => testObject.EnforceCapabilities(EnforcementContext.NotSpecified));

                Assert.Equal("Something about not being compliant", ex.Message);
            }
        }

        [Fact]
        public void CreateDashboardMessage_ReturnsDashboardLicenseItem_WhenOverShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(false);
                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                mock.Mock<ITangoWebClient>().Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                DashboardLicenseItem result = testObject.CreateDashboardMessage();

                Assert.NotNull(result);
            }
        }

        [Fact]
        public void CreateDashboardMessage_ReturnsDashboardLicenseItem_WhenAtShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(false);
                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(8);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                mock.Mock<ITangoWebClient>().Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                DashboardLicenseItem result = testObject.CreateDashboardMessage();

                Assert.NotNull(result);
            }
        }

        [Fact]
        public void CreateDashboardMessage_ReturnsNull_WhenUnderShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(false);
                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(7);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                mock.Mock<ITangoWebClient>().Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                DashboardLicenseItem result = testObject.CreateDashboardMessage();

                Assert.Null(result);
            }
        }

        [Fact]
        public void CreateDashboardMessage_ReturnsNull_WhenIsInTrialAndOverShipmentLimitWarningThreshold()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(l => l.IsInTrial).Returns(true);
                licenseCapabilities.Setup(l => l.ProcessedShipments).Returns(9);
                licenseCapabilities.Setup(l => l.ShipmentLimit).Returns(10);

                mock.Mock<ITangoWebClient>().Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                DashboardLicenseItem result = testObject.CreateDashboardMessage();

                Assert.Null(result);
            }
        }

        [Fact]
        public void CheckRestriction_DelegatesToFeatureRestriction()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.CheckRestriction(EditionFeature.EndiciaAccountLimit, null);

                feature.Verify(x=>x.Check(It.IsAny<ILicenseCapabilities>(), null));
            }
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.Forbidden)]
        [InlineData(EditionRestrictionLevel.Hidden)]
        public void CheckRestriction_ReturnsRestrictionLevelFromRestrictionFeature(EditionRestrictionLevel level)
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                feature.Setup(f => f.Check(It.IsAny<ILicenseCapabilities>(), It.IsAny<object>()))
                    .Returns(level);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var editionRestrictionLevel = testObject.CheckRestriction(EditionFeature.EndiciaAccountLimit, null);

                Assert.Equal(level, editionRestrictionLevel);
            }
        }

        [Fact]
        public void CheckRestriction_ReturnsNone_WhenCannotFindRestriction()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                feature.Setup(f => f.Check(It.IsAny<ILicenseCapabilities>(), It.IsAny<object>()))
                    .Returns(EditionRestrictionLevel.None);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var editionRestrictionLevel = testObject.CheckRestriction(EditionFeature.GenericFile, null);

                Assert.Equal(EditionRestrictionLevel.None, editionRestrictionLevel);
            }
        }

        [Fact]
        public void CheckRestriction_PassesDataIntoFeatureRestrictions()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IWin32Window> window = mock.Mock<IWin32Window>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.CheckRestriction(EditionFeature.EndiciaAccountLimit, "foo");

                feature.Verify(f => f.Check(It.IsAny<ILicenseCapabilities>(),"foo"));
            }
        }

        [Fact]
        public void HandleRestriction_DelegatesToCorrectFeatureRestriction()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IWin32Window> window = mock.Mock<IWin32Window>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.HandleRestriction(EditionFeature.EndiciaAccountLimit, null, window.Object);

                feature.Verify(f=>f.Handle(It.IsAny<IWin32Window>(), It.IsAny<ILicenseCapabilities>(), It.IsAny<object>()));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HandleRestriction_ReturnsValueFromFeatureRestrictionHandle(bool value)
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                feature.Setup(f => f.Handle(It.IsAny<IWin32Window>(), It.IsAny<ILicenseCapabilities>(), It.IsAny<object>()))
                    .Returns(value);

                Mock<IWin32Window> window = mock.Mock<IWin32Window>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                bool result = testObject.HandleRestriction(EditionFeature.EndiciaAccountLimit, null, window.Object);

                Assert.Equal(value,result);
            }
        }

        [Fact]
        public void HandleRestriction_ReturnsFalse_WhenCannotFindFeature()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IWin32Window> window = mock.Mock<IWin32Window>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                bool result = testObject.HandleRestriction(EditionFeature.AddOrderCustomer, null, window.Object);

                Assert.Equal(false, result);
            }
        }

        [Fact]
        public void HandleRestriction_PassesDataIntoFeatureRestrictions()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IFeatureRestriction> feature = mock.Mock<IFeatureRestriction>();
                feature.SetupGet(f => f.EditionFeature)
                    .Returns(EditionFeature.EndiciaAccountLimit);

                Mock<IWin32Window> window = mock.Mock<IWin32Window>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.HandleRestriction(EditionFeature.EndiciaAccountLimit, "foo", window.Object);

                feature.Verify(f => f.Handle(It.IsAny<IWin32Window>(), It.IsAny<ILicenseCapabilities>(), "foo"));
            }
        }
    }
}