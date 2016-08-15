using System;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.CoreExtensions.Actions
{
    /// <summary>
    /// Upload shipment details command for ODBC
    /// </summary>
    public class OdbcUploadMenuCommand : MenuCommand
    {
        private const string CommandName = "Upload Shipment Details";
        private readonly OdbcStoreEntity store;
        private readonly IOdbcUploader odbcUploader;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMenuCommand"/> class.
        /// </summary>
        public OdbcUploadMenuCommand(OdbcStoreEntity store, IOdbcUploader odbcUploader) : base(CommandName)
        {
            this.store = store;
            this.odbcUploader = odbcUploader;
            ExecuteEvent = OnUploadDetails;
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                CommandName,
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
            try
            {
                odbcUploader.UploadLatestShipment(store, orderID);
            }
            catch (Exception ex)
            {
                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
