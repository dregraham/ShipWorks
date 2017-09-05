using System;
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
using ShipWorks.Email;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Yahoo)]
    public class OnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IYahooApiOnlineUpdater apiUpdater;
        private readonly IYahooEmailOnlineUpdater emailUpdater;
        private readonly IEmailCommunicatorWrapper emailCommunicator;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateCommandCreator(IYahooApiOnlineUpdater apiUpdater,
            IYahooEmailOnlineUpdater emailUpdater,
            IEmailCommunicatorWrapper emailCommunicator,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.emailUpdater = emailUpdater;
            this.emailCommunicator = emailCommunicator;
            this.apiUpdater = apiUpdater;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Creates the online update instance commands.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            IYahooStoreEntity yahooStore = store as IYahooStoreEntity;

            if (yahooStore == null)
            {
                throw new YahooException("Attempted to create Yahoo instance commands for a non Yahoo store");
            }

            if (yahooStore.YahooStoreID.IsNullOrWhiteSpace())
            {
                return new[] { new AsyncMenuCommand("Upload Shipment Details", OnUploadShipmentDetails) };
            }

            List<IMenuCommand> commands = new List<IMenuCommand>();

            commands.Add(new AsyncMenuCommand("Upload Shipment Details", context => OnApiUploadShipmentDetails(yahooStore, context))
            {
                BreakAfter = true
            });

            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.OK), context => OnSetOnlineStatus(yahooStore, context)));
            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.Fraudulent), context => OnSetOnlineStatus(yahooStore, context)));
            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.Cancelled), context => OnSetOnlineStatus(yahooStore, context)));
            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.Returned), context => OnSetOnlineStatus(yahooStore, context)));
            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.OnHold), context => OnSetOnlineStatus(yahooStore, context)));
            commands.Add(new AsyncMenuCommand(EnumHelper.GetDescription(YahooApiOrderStatus.PendingReview), context => OnSetOnlineStatus(yahooStore, context))
            {
                BreakAfter = true
            });

            return commands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        public async Task OnUploadShipmentDetails(IMenuCommandExecutionContext context)
        {
            var results = await context.SelectedKeys.SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading the tracking number.",
                    "Updating order {0} of {1}...",
                    x => Task.Run(() => UploadShipmentDetailsCallback(x)))
                .ConfigureAwait(true);
            var resultsList = results.ToList();

            // Start emailing for the yahoo email account after generating the online update emails
            var emails = resultsList.Where(x => x.Value != null).SelectMany(x => x.Value);
            emailCommunicator.StartEmailingMessages(emails);

            var exceptions = resultsList.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private async Task<GenericResult<IEnumerable<EmailOutboundEntity>>> UploadShipmentDetailsCallback(long orderID)
        {
            try
            {
                var emails = await emailUpdater.GenerateOrderShipmentUpdateEmail(orderID).ConfigureAwait(false);
                return GenericResult.FromSuccess(emails);
            }
            catch (YahooException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return GenericResult.FromError<IEnumerable<EmailOutboundEntity>>(ex);
            }
        }

        /// <summary>
        /// Called when [API upload shipment details].
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task OnApiUploadShipmentDetails(IYahooStoreEntity store, IMenuCommandExecutionContext context)
        {
            if (store == null)
            {
                throw new YahooException("Attempted to upload Yahoo shipment details for a non Yahoo store");
            }

            if (store.YahooStoreID.IsNullOrWhiteSpace())
            {
                return;
            }

            var results = await context.SelectedKeys.SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    x => Task.Run(() => ApiUploadShipmentDetailsCallback(store, x)))
                .ConfigureAwait(true);

            var exceptions = results.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// APIs the upload shipment details callback.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        /// <param name="userState">State of the user.</param>
        /// <param name="issueAdder">The issue adder.</param>
        private async Task<IResult> ApiUploadShipmentDetailsCallback(IYahooStoreEntity store, long orderID)
        {
            try
            {
                await apiUpdater.UpdateShipmentDetails(store, orderID).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (YahooException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        public async Task OnSetOnlineStatus(IYahooStoreEntity store, IMenuCommandExecutionContext context)
        {
            string statusCode = context.MenuCommand.Text;

            var results = await context.SelectedKeys.SelectWithProgress(messageHelper,
                    "Set Status",
                    "ShipWorks is setting the online status.",
                    "Updating order {0} of {1}...",
                    x => Task.Run(() => SetOnlineStatusCallback(store, x, statusCode)))
                .ConfigureAwait(true);

            var exceptions = results.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private async Task<IResult> SetOnlineStatusCallback(IYahooStoreEntity store, long orderID, string statusCode)
        {
            log.Debug(store.StoreName);

            try
            {
                await apiUpdater.UpdateOrderStatus(store, orderID, statusCode).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (YahooException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
