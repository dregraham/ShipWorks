using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wrapper for the shipping origin manager
    /// </summary>
    public class ShippingOriginManagerWrapper : IShippingOriginManager
    {
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingOriginManagerWrapper(IStoreManager storeManager, ICarrierAccountRetrieverFactory accountRetrieverFactory)
        {
            this.storeManager = storeManager;
            this.accountRetrieverFactory = accountRetrieverFactory;
        }

        /// <summary>
        /// Gets the origin address based on the given information
        /// </summary>
        public PersonAdapter GetOriginAddress(long originId, long orderId, long accountId, ShipmentTypeCode shipmentType)
        {
            // Other - no change.
            if (originId == (int)ShipmentOriginSource.Other)
            {
                return null;
            }

            // Copy from the store
            if (originId == (long)ShipmentOriginSource.Store)
            {
                StoreEntity store = storeManager.GetRelatedStore(orderId);

                // Create an intermediate person to setup the source information, so we can copy it all at one time. If we dot it in stages, it can
                // look edited when it really shouldn't and cause problems with concurrency.
                PersonAdapter source = new PersonAdapter();
                PersonAdapter.Copy(store, "", source);

                // Store doesn't maintain a first\last name - so we need to create it from the StoreName
                PersonName name = PersonName.Parse(store.StoreName);

                // Apply the name to the source
                source.FirstName = name.First;
                source.MiddleName = name.Middle;
                source.LastName = name.LastWithSuffix;

                return source;
            }

            if (originId == (long)ShipmentOriginSource.Account)
            {
                ICarrierAccountRetriever<ICarrierAccount> retriever = accountRetrieverFactory.Get(shipmentType);
                ICarrierAccount account = retriever.GetAccount(accountId);
                return account?.Address;
            }

            // Try looking it up as ShippingOriginID
            ShippingOriginEntity origin = ShippingOriginManager.GetOrigin(originId);
            return origin != null ? new PersonAdapter(origin, string.Empty) : null;
        }
    }
}
