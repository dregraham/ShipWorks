using System;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class EntityUtilityTest : IDisposable
    {
        private readonly DataContext context;

        public EntityUtilityTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ResetDirtyFlagOnUnchangedEntityFields_DirtyFlagsAreReset_WhenChangedAndChangedBack()
        {
            var order = context.Order;
            var existingCity = order.BillCity;
            order.BillCity = new string('x', 20);
            order.BillCity = existingCity;

            order.ResetDirtyFlagOnUnchangedEntityFields();

            Assert.False(order.Fields[OrderFields.BillCity.FieldIndex].IsChanged);
            Assert.False(order.IsDirty);
        }

        [Fact]
        public void ResetDirtyFlagOnUnchangedEntityFields_DirtyFlagsAreNotReset_WhenFieldIsChangedThenSavedThenChangdBack()
        {
            var order = context.Order;
            var existingCity = order.BillCity;
            order.BillCity = new string('x', 20);

            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                sqlAdapter.SaveAndRefetch(order);
            }

            Assert.False(order.Fields[OrderFields.BillCity.FieldIndex].IsChanged);
            Assert.False(order.IsDirty);

            order.BillCity = existingCity;

            order.ResetDirtyFlagOnUnchangedEntityFields();

            Assert.True(order.Fields[OrderFields.BillCity.FieldIndex].IsChanged);
            Assert.True(order.IsDirty);
        }

        [Fact]
        public void ResetDirtyFlagOnUnchangedEntityFields_DoesNotResetFlags_WhenEntityIsNew()
        {
            var order = new OrderEntity();
            var existingCity = order.BillCity;
            order.BillCity = new string('x', 20);
            order.BillCity = existingCity;

            order.ResetDirtyFlagOnUnchangedEntityFields();

            Assert.True(order.Fields[OrderFields.BillCity.FieldIndex].IsChanged);
            Assert.True(order.IsDirty);
        }

        public void Dispose() => context.Dispose();
    }
}