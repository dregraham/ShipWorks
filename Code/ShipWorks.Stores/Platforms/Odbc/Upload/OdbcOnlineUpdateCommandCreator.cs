﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Create online update commands for Buy.com stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Odbc)]
    public class OdbcOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IOdbcUploader odbcUploader;
        private readonly IMessageHelper messageHelper;
        private readonly IOdbcStoreRepository odbcStoreRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOnlineUpdateCommandCreator(IOdbcUploader odbcUploader, IMessageHelper messageHelper, IOdbcStoreRepository odbcStoreRepository)
        {
            this.messageHelper = messageHelper;
            this.odbcStoreRepository = odbcStoreRepository;
            this.odbcUploader = odbcUploader;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Creates the menu commands for the store
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            OdbcStoreEntity odbcStoreEntity = store as OdbcStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(odbcStoreEntity, nameof(odbcStoreEntity));

            OdbcStore odbcStore = GetOdbcStoreFromRepository(odbcStoreEntity);

            if (odbcStore == null || odbcStore.UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload)
            {
                return Enumerable.Empty<MenuCommand>();
            }

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(odbcStoreEntity, context))
            };
        }

        /// <summary>
        /// Get store from repository. Catch error if not found.
        /// </summary>
        private OdbcStore GetOdbcStoreFromRepository(OdbcStoreEntity odbcStoreEntity)
        {
            try
            {
                return odbcStoreRepository.GetStore(odbcStoreEntity);
            }
            catch (ShipWorksOdbcException)
            {
                return null;
            }
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(OdbcStoreEntity store, IMenuCommandExecutionContext context)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    x => ShipmentUploadCallback(store, x))
                .ConfigureAwait(true);

            context.Complete(results.Select(x => x.Exception).Where(x => x != null), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private Task<IResult> ShipmentUploadCallback(OdbcStoreEntity store, long orderID)
        {
            return Task.Run(() =>
            {
                return Result.Handle<Exception>()
                    .ExecuteAsync(() => odbcUploader.UploadLatestShipment(store, orderID));
            });
        }
    }
}
