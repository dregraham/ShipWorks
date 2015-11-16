using System;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    public class LemonStandOrderIdentifierTest
    {
        Mock<OrderEntity> order = new Mock<OrderEntity>();
        private LemonStandOrderIdentifier testObject;

        [Fact]
        public void ToString_ReturnsCorrectString_WhenGivenValidOrderID_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");

            Assert.Equal("LemonStandStoreOrderID:1", testObject.ToString());
        }

        [Fact]
        public void ToString_ReturnsWithoutException_WhenLemonStandOrderIDIsNull_Test1()
        {
            testObject = new LemonStandOrderIdentifier(null);
            testObject.ToString();
        }
        
        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenGivenNonLemonStandOrderEntity_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo(order.Object));
        }
        
        [Fact]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPassedNullOrderEntity_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<InvalidOperationException>(() => testObject.ApplyTo((LemonStandOrderEntity) null));
        }
        
        [Fact]
        public void ApplyTo_ThrowsArgumentNullException_WhenPassedNullDownloadDetailEntity_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((DownloadDetailEntity) null));
        }
    }
}
