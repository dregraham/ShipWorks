using System;
using Autofac;
using ShipWorks.Data;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class DataProviderRegistrationTest : IDisposable
    {
        private readonly IContainer container;

        public DataProviderRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void Resolve_ReturnsInstance_ForIDataProvider()
        {
            var instance = container.Resolve<IDataProvider>();
            Assert.IsType<DataProviderWrapper>(instance);
        }

        public void Dispose() => container.Dispose();
    }
}