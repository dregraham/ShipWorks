﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseServiceTest
    {
        [Fact]
        public void InitializeForCurrentDatabase_ForcesCustomerKeyRefresh()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICustomerLicenseReader> customerLicenseReader = mock.Mock<ICustomerLicenseReader>();
                customerLicenseReader.Setup(r => r.Read(CustomerLicenseKeyType.WebReg)).Returns("");
                customerLicenseReader.Setup(r => r.Read(CustomerLicenseKeyType.Legacy)).Returns("legacy key");

                Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();

                // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
                Mock<Func<string, ICustomerLicense>> repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>())).Returns(customerLicense.Object);
                mock.Provide(repo.Object);

                LicenseService testObject = mock.Create<LicenseService>();

                // Get customer key cached
                testObject.AllowsLogOn();

                // Reset calls back to 0
                customerLicenseReader.ResetCalls();
                testObject.AllowsLogOn();
                customerLicenseReader.Verify(l => l.Read(CustomerLicenseKeyType.WebReg), Times.Never);

                // Reset calls to 0
                customerLicenseReader.ResetCalls();

                // Clear the cache
                testObject.InitializeForCurrentDatabase(new UserInterfaceExecutionMode(null));
                testObject.AllowsLogOn();

                // Make sure we read once.
                customerLicenseReader.Verify(l => l.Read(CustomerLicenseKeyType.WebReg), Times.Once);
            }
        }

        [Fact]
        public void AllowsLogOn_ForcesLicenseRefresh()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("foo");

                Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();

                // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
                Mock<Func<string, ICustomerLicense>> repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>())).Returns(customerLicense.Object);
                mock.Provide(repo.Object);

                LicenseService testObject = mock.Create<LicenseService>();
                testObject.AllowsLogOn();

                customerLicense.Verify(l => l.ForceRefresh(), Times.Once);
            }
        }

        [Fact]
        public void AllowsLogOn_Throws_WhenLicenseRefreshThrows()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("foo");

                Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();
                customerLicense.Setup(l => l.ForceRefresh())
                    .Throws(new Exception("something went wrong"));


                // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
                Mock<Func<string, ICustomerLicense>> repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>()))
                    .Returns(customerLicense.Object);
                mock.Provide(repo.Object);

                LicenseService testObject = mock.Create<LicenseService>();

                Exception ex = Assert.Throws<Exception>(() => testObject.AllowsLogOn());
                Assert.Equal("something went wrong", ex.Message);
            }
        }

        [Fact]
        public void GetLicense_ReturnsStoreLicense_WhenLegacy()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns(string.Empty);

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity() {License = "40"});

                Assert.IsType(typeof (StoreLicense), license);
            }
        }

        [Fact]
        public void GetLicense_ReturnsStoreLicense_WhenCustomerLicenseReturnsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns((string) null);

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity {License = "40"});

                Assert.IsType(typeof (StoreLicense), license);
            }
        }

        [Fact]
        public void GetLicense_ReturnsCustomerLicense_WhenNotLegacy()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("42");

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity());

                Assert.True(license is ICustomerLicense);
            }
        }

        [Fact]
        public void GetLicense_CustomerLicenseContainsKey_WhenReaderReturnsKey()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("42");

                Func<string, ICustomerLicense> customerLicenseFactory = s =>
                {
                    Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();
                    customerLicense
                        .Setup(l => l.Key)
                        .Returns("42");

                    return customerLicense.Object;
                };

                LicenseService testObject = mock.Create<LicenseService>(new NamedParameter("customerLicenseFactory", customerLicenseFactory));

                ILicense license = testObject.GetLicense(new StoreEntity());

                Assert.Equal("42", license.Key);
            }
        }

        [Fact]
        public void GetLicense_ReturnsDisabledLicense_WhenReaderThrowsLicenseException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Throws<ShipWorksLicenseException>();

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity());

                Assert.IsType<DisabledLicense>(license);
            }
        }

        [Fact]
        public void GetLicenses_ReturnsTwoStoreLicenses_WhenLegacyAndTwoStoresEnabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns(string.Empty);

                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetEnabledStores())
                    .Returns(Enumerable.Repeat(new StoreEntity() {License = "42"}, 2));

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense[] licenses = testObject.GetLicenses().ToArray();

                Assert.Equal(2, licenses.Count());
                Assert.IsType(typeof (StoreLicense), licenses[0]);
                Assert.IsType(typeof (StoreLicense), licenses[1]);
            }
        }

        [Fact]
        public void GetLicenses_ReturnsCustomerLicense_WhenNotLegacyAndMultipleStores()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("42");

                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetEnabledStores())
                    .Returns(Enumerable.Repeat(new StoreEntity(), 2));

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense[] licenses = testObject.GetLicenses().ToArray();

                Assert.Equal(1, licenses.Count());
                Assert.True(licenses[0] is ICustomerLicense);
            }
        }

        [Fact]
        public void GetLicenses_ReturnsDisabledLicense_WhenReaderThrowsLicenseException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Throws<ShipWorksLicenseException>();

                LicenseService testObject = mock.Create<LicenseService>();

                IEnumerable<ILicense> licenses = testObject.GetLicenses().ToArray();

                Assert.Equal(1, licenses.Count());
                Assert.IsType<DisabledLicense>(licenses.Single());
            }
        }

        [Fact]
        public void AllowsLogOn_ReturnsNone_WhenCustomerIsOnLegacyPricing()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // null customer key makes it legacy
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns((string) null);

                LicenseService testObject = mock.Create<LicenseService>();

                EnumResult<LogOnRestrictionLevel> allowsLogOn = testObject.AllowsLogOn();

                Assert.Equal(LogOnRestrictionLevel.None, allowsLogOn.Value);
            }
        }

        [Fact]
        public void AllowsLogOn_ReturnsNone_WhenCustomerLicenseNotDisabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // null customer key makes it legacy
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("42");

                mock.Mock<ICustomerLicense>(new NamedParameter("key", "someKey"))
                    .Setup(l => l.IsDisabled)
                    .Returns(false);

                LicenseService testObject = mock.Create<LicenseService>();

                EnumResult<LogOnRestrictionLevel> allowsLogOn = testObject.AllowsLogOn();

                Assert.Equal(LogOnRestrictionLevel.None, allowsLogOn.Value);
            }
        }

        [Fact]
        public void AllowsLogOn_ReturnsForbidden_WhenCustomerLicenseDisabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // null customer key makes it legacy
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read(CustomerLicenseKeyType.WebReg))
                    .Returns("42");

                mock.Mock<ICustomerLicense>(new NamedParameter("key", "someKey"))
                    .Setup(l => l.IsDisabled)
                    .Returns(true);

                LicenseService testObject = mock.Create<LicenseService>();

                EnumResult<LogOnRestrictionLevel> allowsLogOn = testObject.AllowsLogOn();

                Assert.Equal(LogOnRestrictionLevel.Forbidden, allowsLogOn.Value);
            }
        }
    }
}
