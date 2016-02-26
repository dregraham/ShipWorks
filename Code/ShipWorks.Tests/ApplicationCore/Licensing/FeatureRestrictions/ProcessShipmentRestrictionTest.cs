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
    public class ProcessShipmentRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly ProcessShipmentRestriction testObject;

        public ProcessShipmentRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<ProcessShipmentRestriction>();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void EditionFeature_IsProcessShipment()
        {
            Assert.Equal(EditionFeature.ProcessShipment, testObject.EditionFeature);
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenProcessShipmentIsRestrictedForTheGivenShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Processing}
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void Check_ReturnsNone_WhenProcessShipmentIsNotRestrictedForTheGivenShipmentType()
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
        public void Check_ReturnsForbidden_WhenProcessShipmentIsRestrictedForTheGivenShipmentTypeButNotRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Processing}
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
        public void Check_ReturnsNone_WhenProcessShipmentIsNotRestrictedForTheGivenShipmentTypeButIsRestrictedForAnotherShipmentType()
        {
            var restrictions = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
            {
                {
                    ShipmentTypeCode.Amazon,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion }
                },
                {
                    ShipmentTypeCode.FedEx,
                    new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Processing }
                }
            };

            Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(c => c.ShipmentTypeRestriction)
                .Returns(restrictions);

            Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Amazon));
        }
    }
}