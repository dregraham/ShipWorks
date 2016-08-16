using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    public class AccountRegistrationRestrictionRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AccountRegistrationRestriction testObject;

        public AccountRegistrationRestrictionRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<AccountRegistrationRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsShipmentTypeRegistration()
        {
            Assert.Equal(EditionFeature.ShipmentTypeRegistration, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenAccountRegistrationRestrictionIsRestrictedForTheGivenShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.AccountRegistration }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void Check_ReturnsNone_WhenAccountRegistrationRestrictionIsNotRestrictedForTheGivenShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Purchasing}
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void Check_ReturnsNone_WhenDataIsNotShipmentTypeCode()
        {
            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(mock.Mock<ILicenseCapabilities>().Object, null));
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsNone()
        {
            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(mock.Mock<ILicenseCapabilities>().Object, ShipmentTypeCode.None));
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenAccountRegistrationRestrictionIsRestrictedForTheGivenShipmentType_AndNotRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Purchasing }
                },
                {
                    ShipmentTypeCode.FedEx,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.AccountRegistration }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.FedEx));
        }
        [Fact]
        public void Check_ReturnsNone_WhenAccountRegistrationRestrictionIsNotRestrictedForTheGivenShipmentType_AndIsRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.AccountRegistration }
                },
                {
                    ShipmentTypeCode.FedEx,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Purchasing }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.FedEx));
        }
    }
}