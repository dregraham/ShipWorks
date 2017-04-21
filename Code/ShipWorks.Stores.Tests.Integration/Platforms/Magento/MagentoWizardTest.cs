using System;
using System.Linq;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.UI.Platforms.Magento.WizardPages;
using ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    [Trait("Store", "Magento")]
    public class MagentoWizardTest : IDisposable
    {
        readonly IContainer container;

        public MagentoWizardTest()
        {
            container = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build());
        }

        [STAFact]
        public void EnsureRegistrationOrder()
        {
            StoreEntity store = new MagentoStoreEntity { StoreTypeCode = StoreTypeCode.Magento };
            var storeType = new MagentoStoreType(store);
            var result = storeType.CreateAddStoreWizardPages(container);

            Assert.IsType<MagentoStoreSetupPage>(result.ElementAt(0));
            Assert.Equal(result.Count, 1);
        }

        public void Dispose() => container.Dispose();
    }
}
