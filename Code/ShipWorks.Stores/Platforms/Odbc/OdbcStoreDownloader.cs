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

                IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.Entries.SingleOrDefault(e => e.ShipWorksField.Name == OrderFields.OrderNumber.Name);

                if (odbcFieldMapEntry == null)
                {
                    throw new DownloadException("Order number not found in map.");
                }

                OrderEntity orderEntity = InstantiateOrder(new OrderNumberIdentifier((long) odbcFieldMapEntry.ShipWorksField.Value));
                SaveDownloadedOrder(orderEntity);
            }
        }
    }
}
