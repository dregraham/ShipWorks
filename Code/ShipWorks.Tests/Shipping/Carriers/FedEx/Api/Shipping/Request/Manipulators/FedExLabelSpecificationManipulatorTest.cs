using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExLabelSpecificationManipulatorTest
    {
        private FedExLabelSpecificationManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity();
            shippingSettings.FedExThermalDocTab = true;
            shippingSettings.FedExMaskAccount = true;

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            // Initialize the shipment entity that we'll be testing with
            shipmentEntity = new ShipmentEntity()
            {
                RequestedLabelFormat = (int) ThermalLanguage.EPL,
                ActualLabelFormat = (int)ThermalLanguage.EPL,
                FedEx = new FedExShipmentEntity()
                {
                    Shipment = new ShipmentEntity()
                },
                ShipCountryCode = "US",
                OriginCountryCode = "US"
            };

            // Configure the native request that the carrier request is representing
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    LabelSpecification = new LabelSpecification()
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExLabelSpecificationManipulator(settingsRepository.Object);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullLabelSpecification_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment.LabelSpecification = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.LabelSpecification);
        }

        [Fact]
        public void Manipulate_AccountsForNullFedExEntity_Test()
        {
            // Setup the test by configuring the shipment entity to have a null FedEx property and re-initialize
            // the carrier request with the updated native request
            shipmentEntity.FedEx = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The FedEx property should be created now
            Assert.IsNotNull(shipmentEntity.FedEx);
        }

        [Fact]
        public void Manipulate_AccountsForNullFedExShipment_Test()
        {
            // Setup the test by configuring the shipment entity to have a null FedEx.Shipment property and re-initialize
            // the carrier request with the updated native request
            shipmentEntity.FedEx.Shipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The FedEx property should be created now
            Assert.IsNotNull(shipmentEntity.FedEx.Shipment);
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            settingsRepository.Verify(r => r.GetShippingSettings(), Times.AtLeastOnce());
        }

        [Fact]
        public void Manipulate_SpecifiesCommon2DFormat_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(LabelFormatType.COMMON2D, labelSpecification.LabelFormatType);
        }

        [Fact]
        public void Manipulate_MasksAccountNumber_WhenFedExMaskAccountSettingIsTrue_AndShipmentIsDomestic_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            CustomerSpecifiedLabelDetail labelDetail = labelSpecification.CustomerSpecifiedDetail;

            Assert.AreEqual(1, labelDetail.MaskedData.Length);
            Assert.AreEqual(LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[0]);
        }

        [Fact]
        public void Manipulate_MasksAccountNumber_WhenFedExMaskAccountSettingIsTrue_AndShipmentIsInternational_Test()
        {
            shipmentEntity.ShipCountryCode = "UK";

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            CustomerSpecifiedLabelDetail labelDetail = labelSpecification.CustomerSpecifiedDetail;

            Assert.AreEqual(4, labelDetail.MaskedData.Length);
            Assert.AreEqual(LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[0]);
            Assert.AreEqual(LabelMaskableDataType.TRANSPORTATION_CHARGES_PAYOR_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[1]);
            Assert.AreEqual(LabelMaskableDataType.DUTIES_AND_TAXES_PAYOR_ACCOUNT_NUMBER, labelSpecification.CustomerSpecifiedDetail.MaskedData[2]);
            Assert.AreEqual(LabelMaskableDataType.CUSTOMS_VALUE, labelSpecification.CustomerSpecifiedDetail.MaskedData[3]);
        }


        [Fact]
        public void Manipulate_ConfiguresShipmentEntityThermalLabel_WithEPL_WhenFexExThermalSettingIsTrueAndConfiguredWithEPL_Test()
        {
            // No setup needed - shipping settings already initialized to EPL

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual((int)ThermalLanguage.EPL, shipmentEntity.ActualLabelFormat);
        }

        [Fact]
        public void Manipulate_ConfiguresShipmentEntityThermalLabel_WithZPL_WhenFexExThermalSettingIsTrueAndConfiguredWithZPL_Test()
        {
            // Setup
            shipmentEntity.RequestedLabelFormat = (int)ThermalLanguage.ZPL;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual((int)ThermalLanguage.ZPL, shipmentEntity.ActualLabelFormat);
        }

        [Fact]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnknownThermalType_Test()
        {
            // Setup: set the thermal type to an invalid value
            shipmentEntity.RequestedLabelFormat = 3;

            // Should throw the exception
            testObject.Manipulate(carrierRequest.Object);
        }


        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithoutDocTab_Test()
        {
            shippingSettings.FedExThermalDocTab = false;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(LabelStockType.STOCK_4X6, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithDocTabAndLeadingTabType_Test()
        {
            shippingSettings.FedExThermalDocTabType = (int)ThermalDocTabType.Leading;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(LabelStockType.STOCK_4X675_LEADING_DOC_TAB, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsLabelStockTypeForThermalLabel_WithDocTabAndTrailingTabType_Test()
        {
            shippingSettings.FedExThermalDocTabType = (int)ThermalDocTabType.Trailing;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(LabelStockType.STOCK_4X675_TRAILING_DOC_TAB, labelSpecification.LabelStockType);
        }

        [Fact]
        public void Manipulate_SetsShipmentEntityThermalType_ForImageLabel_Test()
        {
            // Setup to generate an image label
            shipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.IsNull(shipmentEntity.FedEx.Shipment.ActualLabelFormat);
        }

        [Fact]
        public void Manipulate_SetsImageTypeToPng_ForImageLabel_Test()
        {
            // Setup to generate an image label
            shipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(ShippingDocumentImageType.PNG, labelSpecification.ImageType);
        }

        [Fact]
        public void Manipulate_SetsPaper4x6StockLabel_ForImageLabel_Test()
        {
            // Setup to generate an image label
            shipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;

            testObject.Manipulate(carrierRequest.Object);

            LabelSpecification labelSpecification = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.LabelSpecification;
            Assert.AreEqual(LabelStockType.PAPER_4X6, labelSpecification.LabelStockType);
        }

    }
}
