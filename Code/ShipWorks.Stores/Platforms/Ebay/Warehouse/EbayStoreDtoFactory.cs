﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Ebay.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Ebay)]
    public class EbayStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayStoreDtoFactory(IDownloadStartingPoint downloadStartingPoint,
                               IEncryptionProviderFactory encryptionProviderFactory,
                               IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
            this.downloadStartingPoint = downloadStartingPoint;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as EbayStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new EbayStore());
            var downloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity).ConfigureAwait(false);

            store.DownloadStartDate = helpers.GetUnixTimestampMillis(downloadStartDate);
            store.EbayToken = await helpers.EncryptSecret(storeEntity.EBayToken).ConfigureAwait(false);
            store.UseSandbox = !EbayUrlUtilities.UseLiveServer;

            return store;
        }
    }
}
