using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class PurchasePostageRestrictionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly PurchasePostageRestriction testObject;

        public PurchasePostageRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<PurchasePostageRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsProcessShipment()
        {
            Assert.Equal(EditionFeature.PurchasePostage, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenPurchasePostageIsRestrictedForTheGivenShipmentType()
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

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void Check_ReturnsNone_WhenPurchasePostageIsNotRestrictedForTheGivenShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion}
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
        public void Check_ReturnsNone_WhenGivenShipmentTypeIsNone()
        {
            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(mock.Mock<ILicenseCapabilities>().Object, ShipmentTypeCode.None));
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenPurchasePostageIsRestrictedForTheGivenShipmentTypeButNotRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Purchasing }
                },
                {
                    ShipmentTypeCode.FedEx,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }
        [Fact]
        public void Check_ReturnsNone_WhenPurchasePostageIsNotRestrictedForTheGivenShipmentTypeButIsRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion }
                },
                {
                    ShipmentTypeCode.FedEx,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Purchasing }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }
    }
}