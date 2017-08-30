using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.OrderCombining;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Ebay stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Ebay)]
    public class EbayOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IConfigurationData configuration;
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOnlineUpdateCommandCreator(IConfigurationData configuration,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.messageHelper = messageHelper;
            this.configuration = configuration;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store) =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create the menu commands for updating the ebay order online
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands()
        {
            List<IMenuCommand> commands = new List<IMenuCommand>();

            commands.Add(new AsyncMenuCommand("Send Message...", OnSendMessage));
            commands.Add(new AsyncMenuCommand("Leave Positive Feedback...", OnLeaveFeedback) { BreakAfter = true });

            commands.Add(new MenuCommand("Mark as Paid", OnUpdateShipment) { Tag = EbayOnlineAction.Paid });
            commands.Add(new MenuCommand("Mark as Shipped", OnUpdateShipment) { Tag = EbayOnlineAction.Shipped });

            commands.Add(new MenuCommand("Mark as Not Paid", OnUpdateShipment) { Tag = EbayOnlineAction.NotPaid, BreakBefore = true });
            commands.Add(new MenuCommand("Mark as Not Shipped", OnUpdateShipment) { Tag = EbayOnlineAction.NotShipped });

            commands.AddRange(CreateCombineCommands());

            commands.Add(new MenuCommand("Ship to GSP Facility", OnShipToGspFacility) { BreakBefore = true });
            commands.Add(new MenuCommand("Ship to Buyer", OnShipToBuyer));

            return commands;
        }

        /// <summary>
        /// Create the combine commands
        /// </summary>
        private IEnumerable<IMenuCommand> CreateCombineCommands()
        {
            bool allowCombineLocally = configuration.FetchReadOnly().AllowEbayCombineLocally;

            IMenuCommand combineRemote = new AsyncMenuCommand("Combine orders on eBay...", OnCombineOrders)
            {
                BreakBefore = !allowCombineLocally,
                Tag = EbayCombinedOrderType.Ebay
            };

            return allowCombineLocally ?
                new[] { new AsyncMenuCommand("Combine orders locally...", OnCombineOrders) { BreakBefore = allowCombineLocally, Tag = EbayCombinedOrderType.Local },
                    combineRemote } :
                new[] { combineRemote };
        }

        /// <summary>
        /// Handler for the Ship to GSP Facility menu command
        /// </summary>
        /// <param name="context">The context.</param>
        private void OnShipToGspFacility(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating selected shipping method to Global Shipping Program",
                "ShipWorks is updating the selected shipment method.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);

                SendOrderSelectionChangingMessage(context.SelectedKeys);
            };

            executor.ExecuteAsync(ShipToGspFacilityCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Callback for designating an order to be shipped via GSP
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userState">State of the user.</param>
        /// <param name="issueAdder">The issue adder.</param>
        private void ShipToGspFacilityCallback(long orderId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(orderId) as EbayOrderEntity;
                if (ebayOrder == null)
                {
                    throw new EbayException(string.Format("Could not find order {0}. It may have been deleted.", orderId));
                }

                if (ebayOrder.GspEligible)
                {
                    if (ebayOrder.SelectedShippingMethod != (int) EbayShippingMethod.GlobalShippingProgram)
                    {
                        // We have an eBay order that is eligible for the GSP program that needs to have the
                        // shipping method changed
                        ebayOrder.SelectedShippingMethod = (int) EbayShippingMethod.GlobalShippingProgram;
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.SaveAndRefetch(ebayOrder);
                        }
                    }
                }
                else
                {
                    // The order is not eligible for the GSP program (determined by eBay)
                    throw new EbayException(string.Format("Order number {0} is not eligible for the Global Shipping Program", ebayOrder.OrderNumber));
                }
            }
            catch (EbayException ex)
            {
                log.ErrorFormat("Could not change order ID {0} to be shipped to GSP facility: ", orderId);
                issueAdder.Add(orderId, ex);
            }
        }

        /// <summary>
        /// Sends an OrderSelectionChangedMessage so that other panels can update appropriately.
        /// </summary>
        private static void SendOrderSelectionChangingMessage(IEnumerable<long> orderIds)
        {
            OrderSelectionChangingMessage orderSelectionChangingMessage = new OrderSelectionChangingMessage(new object(), orderIds.ToList());
            Messenger.Current.Send(orderSelectionChangingMessage);
        }

        /// <summary>
        /// Handler for the Ship to Buyer menu command
        /// </summary>
        /// <param name="context">The context.</param>
        private void OnShipToBuyer(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating selected shipping method to Standard",
                "ShipWorks is updating the selected shipment method.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);

                SendOrderSelectionChangingMessage(context.SelectedKeys);
            };

            executor.ExecuteAsync(ShipToBuyerCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Callback for designating an order to be shipped to buyer
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userState">State of the user.</param>
        /// <param name="issueAdder">The issue adder.</param>
        private void ShipToBuyerCallback(long orderId, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                EbayOrderEntity ebayOrder = DataProvider.GetEntity(orderId) as EbayOrderEntity;
                if (ebayOrder == null)
                {
                    throw new EbayException(string.Format("Could not find order {0}. It may have been deleted.", orderId));
                }

                // Only perform the update if the selected shipping method has not already been overridden to direct to buyer
                if (ebayOrder.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
                {
                    ebayOrder.SelectedShippingMethod = (int) EbayShippingMethod.DirectToBuyerOverridden;
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(ebayOrder);
                    }
                }
            }
            catch (EbayException ex)
            {
                log.ErrorFormat("Error designating order {0} to be shipped to buyer: ", orderId);
                issueAdder.Add(orderId, ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for allowing the user to combine eBay orders
        /// </summary>
        private async Task OnCombineOrders(MenuCommandExecutionContext context)
        {
            EbayPotentialCombinedOrderFinder finder = new EbayPotentialCombinedOrderFinder(context.Owner);
            List<long> orderIDs = context.SelectedKeys.ToList();

            try
            {
                var candidates = await finder.SearchAsync(orderIDs, context, (EbayCombinedOrderType) context.MenuCommand.Tag).ConfigureAwait(true);
                await OnPotentialCombinedOrdersFound(candidates).ConfigureAwait(true);
            }
            catch (EbayException ex)
            {
                context.Complete(MenuCommandResult.Error, ex.Message);
            }
        }

        /// <summary>
        /// Loading of possible combined payments is complete
        /// </summary>
        private async Task OnPotentialCombinedOrdersFound(EbayPotentialCombinedOrdersFoundEventArgs e)
        {
            // unpack the user state
            MenuCommandExecutionContext context = (MenuCommandExecutionContext) e.UserState;

            // handle completing the menu command
            if (e.Error != null)
            {
                context.Complete(MenuCommandResult.Error, e.Error.Message);
                return;
            }

            if (e.Cancelled)
            {
                context.Complete();
                return;
            }

            if (e.Candidates.Count == 0)
            {
                context.Complete(MenuCommandResult.Warning, string.Format("None of the selected orders are able to be combined {0}.", e.CombinedOrderType == EbayCombinedOrderType.Local ? "locally" : "on eBay"));
                return;
            }

            // continue to allow selection of which order(s) to combine
            using (EbayCombineOrdersDlg dlg = new EbayCombineOrdersDlg(e.CombinedOrderType, e.Candidates))
            {
                if (dlg.ShowDialog(e.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }

                List<EbayCombinedOrderCandidate> selectedOrders = dlg.SelectedOrders;

                // We should not usually be wrapping an action in a Task.Run if we can avoid it, but in this case
                // we don't have all the database and/or web calls converted to their async counterparts
                var results = await Task.Run(async () => await selectedOrders.SelectWithProgress(messageHelper,
                    "Combining eBay Orders",
                    "ShipWorks is combining eBay Orders.",
                    "Combining Order {0} of {1}...",
                    CombineOrdersCallback)
                    .ConfigureAwait(false)).ConfigureAwait(true);

                var exceptions = results.Where(x => x.Failure)
                    .Select(x => x.Exception)
                    .Where(x => x != null);

                context.Complete(exceptions, MenuCommandResult.Error);
            }
        }

        /// <summary>
        /// Worker method for combining eBay orders
        /// </summary>
        private async Task<GenericResult<EbayCombinedOrderCandidate>> CombineOrdersCallback(EbayCombinedOrderCandidate toCombine)
        {
            try
            {
                await toCombine.Combine().ConfigureAwait(false);
                return GenericResult.FromSuccess(toCombine);
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error creating combined order: {0}", ex.Message);

                return GenericResult.FromError(ex, toCombine);
            }
        }

        /// <summary>
        /// MenuCommand handler for sending messages to the eBay buyer
        /// </summary>
        private async Task OnSendMessage(MenuCommandExecutionContext context)
        {
            List<long> selectedIds = context.SelectedKeys.ToList();

            using (EbayMessagingDlg dlg = new EbayMessagingDlg(selectedIds))
            {
                if (dlg.ShowDialog(context.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }

                // if the user selected to send a message relating to a Single item, we only use that key
                if (dlg.SelectedItemID > 0)
                {
                    selectedIds.Clear();
                    selectedIds.Add(dlg.SelectedItemID);
                }

                var results = await selectedIds
                    .SelectWithProgress(messageHelper, "Sending Messages", "ShipWorks is sending eBay Messages.", "Sending message for {0} of {1}...",
                        orderID => Task.Run(() => SendMessageCallback(orderID, dlg.Subject, dlg.Message, dlg.CopyMe, dlg.MessageType)))
                    .ConfigureAwait(true);

                var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
                context.Complete(exceptions, MenuCommandResult.Error);
            }
        }

        /// <summary>
        /// Worker method for sending eBay messages.
        /// </summary>
        private async Task<IResult> SendMessageCallback(long entityId, string subject, string message, bool copyMe, EbaySendMessageType messageType)
        {
            // get the store instance
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

            // Perform token processing on the message to be sent
            string processedSubject = TemplateTokenProcessor.ProcessTokens(subject, entityId);
            string processedMessage = TemplateTokenProcessor.ProcessTokens(message, entityId);

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                await updater.SendMessage(entityId, messageType, processedSubject, processedMessage, copyMe).ConfigureAwait(false);

                return Result.FromSuccess();
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error sending eBay message for entityID {0}: {1}", entityId, ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for leaving ebay feedback
        /// </summary>
        private async Task OnLeaveFeedback(MenuCommandExecutionContext context)
        {
            // get the list of orderIds selected
            List<long> selectedIds = context.SelectedKeys.ToList();

            using (LeaveFeedbackDlg dlg = new LeaveFeedbackDlg(selectedIds))
            {
                if (dlg.ShowDialog(context.Owner) != DialogResult.OK)
                {
                    context.Complete();
                    return;
                }
                
                // if the user selected to leave feedback for a single item, use it only
                if (dlg.SelectedItemID > 0)
                {
                    selectedIds.Clear();
                    selectedIds.Add(dlg.SelectedItemID);
                }

                var results = await selectedIds
                    .SelectWithProgress(messageHelper, 
                        "Leaving Feedback", 
                        "ShipWorks is leaving eBay Feedback.", 
                        "Leaving feedback for order {0} of {1}...",
                        entityID => Task.Run(() => LeaveFeedbackCallback(entityID, dlg.FeedbackType, dlg.Comments)))
                    .ConfigureAwait(true);

                var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
                context.Complete(exceptions, MenuCommandResult.Error);
            }
        }

        /// <summary>
        /// Worker method for leaving feedback.
        /// </summary>
        private async Task<IResult> LeaveFeedbackCallback(long entityId, CommentTypeCodeType feedbackType, string feedback)
        {
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                await updater.LeaveFeedback(entityId, feedbackType, feedback).ConfigureAwait(false);

                return Result.FromSuccess();
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status for entity Id {0}: {1}", entityId, ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// MenuCommand handler for marking an order as paid/shipped
        /// </summary>
        private void OnUpdateShipment(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Updating Shipment Status",
                "ShipWorks is updating shipment status.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            // execute, passing the menu command state which tells how to perform the update
            executor.ExecuteAsync(UpdateStatusCallback, context.SelectedKeys, context.MenuCommand.Tag);
        }

        /// <summary>
        /// Worker thread method for updated order status
        /// </summary>
        private void UpdateStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // need to get the store instance for this order
            EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(orderID) as EbayStoreEntity;

            // unpack the user state
            EbayOnlineAction action = (EbayOnlineAction) userState;

            bool? paid = null;
            if (action == EbayOnlineAction.Paid || action == EbayOnlineAction.NotPaid)
            {
                paid = action == EbayOnlineAction.Paid;
            }

            bool? shipped = null;
            if (action == EbayOnlineAction.Shipped || action == EbayOnlineAction.NotShipped)
            {
                shipped = action == EbayOnlineAction.Shipped;
            }

            try
            {
                EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                updater.UpdateOnlineStatus(orderID, paid, shipped);
            }
            catch (EbayException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status for orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
