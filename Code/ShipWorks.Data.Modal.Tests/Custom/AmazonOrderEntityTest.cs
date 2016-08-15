using System;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class AmazonOrderEntityTest
    {
        [Fact]
        public void IsSameDay_ReturnsTrue_WhenReqeustedAndShipDateIsInTheFuture()
        {
            var order = new AmazonOrderEntity
            {
                RequestedShipping = "SameDay",
                LatestExpectedDeliveryDate = new DateTime(2016, 8, 15, 12, 30, 0)
            };

            var isSameDay = order.IsSameDay(() => new DateTime(2016, 8, 15, 10, 30, 0));

            Assert.True(isSameDay);
        }

        [Theory]
        [InlineData("USPS")]
        [InlineData(null)]
        public void IsSameDay_ReturnsFalse_WhenNotReqeustedAndShipDateIsInTheFuture(string requestedShipping)
        {
            var order = new AmazonOrderEntity
            {
                RequestedShipping = requestedShipping,
                LatestExpectedDeliveryDate = new DateTime(2016, 8, 15, 12, 30, 0)
            };

            var isSameDay = order.IsSameDay(() => new DateTime(2016, 8, 15, 10, 30, 0));

            Assert.False(isSameDay);
        }

        [Theory]
        [InlineData("2016-08-15T08:30:00")]
        [InlineData(null)]
        public void IsSameDay_ReturnsFalse_WhenReqeustedAndShipDateIsNotInTheFuture(string deliveryDateValue)
        {
            DateTime tempDeliveryDate;
            DateTime? deliveryDate = DateTime.TryParse(deliveryDateValue, out tempDeliveryDate) ?
                tempDeliveryDate : (DateTime?) null;

            var order = new AmazonOrderEntity
            {
                RequestedShipping = "SameDay",
                LatestExpectedDeliveryDate = deliveryDate
            };

            var isSameDay = order.IsSameDay(() => new DateTime(2016, 8, 15, 10, 30, 0));

            Assert.False(isSameDay);
        }
    }
}
