﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Wizard;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class WizardPageRegistrationTest : IDisposable
    {
        readonly IContainer container;

        public WizardPageRegistrationTest()
        {
            container = ContainerInitializer.Build();
        }

        [Fact]
        public void EnsureImplementationsHaveOrderAttribute_WhenRegisteredAsComponent()
        {
            IEnumerable<Type> types = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Where(x => x.IsAssignableTo<WizardPage>() && !x.IsAbstract);

            Assert.True(types.Any());

            foreach (var type in types.Where(ShouldHaveOrderAttribute))
            {
                var count = type.GetCustomAttributes(typeof(OrderAttribute), false)
                    .OfType<OrderAttribute>()
                    .Count(x => x.Service == typeof(WizardPage));

                Assert.True(count == 1,
                    $"{type.Name} should have 1 OrderAttribute for WizardPage, but has {count}");
            }
        }

        private bool ShouldHaveOrderAttribute(Type arg)
        {
            return arg.GetCustomAttributes(typeof(KeyedComponentAttribute), false)
                .OfType<KeyedComponentAttribute>()
                .Any(x => x.Service == typeof(WizardPage));
        }

        public void Dispose() => container?.Dispose();
    }
}
