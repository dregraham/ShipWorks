using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping.Settings
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class ShippingProviderRuleManagerRegistrationTest : IDisposable
    {
        private readonly IContainer container;

        public ShippingProviderRuleManagerRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void Resolve_ReturnsSameInstance_ForAllInterfaces()
        {
            IShippingProviderRuleManager ruleManager = null;
            IShippingProviderRuleManager fromCurrentSession = null;

            // Resolve from two different sessions to make sure that single instance registration is correct
            using (var scope = container.BeginLifetimeScope())
            {
                ruleManager = scope.Resolve<IShippingProviderRuleManager>();
            }

            using (var scope = container.BeginLifetimeScope())
            {
                fromCurrentSession = scope.Resolve<IOrderedEnumerable<IInitializeForCurrentSession>>()
                    .OfType<IShippingProviderRuleManager>()
                    .Single();
            }

            Assert.Same(ruleManager, fromCurrentSession);
        }

        public void Dispose() => container.Dispose();
    }
}