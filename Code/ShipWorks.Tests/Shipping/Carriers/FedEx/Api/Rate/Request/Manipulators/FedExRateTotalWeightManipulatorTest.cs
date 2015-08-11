using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateTotalWeightManipulatorTest
    {
        FedExRateTotalWeightManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateTotalWeightManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new RateRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                TotalWeight = 3.245f,
                FedEx = new FedExShipmentEntity { WeightUnitType = (int)WeightUnitOfMeasure.Pounds }
            };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateTotalWeightManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
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
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsUnitsToPounds_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;
            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.LB, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsUnitsToKilograms_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;
            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsWeightValue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal((decimal)shipmentEntity.TotalWeight, weight.Value);
        }

        [Fact]
        public void Manipulate_SetsWeightValueToZeroPointOne_WhenValueIsZero_Test()
        {
            shipmentEntity.TotalWeight = 0f;

            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal(0.1m, weight.Value);
        }
    }
}
