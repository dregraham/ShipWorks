using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateOneRateManipulatorTest
    {
        private FedExRateOneRateManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateOneRateManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            nativeRequest = new RateRequest
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
            testObject = new FedExRateOneRateManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenSpecialServiceTypeArrayIsEmpty()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_AndRetainsExistingSpecialServices_WhenSpecialServiceTypeArrayIsEmpty()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { ShipmentSpecialServiceType.COD };

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(2, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.COD));
            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.FEDEX_ONE_RATE));
        }
    }
}
