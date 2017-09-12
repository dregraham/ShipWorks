using System;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    public class LemonStandOrderIdentifierTest
    {
        readonly Mock<OrderEntity> order = new Mock<OrderEntity>();

        [Fact]
        public void ToString_ReturnsCorrectString_WhenGivenValidOrderID()
        {
            var testObject = new LemonStandOrderIdentifier("1");

            Assert.Equal("LemonStandStoreOrderID:1", testObject.ToString());
        }

        [Fact]
        public void AuditValue_ReturnsLemonStandStoreOrderID()
        {
            var testObject = new LemonStandOrderIdentifier("ABC-123");
            Assert.Equal("ABC-123", testObject.AuditValue);
        }

        [Fact]
        public void ToString_ReturnsWithoutException_WhenLemonStandOrderIDIsNull_Test1()
        {
            var testObject = new LemonStandOrderIdentifier(null);
            testObject.ToString();
        }

        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenGivenNonLemonStandOrderEntity()
        {
            var testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo(order.Object));
        }

        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPassedNullOrderEntity()
        {
            var testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo((LemonStandOrderEntity) null));
        }

        [Fact]
        public void ApplyTo_ThrowsArgumentNullException_WhenPassedNullDownloadDetailEntity()
        {
            var testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((DownloadDetailEntity) null));
        }
    }
}
