﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Factory for creating the online update instance commands for Walmart
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Walmart)]
    public class WalmartOnlineUpdateInstanceCommands : IOnlineUpdateCommandCreator
    {
        private readonly IShipmentDetailsUpdater onlineUpdater;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingManager shippingManager;
        private readonly IOrderManager orderManager;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOnlineUpdateInstanceCommands"/> class.
        /// </summary>
        public WalmartOnlineUpdateInstanceCommands(IShipmentDetailsUpdater onlineUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory,
            IShippingManager shippingManager,
            IOrderManager orderManager)
        {
            this.messageHelper = messageHelper;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.onlineUpdater = onlineUpdater;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Creates the online update instance commands.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            var typedStore = store as IWalmartStoreEntity;
            if (typedStore == null)
            {
                throw new WalmartException("Attempted to create Walmart instance commands for a non Walmart store");
            }

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(typedStore, context))
                {
                    BreakAfter = true
                }
            };
        }

        /// <summary>
        /// Called when the user clicks the upload shipment details menu command
        ///  </summary>
        public async Task OnUploadShipmentDetails(IWalmartStoreEntity store, IMenuCommandExecutionContext context)
        {
            IEnumerable<IResult> results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    x => Task.Run(() => UploadShipmentDetailsCallback(store, x)))
                .ConfigureAwait(true);

            IEnumerable<Exception> exceptions = results.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Uploads shipment details for the given order
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(IWalmartStoreEntity store, long orderID)
        {
            try
            {
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(orderID, false).ConfigureAwait(false);
                if (shipment != null)
                {
                    await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
                    await onlineUpdater.UpdateShipmentDetails(store, shipment).ConfigureAwait(false);
                }

                return Result.FromSuccess();
            }
            catch (WalmartException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}