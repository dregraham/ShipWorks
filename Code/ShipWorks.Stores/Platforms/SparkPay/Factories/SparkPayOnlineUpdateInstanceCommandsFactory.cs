using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    /// <summary>
    /// Creates the menu commands for spark pay stores
    /// </summary>
    public class SparkPayOnlineUpdateInstanceCommandsFactory
    {
        private readonly SparkPayStoreEntity store;
        private readonly StatusCodeProvider<int> statusCodeProvider;
        private readonly SparkPayOnlineUpdater onlineUpdater;

        public SparkPayOnlineUpdateInstanceCommandsFactory(
            SparkPayStoreEntity store, 
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory,
            Func<SparkPayStoreEntity, SparkPayOnlineUpdater> onlineUpdaterFactory
            )
        {
            this.store = store;
            statusCodeProvider = statusCodeProviderFactory(store);
            onlineUpdater = onlineUpdaterFactory(store);
        }

        /// <summary>
        /// Create a list of menu commands for SparkPay
        /// </summary>
        /// <returns></returns>
        public List<MenuCommand> Create()
        {
            List<MenuCommand> commands = new List<MenuCommand>();
            
            bool isOne = false;
            foreach (string codeName in statusCodeProvider.CodeNames)
            {
                isOne = true;

                MenuCommand command = new MenuCommand(codeName, new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = statusCodeProvider.GetCodeValue(codeName);

                commands.Add(command);
            }

            // shipment details
            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            uploadCommand.BreakBefore = isOne;
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                return;
            }
    
            try
            {
                onlineUpdater.UpdateShipmentDetails(shipment);
            }
            catch (Exception ex)
            {
                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
     
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Set Status",
               "ShipWorks is setting the online status.",
               "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            int statusCode = (int)command.Tag;

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };
            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, statusCode);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                onlineUpdater.UpdateOrderStatus(orderID, (int)userState);
            }
            catch (SparkPayException ex)
            {
                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
