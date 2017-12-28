using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Orchestrate the steps necessary to split an order
    /// </summary>
    [Component]
    public class OrderSplitOrchestrator : IOrderSplitOrchestrator
    {
        private readonly IOrderSplitter orderSplitter;
        private readonly IOrderSplitGateway orderSplitGateway;
        private readonly IOrderSplitUserInteraction userInteraction;
        private readonly ISecurityContext securityContext;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitOrchestrator(
            IOrderSplitter orderSplitter,
            IOrderSplitGateway orderSplitGateway,
            IOrderSplitUserInteraction userInteraction,
            ISecurityContext securityContext,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.securityContext = securityContext;
            this.userInteraction = userInteraction;
            this.orderSplitGateway = orderSplitGateway;
            this.orderSplitter = orderSplitter;
        }

        /// <summary>
        /// Split an order
        /// </summary>
        public Task<IEnumerable<long>> Split(long orderID)
        {
            return orderSplitGateway
                .LoadOrder(orderID)
                .Bind(RequestPermission)
                .Bind(GetSuggestedOrderNumber)
                .Bind(GetSplitDetailsFromUser)
                .Bind(orderSplitter.Split)
                .Do(DisplaySuccessDialog, DisplayErrorMessage)
                .Map(GetOrderIDs);
        }

        /// <summary>
        /// Request permission to split the order
        /// </summary>
        private Task<OrderEntity> RequestPermission(OrderEntity order) =>
            securityContext
                    .RequestPermission(PermissionType.OrdersModify, order.StoreID)
                    .Map(() => order);

        /// <summary>
        /// Get suggested order number for the split order
        /// </summary>
        private Task<(OrderEntity order, string suggestedOrderNumber)> GetSuggestedOrderNumber(OrderEntity order) =>
            orderSplitGateway
                .GetNextOrderNumber(order.OrderID, order.OrderNumberComplete)
                .Map(x => (order, x));

        /// <summary>
        /// Get split details from the user
        /// </summary>
        private Task<OrderSplitDefinition> GetSplitDetailsFromUser((OrderEntity order, string suggestedOrderNumber) details) =>
            userInteraction.GetSplitDetailsFromUser(details.order, details.suggestedOrderNumber);

        /// <summary>
        /// Show a success dialog
        /// </summary>
        private void DisplaySuccessDialog(GenericResult<IDictionary<long, string>> result) =>
            userInteraction.ShowSuccessConfirmation(result.Value.Values);

        /// <summary>
        /// Show an error message
        /// </summary>
        private void DisplayErrorMessage(Exception ex) =>
            messageHelper.ShowError(ex.Message);

        /// <summary>
        /// Get order IDs from the split results
        /// </summary>
        private IEnumerable<long> GetOrderIDs(GenericResult<IDictionary<long, string>> result) =>
            result.Value.Keys;
    }
}
