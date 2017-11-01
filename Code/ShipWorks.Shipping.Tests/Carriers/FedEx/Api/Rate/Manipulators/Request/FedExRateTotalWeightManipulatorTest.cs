using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateTotalWeightManipulatorTest
    {
        FedExRateTotalWeightManipulator testObject;

        private RateRequest rateRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateTotalWeightManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            rateRequest = new RateRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                TotalWeight = 3.245f,
                FedEx = new FedExShipmentEntity { WeightUnitType = (int) WeightUnitOfMeasure.Pounds }
            };

            testObject = new FedExRateTotalWeightManipulator();
        }

        [Fact]
        public void Shouldapply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(null, FedExRateRequestOptions.None));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenRateRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            rateRequest.RequestedShipment = null;

            testObject.Manipulate(shipmentEntity, rateRequest);

            // The requested shipment property should be created now
            Assert.NotNull(rateRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsUnitsToPounds()
        {
            shipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;
            testObject.Manipulate(shipmentEntity, rateRequest);

            Weight weight = rateRequest.RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.LB, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsUnitsToKilograms()
        {
            shipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Kilograms;
            testObject.Manipulate(shipmentEntity, rateRequest);

            Weight weight = rateRequest.RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsWeightValue()
        {
            testObject.Manipulate(shipmentEntity, rateRequest);

            Weight weight = rateRequest.RequestedShipment.TotalWeight;
            Assert.Equal((decimal) shipmentEntity.TotalWeight, weight.Value);
        }

        [Fact]
        public void Manipulate_SetsWeightValueToZeroPointOne_WhenValueIsZero()
        {
            shipmentEntity.TotalWeight = 0f;

            testObject.Manipulate(shipmentEntity, rateRequest);

            Weight weight = rateRequest.RequestedShipment.TotalWeight;
            Assert.Equal(0.1m, weight.Value);
        }
    }
}
