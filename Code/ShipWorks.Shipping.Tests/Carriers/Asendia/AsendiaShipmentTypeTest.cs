using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaShipmentTypeTest : IDisposable
    {
        AutoMock mock;

        public AsendiaShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void ShipmentTypeCode_IsAsendia()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            Assert.Equal(ShipmentTypeCode.Asendia, testObject.ShipmentTypeCode);
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
