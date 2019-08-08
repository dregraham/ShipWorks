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
        private readonly Mock<IMainForm> mainForm;
        private readonly Mock<ISingleScanAutomationSettings> singleScanAutomationSettings;
        private readonly OrderEntity order = new OrderEntity() { Verified = true };

        public ScanPackOrderValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            licenseService = mock.Mock<ILicenseService>();
            mainForm = mock.Mock<IMainForm>();
            singleScanAutomationSettings = mock.Mock<ISingleScanAutomationSettings>();

            licenseService.Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.None);

            mainForm.SetupGet(m => m.UIMode).Returns(UIMode.OrderLookup);

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

        [Fact]
        public void CanProcessShipment_ReturnsSuccess_WhenUIModeIsNotOrderLookup()
        {
            mainForm.SetupGet(m => m.UIMode).Returns(UIMode.Batch);

            var result = testObject.CanProcessShipment(order);

            Assert.True(result.Success);
        }

        [Fact]
        public void CanProcessShipment_ReturnsError_WhenUIModeIsOrderLookupAndOrderIsNotVerified()
        {
            order.Verified = false;
            var result = testObject.CanProcessShipment(order);

            Assert.True(result.Failure);
        }

        [Fact]
        public void CanProcessShipment_ReturnsSuccess_WhenNotWarehouseCustomer()
        {
            licenseService.Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.Forbidden);

            order.Verified = false;
            var result = testObject.CanProcessShipment(order);

            Assert.True(result.Success);
        }

        [Fact]
        public void CanProcessShipment_ReturnsSuccess_WhenRequireValidationIsDisabled()
        {
            singleScanAutomationSettings.SetupGet(s => s.AutoPrintScanPackRequireValidation)
                .Returns(false);

            order.Verified = false;
            var result = testObject.CanProcessShipment(order);

            Assert.True(result.Success);
        }
    }
}