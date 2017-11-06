using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExOneRateManipulatorTest
    {
        private FedExOneRateManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExOneRateManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            nativeRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested
                    {
                        SpecialServiceTypes = new ShipmentSpecialServiceType[0]
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExOneRateManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotIFedExNativeShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ProcessShipmentReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        [Fact]
        public void Manipulate_DoesNotAddOneRateSpecialServiceType_WhenServiceTypeIsNotOneRate()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.FEDEX_ONE_RATE));
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRateExpressSaver_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRateExpressSaver;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRate2Day_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRate2Day;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRate2DayAM_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRate2DayAM;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRateFirstOvernight_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRateFirstOvernight;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRatePriorityOvernight_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRatePriorityOvernight;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenServiceTypeIsOneRateStandardOvernight_AndSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRateStandardOvernight;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_AndRetainsExistingSpecialServices_WhenSpecialServiceTypeArrayIsEmpty()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.OneRateExpressSaver;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { ShipmentSpecialServiceType.COD };

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(2, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.COD));
            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.FEDEX_ONE_RATE));
        }
    }
}
