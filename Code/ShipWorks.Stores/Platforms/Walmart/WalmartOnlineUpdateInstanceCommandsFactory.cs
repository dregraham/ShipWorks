using System;
using System.Collections.Generic;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.SparkPay;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Factory for creating the online update instance commands for Walmart
    /// </summary>
    [Component(RegistrationType.Self)]
    public class WalmartOnlineUpdateInstanceCommandsFactory
    {
        private readonly WalmartStoreEntity store;
        private readonly WalmartOnlineUpdater onlineUpdater;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOnlineUpdateInstanceCommandsFactory"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="onlineUpdaterFactory">The online updater.</param>
        /// <param name="logFactory">The log factory.</param>
        public WalmartOnlineUpdateInstanceCommandsFactory(WalmartStoreEntity store,
            Func<WalmartStoreEntity, WalmartOnlineUpdater> onlineUpdaterFactory, Func<Type, ILog> logFactory)
        {
            this.store = store;
            onlineUpdater = onlineUpdaterFactory(store);
            log = logFactory(typeof(WalmartOnlineUpdateInstanceCommandsFactory));
        }

        /// <summary>
        /// Creates the online update instance commands.
        /// </summary>
        public List<MenuCommand> CreateCommands()
        {
            if (store == null)
            {
                throw new WalmartException("Attempted to create Walmart instance commands for a non Walmart store");
            }

            return new List<MenuCommand>
            {
                new MenuCommand("Upload Shipment Details", OnUploadShipmentDetails)
                {
                    BreakAfter = true
                }
            };
        }

        /// <summary>
        /// Called when the user clicks the upload shipment details menu command
        ///  </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys, context.SelectedKeys);
        }

        /// <summary>
        /// Uploads shipment details for the given order
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            List<long> orders = userState as List<long>;

            try
            {
                onlineUpdater.UpdateShipmentDetails(orders);
            }
            catch (WalmartException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                if (orders != null)
                {
                    foreach (long order in orders)
                    {
                        // add the error to issues for the user
                        issueAdder.Add(order, ex);
                    }
                }
            }
        }
    }
}