using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class StoreSpecificCombineOrderActionTest : IDisposable
    {
        readonly ITestOutputHelper testOutputHelper;
        readonly IContainer container;

        public StoreSpecificCombineOrderActionTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void EnsureImplementationsKeyedComponentAttribute()
        {
            IEnumerable<Type> types = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Where(x => x.IsAssignableTo<IStoreSpecificCombineOrderAction>() && !x.IsAbstract)
                .Where(x => !HasKeyedComponentAttribute(x));

            foreach (Type type in types)
            {
                testOutputHelper.WriteLine($"{type.Name} does not have a KeyedComponent attribute for IStoreSpecificCombineOrderAction");
            }

            Assert.Empty(types);
        }

        private bool HasKeyedComponentAttribute(Type arg)
        {
            return arg.GetCustomAttributes(typeof(KeyedComponentAttribute), false)
                .OfType<KeyedComponentAttribute>()
                .Any(x => x.Service == typeof(IStoreSpecificCombineOrderAction));
        }

        public void Dispose() => container?.Dispose();
    }
}
