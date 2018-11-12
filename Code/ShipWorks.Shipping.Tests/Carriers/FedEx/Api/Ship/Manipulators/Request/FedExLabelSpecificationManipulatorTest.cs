using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    [Collection(TestCollections.IoC)]
    public class FedExLabelSpecificationManipulatorTest : IDisposable
    {
        private FedExLabelSpecificationManipulator testObject;
        private readonly AutoMock mock;
        private ShippingSettingsEntity shippingSettings;
        private ShipmentEntity shipment;

        public FedExLabelSpecificationManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            IoC.InitializeForUnitTests(mock.Container);

            shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExThermalDocTab = true;
            shippingSettings.FedExMaskAccount = true;

            mock.Mock<IFedExSettingsRepository>().Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            // Initialize the shipment entity that we'll be testing with
            shipment = new ShipmentEntity()
            {
                RequestedLabelFormat = (int) ThermalLanguage.EPL,
                ActualLabelFormat = (int) ThermalLanguage.EPL,
                ShipmentType = (int) ShipmentTypeCode.FedEx,
                FedEx = new FedExShipmentEntity()
                {
                    Shipment = new ShipmentEntity()
                },
                ShipCountryCode = "US",
                OriginCountryCode = "US"
            };
            testObject = mock.Create<FedExLabelSpecificationManipulator>();
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, true)]
        [InlineData(FedExServiceType.FedEx1DayFreight, true)]
        [InlineData(FedExServiceType.FedExFreightEconomy, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, false)]
        public void ShouldApply_ReturnsCorrectValue(FedExServiceType serviceType, bool expectedValue)
        {
            shipment.FedEx.Service = (int) serviceType;

            Assert.Equal(expectedValue, testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository()
        {
            testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            mock.Mock<IFedExSettingsRepository>().Verify(r => r.GetShippingSettings(), Times.AtLeastOnce());
        }

        [Fact]
        public void Manipulate_SpecifiesCommon2DFormat()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelFormatType.COMMON2D, labelSpecification.LabelFormatType);
        }

        [Fact]
        public void Manipulate_MasksAccountNumber_WhenFedExMaskAccountSettingIsTrue_AndShipmentIsDomestic()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            CustomerSpecifiedLabelDetail labelDetail = labelSpecification.CustomerSpecifiedDetail;

            Assert.Equal(1, labelDetail.MaskedData.Length);
            Assert.Equal(LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[0]);
        }

        [Fact]
        public void Manipulate_MasksAccountNumber_WhenFedExMaskAccountSettingIsTrue_AndShipmentIsInternational()
        {
            shipment.ShipCountryCode = "UK";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            CustomerSpecifiedLabelDetail labelDetail = labelSpecification.CustomerSpecifiedDetail;

            Assert.Equal(4, labelDetail.MaskedData.Length);
            Assert.Equal(LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[0]);
            Assert.Equal(LabelMaskableDataType.TRANSPORTATION_CHARGES_PAYOR_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[1]);
            Assert.Equal(LabelMaskableDataType.DUTIES_AND_TAXES_PAYOR_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[2]);
            Assert.Equal(LabelMaskableDataType.CUSTOMS_VALUE, labelSpecification.CustomerSpecifiedDetail.MaskedData[3]);
        }

        [Fact]
        public void Manipulate_ReturnsFailure_WhenUnknownThermalType()
        {
            shipment.RequestedLabelFormat = 3;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithoutDocTab()
        {
            shippingSettings.FedExThermalDocTab = false;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelStockType.STOCK_4X6, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithDocTabAndLeadingTabType()
        {
            shippingSettings.FedExThermalDocTabType = (int) ThermalDocTabType.Leading;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelStockType.STOCK_4X675_LEADING_DOC_TAB, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithDocTabAndTrailingTabType()
        {
            shippingSettings.FedExThermalDocTabType = (int) ThermalDocTabType.Trailing;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelStockType.STOCK_4X675_TRAILING_DOC_TAB, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsImageTypeToPng_ForImageLabel()
        {
            // Setup to generate an image label
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(ShippingDocumentImageType.PNG, labelSpecification.ImageType);
        }

        [Fact]
        public void Manipulate_SetsPaper4x6StockLabel_ForImageLabel()
        {
            // Setup to generate an image label
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelStockType.PAPER_4X6, labelSpecification.LabelStockType);
        }

        public void Dispose() => mock.Dispose();
    }
}
