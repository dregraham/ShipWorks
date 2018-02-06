using System;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.BestRate
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class BestRateShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public BestRateShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock => mock.Provide(mock.Build<ISqlAdapter>()));
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateDynamicShipmentData_SetsShipmentInsuranceFromCarrierShipment(bool insured)
        {
            var shipment = Create.Shipment(context.Order)
                .AsBestRate()
                .Save();

            shipment.Insurance = !insured;
            shipment.BestRate.Insurance = insured;

            BestRateShipmentType bestRateShipmentType = context.Mock.Create<BestRateShipmentType>();
            bestRateShipmentType.UpdateDynamicShipmentData(shipment);

            Assert.Equal(insured, shipment.BestRate.Insurance);
            Assert.Equal(insured, shipment.Insurance);
        }

        public void Dispose() => context.Dispose();
    }
}
