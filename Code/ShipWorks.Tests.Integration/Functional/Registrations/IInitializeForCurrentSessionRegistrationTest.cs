using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Tests.Integration.Functional.Registrations
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class IInitializeForCurrentSessionRegistrationTest : IDisposable
    {
        IContainer container;

        public IInitializeForCurrentSessionRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void EnsureImplementationsHaveOrderAttribute()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith("ShipWorks", StringComparison.OrdinalIgnoreCase) ||
                    x.FullName.StartsWith("Interapptive", StringComparison.OrdinalIgnoreCase))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsAssignableTo<IInitializeForCurrentSession>() && !x.IsAbstract);

            foreach (var type in types)
            {
                var count = type.GetCustomAttributes(typeof(OrderAttribute), false)
                    .OfType<OrderAttribute>()
                    .Where(x => x.Service == typeof(IInitializeForCurrentSession))
                    .Count();

                Assert.True(count == 1,
                    $"{type.Name} should have 1 OrderAttribute for IInitializeForCurrentSession, but has {count}");
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
