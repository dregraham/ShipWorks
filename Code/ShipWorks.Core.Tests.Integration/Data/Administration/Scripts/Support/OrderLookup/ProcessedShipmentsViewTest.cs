using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data.Administration.Scripts.Support.OrderLookup
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ProcessedShipmentsViewTest : IDisposable
    {
        private readonly DataContext context;
        private readonly SqlAdapter adapter;

        public ProcessedShipmentsViewTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            adapter = new SqlAdapter(false);            
        }

        [Fact]
        public void ProcessedShipmentView_ReturnsAllPossibleShipmentTypeCodes()
        {
            var shipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>().Select(e => e.Value)
                                    .Where(stc => stc != ShipmentTypeCode.None);

            BulkOrderCreator boc = new BulkOrderCreator(context);
            boc.StoreTypeCodes = new[] { boc.StoreTypeCodes.First() };
            boc.OrderDates = new[] { DateTime.UtcNow };
            boc.CreateOrderForAllStores();

            var processedShipments = new ProcessedShipmentCollection();
            adapter.FetchEntityCollection(processedShipments, null, null as IPrefetchPath2);

            int distinctShipmentTypes = processedShipments.Select(ps => ps.ShipmentType).Distinct().Count();

            Assert.Equal(shipmentTypeCodes.Count(), distinctShipmentTypes);
        }

        public void Dispose()
        {
            adapter.Dispose();
            context.Dispose();
        }
    }
}
