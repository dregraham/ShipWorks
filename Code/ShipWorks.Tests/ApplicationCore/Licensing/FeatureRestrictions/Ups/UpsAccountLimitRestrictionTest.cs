using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    public class UpsAccountLimitRestrictionTest
    {
        private readonly AutoMock mock;
        private readonly UpsAccountLimitRestriction testObject;

        public UpsAccountLimitRestrictionTest()
        {
            mock = AutoMock.GetLoose();
            testObject = mock.Create<UpsAccountLimitRestriction>();
            
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Theory]
        [InlineData(UpsStatus.None, 0, 100, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Tier1, 0, 100, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Tier2, 0, 100, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Tier3, 0, 100, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Subsidized, 10, 11, EditionRestrictionLevel.Forbidden)]
        [InlineData(UpsStatus.Discount, 10, 11, EditionRestrictionLevel.Forbidden)]
        [InlineData(UpsStatus.Subsidized, 10, 9, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Discount, 10, 9, EditionRestrictionLevel.None)]
        public void Check_ReturnsCorrectRestricitonLevel(
            UpsStatus upsStatus,
            int upsAccountLimit,
            int accountCount,
            EditionRestrictionLevel expectedResult)
        {
            var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(l => l.UpsStatus)
                .Returns(upsStatus);
            licenseCapabilities.Setup(l => l.UpsAccountLimit)
                .Returns(upsAccountLimit);

            var actualResult = testObject.Check(licenseCapabilities.Object, accountCount);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Handle_ReturnsTrue_WhenCheckReturnsNone()
        {
            var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(l => l.UpsStatus)
                .Returns(UpsStatus.None);

            var handleResult = testObject.Handle(null, licenseCapabilities.Object, 5);

            Assert.True(handleResult);
        }

        [Fact]
        public void Handle_ThrowsUpsException_WhenCheckReturnsForbidden()
        {
            var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            licenseCapabilities.Setup(l => l.UpsStatus)
                .Returns(UpsStatus.Discount);

            Assert.Throws<UpsException>(()=> testObject.Handle(null, licenseCapabilities.Object, 5));
        }

    }
}
