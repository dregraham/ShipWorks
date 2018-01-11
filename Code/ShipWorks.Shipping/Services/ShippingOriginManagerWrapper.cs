using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;
using System.Linq;

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
            if (originId == (int) ShipmentOriginSource.Other)
            {
                return null;
            }

            // Copy from the store
            if (originId == (long) ShipmentOriginSource.Store)
            {
                return GetStoreOriginAddress(orderId);
            }

            if (originId == (long) ShipmentOriginSource.Account)
            {
                return GetAccountOriginAddress(accountId, shipmentType);
            }

            // Try looking it up as ShippingOriginID
            ShippingOriginEntity origin = ShippingOriginManager.GetOrigin(originId);
            return origin?.AsPersonAdapter();
        }

        /// <summary>
        /// Gets the origin address based on the origin id and shipment
        /// </summary>
        public PersonAdapter GetOriginAddress(long originId, ShipmentEntity shipment)
        {
            // Other - no change.
            if (originId == (int) ShipmentOriginSource.Other)
            {
                return null;
            }

            // Copy from the store
            if (originId == (long) ShipmentOriginSource.Store)
            {
                return GetStoreOriginAddress(shipment.OrderID);
            }

            if (originId == (long) ShipmentOriginSource.Account)
            {
                return GetAccountOriginAddress(shipment, shipment.ShipmentTypeCode);
            }

            // Try looking it up as ShippingOriginID
            ShippingOriginEntity origin = ShippingOriginManager.GetOrigin(originId);
            return origin?.AsPersonAdapter();
        }
                
        /// <summary>
        /// Gets the store origin address using the order id
        /// </summary>
        private PersonAdapter GetStoreOriginAddress(long orderId)
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

        /// <summary>
        /// Get the account origin address using the account id
        /// </summary>
        private PersonAdapter GetAccountOriginAddress(long accountId, ShipmentTypeCode shipmentType)
        {
            ICarrierAccountRetriever retriever = accountRetrieverFactory.Create(shipmentType);
            ICarrierAccount account = retriever.GetAccountReadOnly(accountId);
            return account?.Address;
        }

        /// <summary>
        /// Get the account origin address using the shipment
        /// </summary>
        private PersonAdapter GetAccountOriginAddress(ShipmentEntity shipment, ShipmentTypeCode shipmentType)
        {
            ICarrierAccountRetriever retriever = accountRetrieverFactory.Create(shipmentType);
            ICarrierAccount account = retriever.GetAccountReadOnly(shipment);
            if (account == null)
            {
                account = retriever.AccountsReadOnly.FirstOrDefault();
            }
            return account?.Address;
        }      
    }
}
