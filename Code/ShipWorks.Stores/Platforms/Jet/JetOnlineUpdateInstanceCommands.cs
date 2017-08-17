using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Generates and handles OnlineUpdateInstanceCommands
    /// </summary>
    [Component(RegistrationType.Self)]
    public class JetOnlineUpdateInstanceCommands 
    {
        private readonly JetStoreEntity store;
        private readonly JetOnlineUpdater onlineUpdater;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetOnlineUpdateInstanceCommands"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="onlineUpdaterFactory">The online updater.</param>
        /// <param name="logFactory">The log factory.</param>
        public JetOnlineUpdateInstanceCommands(JetStoreEntity store,
            Func<JetStoreEntity, JetOnlineUpdater> onlineUpdaterFactory, Func<Type, ILog> logFactory)
        {
            this.store = store;
            onlineUpdater = onlineUpdaterFactory(store);
            log = logFactory(typeof(JetOnlineUpdateInstanceCommands));
        }

        /// <summary>
        /// Creates the online update instance commands.
        /// </summary>
        public IEnumerable<MenuCommand> Create()
        {
            if (store == null)
            {
                throw new JetException("Attempted to create Jet instance commands for a non Jet store");
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

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Uploads shipment details for the given order
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                onlineUpdater.UpdateShipmentDetails(orderID, store);
            }
            catch (JetException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }
    }
}