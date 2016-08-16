using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartOrderIdentifierTest
    {
        readonly ThreeDCartOrderIdentifier testObject = new ThreeDCartOrderIdentifier(1, "pre-", "-post");

        [Fact]
        public void ApplyTo_ThrowsArgumentNullException_WhenGivenNullOrder()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.ApplyTo((OrderEntity)null));
        }

        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            Assert.Equal("pre-1-post", testObject.ToString());   
        }
    }
}