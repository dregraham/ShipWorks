using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcStoreDownloader : StoreDownloader
    {
        private readonly OdbcCommandFactory commandFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly OdbcStoreEntity store;

        public OdbcStoreDownloader(StoreEntity store,
            OdbcCommandFactory commandFactory,
            IOdbcFieldMap fieldMap) : base(store)
        {
            this.commandFactory = commandFactory;
            this.fieldMap = fieldMap;
            this.store = (OdbcStoreEntity) store;

            fieldMap.Load(this.store.Map);
        }

        protected override void Download()
        {
            IOdbcCommand downloadCommand = commandFactory.CreateDownloadCommand(store);

            IEnumerable<OdbcRecord> odbcOrders = downloadCommand.Execute();

            foreach (OdbcRecord odbcOrder in odbcOrders)
            {
                fieldMap.ResetValues();
                fieldMap.ApplyValues(odbcOrder);

                // Find the OrderNumber Entry
                IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.FindEntryBy(OrderFields.OrderNumber);

                if (odbcFieldMapEntry == null)
                {
                    throw new DownloadException("Order number not found in map.");
                }

                // Create an order using the order number
                OrderEntity orderEntity = InstantiateOrder(new OrderNumberIdentifier((long) odbcFieldMapEntry.ShipWorksField.Value));




                fieldMap.CopyToEntity(orderEntity);



                // This stuff is just to ensure that ShipWorks does not crash, it might need to change in the future
                if (orderEntity.OrderDate < DateTime.Parse("1/1/1753 12:00:00 AM"))
                {
                    orderEntity.OrderDate = DateTime.UtcNow;
                }

                if (orderEntity.OnlineLastModified < DateTime.Parse("1/1/1753 12:00:00 AM"))
                {
                    orderEntity.OnlineLastModified = DateTime.UtcNow;
                }

                if (orderEntity.IsNew)
                {
                    orderEntity.OrderTotal = OrderUtility.CalculateTotal(orderEntity);
                }

                SaveDownloadedOrder(orderEntity);
            }
        }
    }
}
