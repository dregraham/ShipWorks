﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseFactoryTest
    {
        [Fact]
        public void GetLicense_ReturnsStoreLicense_WhenLegacy()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read())
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
                    .Setup(r => r.Read())
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
                    .Setup(r => r.Read())
                    .Returns("42");

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity());

                Assert.IsType(typeof (CustomerLicense), license);
            }
        }

        [Fact]
        public void GetLicense_CustomerLicenseContainsKey_WhenReaderReturnsKey()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read())
                    .Returns("42");

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense license = testObject.GetLicense(new StoreEntity());

                Assert.Equal("42", ((CustomerLicense) license).Key);
            }
        }

        [Fact]
        public void GetLicenses_ReturnsTwoStoreLicenses_WhenLegacyAndTwoStoresEnabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICustomerLicenseReader>()
                    .Setup(r => r.Read())
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
                    .Setup(r => r.Read())
                    .Returns("42");

                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetEnabledStores())
                    .Returns(Enumerable.Repeat(new StoreEntity(), 2));

                LicenseService testObject = mock.Create<LicenseService>();

                ILicense[] licenses = testObject.GetLicenses().ToArray();

                Assert.Equal(1, licenses.Count());
                Assert.IsType(typeof (CustomerLicense), licenses[0]);
            }
        }
    }
}
