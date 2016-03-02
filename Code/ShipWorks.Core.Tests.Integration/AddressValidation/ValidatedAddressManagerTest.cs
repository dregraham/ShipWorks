using System;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.AddressValidation
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ValidatedAddressManagerTest : IDisposable
    {
        private readonly DataContext context;

        public ValidatedAddressManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ValidateShipmentAsync_DoesNotChangeOrderInstanceOnShipment_WhenOrderIsAlreadyLoaded()
        {
            var addressValidator = context.Mock.CreateMock<IAddressValidator>();
            var order = Modify.Order(context.Order)
                .Set(x => x.ShipAddressValidationStatus = (int) AddressValidationStatusType.Pending)
                .Save();
            var shipment = Create.Shipment(order)
                .WithAddress(x => x.ShipPerson, order.ShipPerson)
                .Set(x => x.ShipAddressValidationStatus = (int) AddressValidationStatusType.Pending)
                .Save();

            ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator.Object);

            Assert.Same(context.Order, shipment.Order);
        }

        public void Dispose() => context.Dispose();
    }
}