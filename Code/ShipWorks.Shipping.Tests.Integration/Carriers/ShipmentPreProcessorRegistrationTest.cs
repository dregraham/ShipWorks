using System;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class ShipmentPreProcessorRegistrationTest : IDisposable
    {
        IContainer container;

        public ShipmentPreProcessorRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void EnsureBestRateUsesBestRateShipmentPreProcessor()
        {
            IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(ShipmentTypeCode.BestRate);
            Assert.IsType<BestRateShipmentPreProcessor>(service);
        }

        [Fact]
        public void EnsureAllCarriersExceptBestRateUseGenericShipmentPreProcessor()
        {
            foreach (var value in EnumHelper.GetEnumList<ShipmentTypeCode>().Where(x => x.Value != ShipmentTypeCode.BestRate))
            {
                IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(value.Value);
                Assert.IsType<GenericShipmentPreProcessor>(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
