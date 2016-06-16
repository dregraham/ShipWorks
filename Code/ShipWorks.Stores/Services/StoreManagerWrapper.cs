using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Algorithms and functions for working with stores.
    /// </summary>
    /// <remarks>This is an instance that wraps the static StoreManager until we can replace that class</remarks>
    public class StoreManagerWrapper : IStoreManager, IInitializeForCurrentSession
    {
        readonly Func<ISecurityContext> getSecurityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreManagerWrapper(Func<ISecurityContext> getSecurityContext)
        {
            this.getSecurityContext = getSecurityContext;
        }

        /// <summary>
        /// Creates the online status filters for the given store.
        /// </summary>
        public void CreateStoreStatusFilters(IWin32Window owner, StoreEntity store)
        {
            StoreManager.CreateStoreStatusFilters(owner, store);
        }

        /// <summary>
        /// Check for any changes that may have been made
        /// </summary>
        public void CheckForChanges() => StoreManager.CheckForChanges();

        /// <summary>
        /// Initialize StoreManager
        /// </summary>
        public void InitializeForCurrentSession() =>
            StoreManager.InitializeForCurrentSession(getSecurityContext());

        /// <summary>
        /// Get the store from Id
        /// </summary>
        public StoreEntity GetStore(long storeId) => StoreManager.GetStore(storeId);

        /// <summary>
        /// Get the store for the related entity
        /// </summary>
        public StoreEntity GetRelatedStore(long orderId) => StoreManager.GetRelatedStore(orderId);

        /// <summary>
        /// Get the store for the shipment
        /// </summary>
        public StoreEntity GetRelatedStore(ShipmentEntity shipment) =>
            StoreManager.GetRelatedStore(shipment.ShipmentID);

        /// <summary>
        /// Get the current list of stores.  All stores are returned, regardless of security.
        /// </summary>
        public IEnumerable<StoreEntity> GetAllStores() => StoreManager.GetAllStores();

        /// <summary>
        /// Get all stores, regardless of security, that are currently enabled for downloading and shipping
        /// </summary>
        public IEnumerable<StoreEntity> GetEnabledStores() => StoreManager.GetEnabledStores();

        /// <summary>
        /// Saves the store.
        /// </summary>
        public void SaveStore(StoreEntity store)
        {
            StoreManager.SaveStore(store);
            StatusPresetManager.CheckForChanges();
            StoreManager.CheckForChanges();
        }
    }
}