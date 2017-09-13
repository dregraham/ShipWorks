using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.NetworkSolutions
{
    public class NetworkSolutionsOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly NetworkSolutionsOrderEntity order;

        public NetworkSolutionsOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new NetworkSolutionsOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsNetworkSolutionsOrderID()
        {
            var testObject = new NetworkSolutionsOrderIdentifier(123);
            Assert.Equal("NetworkSolutionsOrderID:123", testObject.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
