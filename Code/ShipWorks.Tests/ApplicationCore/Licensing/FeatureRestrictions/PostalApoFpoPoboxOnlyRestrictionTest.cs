using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class PostalApoFpoPoboxOnlyRestrictionTest
    {
        [Fact]
        public void EditionFeature_ReturnsEditionFeaturePostalApoFpoPoboxOnly()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionFeature.PostalApoFpoPoboxOnly, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenDataIsNotShipment()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);
                
                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, null));
            }
        }

        [Fact]
        public void Check_ReturnsEditionRestrictionLevelNone_WhenShipmentIsNotPostal()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipmentType = (int) ShipmentTypeCode.iParcel
                };
                
                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }

        [Fact]
        public void Check_ReturnsEditionFeatureLevelNone_WhenShipmentIsMilitaryState()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipStateProvCode = "AA"
                };

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }

        [Fact]
        public void Check_ReturnsEditionFeatureLevelNone_WhenShipmentIsPOBox()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipStreet1 = "P.O. Box 123",
                    ShipStreet2 = "",
                    ShipStreet3 = ""
                };

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }

        [Fact]
        public void Check_ReturnsEditionFeatureLevelNone_WhenPostalAvailibilityIsAllServices()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.PostalAvailability).Returns(BrownPostalAvailability.AllServices);
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipmentType = (int) ShipmentTypeCode.Usps
                };

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.None, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }

        [Fact]
        public void Check_ReturnsEditionFeatureLevelForbidden_WhenPostalAvailibilityIsApoFpoPobox()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.PostalAvailability).Returns(BrownPostalAvailability.ApoFpoPobox);
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ShipStreet1 = "1 South Memorial Drive",
                    ShipStreet2 = "Suite 2000",
                    ShipStreet3 = ""
                };

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }

        [Fact]
        public void Check_ReturnsEditionFeatureLevelForbidden_WhenPostalAvailibilityNone()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.PostalAvailability).Returns(BrownPostalAvailability.None);
                Mock<IMessageHelper> messagehelper = mock.Mock<IMessageHelper>();

                var shipment = new ShipmentEntity()
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ShipStreet1 = "1 South Memorial Drive",
                    ShipStreet2 = "Suite 2000",
                    ShipStreet3 = ""
                };

                PostalApoFpoPoboxOnlyRestriction testObject = new PostalApoFpoPoboxOnlyRestriction(messagehelper.Object);

                Assert.Equal(EditionRestrictionLevel.Forbidden, testObject.Check(licenseCapabilities.Object, shipment));
            }
        }
    }
}