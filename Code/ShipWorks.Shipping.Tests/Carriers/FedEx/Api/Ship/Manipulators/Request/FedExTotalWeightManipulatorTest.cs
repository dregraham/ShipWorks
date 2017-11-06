using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExTotalWeightManipulatorTest
    {
        FedExTotalWeightManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExTotalWeightManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
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

            testObject = new FedExTotalWeightManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
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
        public void Manipulate_SetsUnitsToPounds()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.LB, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsUnitsToKilograms()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsWeightValue()
        {
            testObject.Manipulate(carrierRequest.Object);

            Weight weight = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalWeight;
            Assert.Equal((decimal)shipmentEntity.TotalWeight, weight.Value);
        }
    }
}
