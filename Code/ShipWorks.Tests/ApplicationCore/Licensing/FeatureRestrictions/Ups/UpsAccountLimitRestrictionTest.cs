﻿using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    public class UpsAccountLimitRestrictionTest : IDisposable
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
        [InlineData(UpsStatus.Tier1, 1, 100, EditionRestrictionLevel.Forbidden)]
        [InlineData(UpsStatus.Tier1, 1, 1, EditionRestrictionLevel.None)]
        [InlineData(UpsStatus.Tier1, 1, 0, EditionRestrictionLevel.None)]
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
        public void Handle_CallsMessageHelperShowError_WhenCheckReturnsForbidden()
        {
            var owner = mock.Mock<IWin32Window>();
            var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
            var messageHelper = mock.Mock<IMessageHelper>();
            licenseCapabilities.Setup(l => l.UpsStatus)
                .Returns(UpsStatus.Discount);

            testObject.Handle(owner.Object, licenseCapabilities.Object, 5);

            messageHelper.Verify(m => m.ShowError(owner.Object, EnumHelper.GetDescription(testObject.EditionFeature)), Times.Once);
        }
    }
}