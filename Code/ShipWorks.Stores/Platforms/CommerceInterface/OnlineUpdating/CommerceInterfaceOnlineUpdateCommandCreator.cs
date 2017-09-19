using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceInterface.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for CommerceInterface
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CommerceInterfaceOnlineUpdateCommandCreator));
        private readonly IMessageHelper messageHelper;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceOnlineUpdateCommandCreator(IMessageHelper messageHelper, IOrderManager orderManager)
        {
            this.messageHelper = messageHelper;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create online update commands
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            GenericModuleStoreEntity typedStore = (GenericModuleStoreEntity) store;
            
            IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, typedStore));
            uploadCommand.BreakBefore = false;
            
            return new []{ uploadCommand };
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private async Task OnUploadDetails(MenuCommandExecutionContext context, GenericModuleStoreEntity store)
        {
            CommerceInterfaceStoreType commerceInterfaceStoreType = new CommerceInterfaceStoreType(store, messageHelper, orderManager);
            using (StatusCodeSelectionDlg dlg = new StatusCodeSelectionDlg(commerceInterfaceStoreType.CreateStatusCodeProvider()))
            {
                if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    int selectedCode = dlg.SelectedStatusCode;
                    var results = await context.SelectedKeys
                        .SelectWithProgress(messageHelper, "Upload Shipment Details", "ShipWorks is uploading shipment information.", "Updating order {0} of {1}...",
                            orderID => UploadShipmentDetailsCallback(orderID, store, selectedCode))
                        .ConfigureAwait(true);

                    var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
                    context.Complete(exceptions, MenuCommandResult.Error);
                }
            }
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, GenericModuleStoreEntity store, int code)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                }
                else
                {
                    // upload
                    CommerceInterfaceOnlineUpdater updater = new CommerceInterfaceOnlineUpdater(store);
                    await updater.UploadTrackingNumber(shipment.ShipmentID, code).ConfigureAwait(false);
                }

                return Result.FromSuccess();
            }
            catch (GenericStoreException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
