using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class DataAccessAdapterTest : IDisposable
    {
        private readonly DataContext context;

        public DataAccessAdapterTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void DoNotSaveWhenEntityIsChangedAndChangedBack()
        {
            var order = context.Order;
            var existingStatus = order.LocalStatus;
            var existingVersion = order.RowVersion.ToHexString();
            order.LocalStatus = "foo";
            order.LocalStatus = existingStatus;

            using (var adapter = SqlAdapter.Create(false))
            {
                adapter.SaveAndRefetch(order);
            }

            Assert.Equal(existingVersion, order.RowVersion.ToHexString());
        }

        [Fact]
        public void SavesWhenEntityIsChanged()
        {
            var order = context.Order;
            var existingVersion = order.RowVersion.ToHexString();
            order.LocalStatus = "foo";

            using (var adapter = SqlAdapter.Create(false))
            {
                adapter.SaveAndRefetch(order);
            }

            Assert.NotEqual(existingVersion, order.RowVersion.ToHexString());
        }

        public void Dispose() => context.Dispose();
    }
}