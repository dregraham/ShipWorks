﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.UI.Platforms.BigCommerce.WizardPages;
using ShipWorks.UI.Wizard;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BigCommerce
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    [Trait("Store", "BigCommerce")]
    public class BigCommerceWizardTest : IDisposable
    {
        IContainer container;

        public BigCommerceWizardTest()
        {
            container = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build());
        }

        [Fact]
        public void EnsureRegistrationOrder()
        {
            StoreEntity store = new BigCommerceStoreEntity { StoreTypeCode = StoreTypeCode.BigCommerce };
            var storeType = container.ResolveKeyed<StoreType>(StoreTypeCode.BigCommerce, TypedParameter.From(store));
            var result = storeType.CreateAddStoreWizardPages(container);

            Assert.IsType<BigCommerceAccountPage>(result.ElementAt(0));
            Assert.IsType<BigCommerceStoreSettingsPage>(result.ElementAt(1));
            Assert.Equal(result.Count(), 2);
        }

        public void Dispose() => container.Dispose();
    }
}