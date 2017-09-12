using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Amazon
{
    public class AmazonOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly AmazonOrderEntity order;

        public AmazonOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new AmazonOrderEntity();
        }

        [Fact]
        public void Test()
        {
            var testObject = new AmazonOrderIdentifier("ABC-123");
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
