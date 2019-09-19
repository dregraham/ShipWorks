using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public class RefreshingOdbcStore<TStoreDto, TStoreEntity> : IDisposable
    {
        private readonly TStoreEntity storeEntity;
        private readonly Func<TStoreEntity, Task<TStoreDto>> refreshAction;
        private readonly Timer timer;

        /// <summary>
        /// Constructor
        /// </summary>
        public RefreshingOdbcStore(TStoreDto store,
            TStoreEntity storeEntity,
            Func<TStoreEntity, Task<TStoreDto>> refreshAction,
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
        public TStoreDto Store { get; private set; }

        /// <summary>
        /// Refreshes the Store
        /// </summary>
        [SuppressMessage("SonarQube", "S3168:Return Task instead", Justification = "This is used as a timer callback")]
        private async void TimerCallback(object state)
        {
            TStoreDto store = await refreshAction(storeEntity).ConfigureAwait(false);
            if (store != null)
            {
                Store = store;
            }
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
