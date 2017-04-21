using System;
using System.Linq;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.UI.Platforms.Walmart.WizardPages;
using ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Walmart
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    [Trait("Store", "Walmart")]
    public class WalmartWizardTest : IDisposable
    {
        readonly IContainer container;

        public WalmartWizardTest()
        {
            container = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build());
        }

        [STAFact]
        public void EnsureRegistrationOrder()
        {
            StoreEntity store = new WalmartStoreEntity { StoreTypeCode = StoreTypeCode.Walmart };
            var storeType = container.ResolveKeyed<StoreType>(StoreTypeCode.Walmart, TypedParameter.From(store));
            var result = storeType.CreateAddStoreWizardPages(container);

            Assert.IsType<WalmartStoreSetupControlHost>(result.ElementAt(0));
            Assert.Equal(result.Count, 1);
        }

        public void Dispose() => container.Dispose();
    }
}
