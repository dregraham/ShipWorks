using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Services
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class StoreManagerWrapperRegistrationTest : IDisposable
    {
        private readonly IContainer container;

        public StoreManagerWrapperRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void Resolve_ReturnsSameInstance_ForAllInterfaces()
        {
            IStoreManager storeManager;
            IStoreManager fromCurrentSession;

            // Resolve from two different sessions to make sure that single instance registration is correct
            using (var scope = container.BeginLifetimeScope())
            {
                storeManager = scope.Resolve<IStoreManager>();
            }

            using (var scope = container.BeginLifetimeScope())
            {
                fromCurrentSession = scope.Resolve<IOrderedEnumerable<IInitializeForCurrentSession>>()
                    .OfType<IStoreManager>()
                    .Single();
            }

            Assert.Same(storeManager, fromCurrentSession);
        }

        public void Dispose() => container.Dispose();
    }
}