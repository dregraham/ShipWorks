using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Settings;
using ShipWorks.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class ScanPackOrderValidatorTest
    {
        private readonly AutoMock mock;
        private readonly ScanPackOrderValidator testObject;
        private readonly Mock<ILicenseService> licenseService;
        private readonly Mock<ISingleScanAutomationSettings> singleScanAutomationSettings;
        private readonly OrderEntity order = new OrderEntity() { Verified = true };

        public ScanPackOrderValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            licenseService = mock.Mock<ILicenseService>();
            licenseService.Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.None);

            singleScanAutomationSettings = mock.Mock<ISingleScanAutomationSettings>();
            singleScanAutomationSettings.SetupGet(s => s.AutoPrintScanPackRequireValidation)
                .Returns(true);

            testObject = mock.Create<ScanPackOrderValidator>();
        }

        [Fact]
        public void CanProcessShipment_DelegatesToLicenseServiceCheckRestriction()
        {
            testObject.CanProcessShipment(order);

            licenseService.Verify(l => l.CheckRestriction(EditionFeature.Warehouse, null));
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.None, true, false, false)]
        [InlineData(EditionRestrictionLevel.None, true, true, true)]
        [InlineData(EditionRestrictionLevel.None, false, true, true)]
        [InlineData(EditionRestrictionLevel.None, false, false, true)]
        [InlineData(EditionRestrictionLevel.Forbidden, true, true, true)]
        public void CanProcessShipment_ReturnsExpectedResult(EditionRestrictionLevel editionRestrictionLevel, bool requireValidation, bool orderVerified, bool resultShouldBeSuccess)
        {
            licenseService.Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(editionRestrictionLevel);
            singleScanAutomationSettings.SetupGet(s => s.AutoPrintScanPackRequireValidation)
                .Returns(requireValidation);

            order.Verified = orderVerified;

            var result = testObject.CanProcessShipment(order);

            Assert.Equal(resultShouldBeSuccess, result.Success);
        }
    }
}