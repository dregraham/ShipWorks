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
    public class FedExLtlFreightLabelSpecificationManipulatorTest : IDisposable
    {
        private FedExLtlFreightLabelSpecificationManipulator testObject;
        private readonly AutoMock mock;
        private ShippingSettingsEntity shippingSettings;
        private ShipmentEntity shipment;

        public FedExLtlFreightLabelSpecificationManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            IoC.InitializeForUnitTests(mock.Container);

            shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExThermalDocTab = false;
            shippingSettings.FedExMaskAccount = true;

            mock.Mock<IFedExSettingsRepository>().Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            // Initialize the shipment entity that we'll be testing with
            shipment = new ShipmentEntity()
            {
                ShipmentType = (int) ShipmentTypeCode.FedEx,
                FedEx = new FedExShipmentEntity(),
                ShipCountryCode = "US",
                OriginCountryCode = "US"
            };

            testObject = mock.Create<FedExLtlFreightLabelSpecificationManipulator>();
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, false)]
        [InlineData(FedExServiceType.FedEx1DayFreight, false)]
        [InlineData(FedExServiceType.FedExFreightEconomy, true)]
        [InlineData(FedExServiceType.FedExFreightPriority, true)]
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
        public void Manipulate_SpecifiesBillOfLading()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelFormatType.FEDEX_FREIGHT_STRAIGHT_BILL_OF_LADING, labelSpecification.LabelFormatType);
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
        public void Manipulate_SetsImageTypeToPng_ForImageLabel()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(ShippingDocumentImageType.PDF, labelSpecification.ImageType);
        }

        [Fact]
        public void Manipulate_SetsPaperLetter_ForImageLabel()
        {
            // Setup to generate an image label
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            LabelSpecification labelSpecification = result.Value.RequestedShipment.LabelSpecification;
            Assert.Equal(LabelStockType.PAPER_LETTER, labelSpecification.LabelStockType);
        }

        public void Dispose() => mock.Dispose();
    }
}
