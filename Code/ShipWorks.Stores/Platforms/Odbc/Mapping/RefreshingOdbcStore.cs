using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// An ODBC store container that holds a store and refreshes it based on the specified timeframe.
    /// </summary>
    public class RefreshingOdbcStore : IDisposable
    {
        private readonly OdbcStoreEntity storeEntity;
        private readonly Func<OdbcStoreEntity, Task<OdbcStore>> refreshAction;
        private readonly Timer timer;

        /// <summary>
        /// Constructor
        /// </summary>
        public RefreshingOdbcStore(OdbcStore store, 
            OdbcStoreEntity storeEntity, 
            Func<OdbcStoreEntity, Task<OdbcStore>> refreshAction, 
            TimeSpan timeToRefresh)
        {
            Store = store;
            this.storeEntity = storeEntity;
            this.refreshAction = refreshAction;
            
            timer = new Timer(TimerCallback, null, timeToRefresh, timeToRefresh);
        }

        /// <summary>
        /// The store
        /// </summary>
        public OdbcStore Store { get; private set; }

        /// <summary>
        /// Refreshes the Store
        /// </summary>
        private async void TimerCallback(object state)
        {
            Store = await refreshAction(storeEntity).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispose of the timer.
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
