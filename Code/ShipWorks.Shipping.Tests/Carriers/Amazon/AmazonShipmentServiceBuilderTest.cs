using System;
using Autofac.Extras.Moq;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentServiceBuilderTest : IDisposable
    {
        private readonly AutoMock mock;
        public AmazonShipmentServiceBuilderTest()
        {
            mock = AutoMock.GetLoose();
        }
        
        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}