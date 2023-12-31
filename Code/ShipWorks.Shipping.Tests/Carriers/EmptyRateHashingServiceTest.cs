﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers
{
    public class EmptyRateHashingServiceTest
    {
        private readonly EmptyRateHashingService emptyRatingHashingService;
        private readonly ShipmentEntity shipment;

        public EmptyRateHashingServiceTest()
        {
            emptyRatingHashingService = new EmptyRateHashingService();

            shipment = new ShipmentEntity();
        }

        [Fact]
        public void GetRatingHashReturns_EmptyString()
        {
            string testObject = emptyRatingHashingService.GetRatingHash(shipment);

            Assert.Equal(string.Empty, testObject);
        }

        [Fact]
        public void RatingFieldsReturns_NoRatingFields()
        {
            RatingFields testObject = emptyRatingHashingService.RatingFields;

            Assert.Equal(0, testObject.PackageFields.Count);
        }
    }
}