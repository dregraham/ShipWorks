using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Orchestrate the combination of orders
    /// </summary>
    [Component]
    public class OrderCombinationOrchestrator : IOrderCombinationOrchestrator
    {
        readonly ICombineOrdersGateway gateway;
        readonly IMessageHelper messageHelper;
        readonly IOrderCombinationUserInteraction userInteraction;
        readonly IOrderCombiner orderCombiner;
        readonly ISecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombinationOrchestrator(ICombineOrdersGateway gateway,
            IOrderCombinationUserInteraction userInteraction,
            IOrderCombiner orderCombiner,
            ISecurityContext securityContext,
            IMessageHelper messageHelper)
        {
            this.securityContext = securityContext;
            this.orderCombiner = orderCombiner;
            this.gateway = gateway;
            this.userInteraction = userInteraction;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Combine the list of orders into a single order
        /// </summary>
        /// <returns>
        /// The value of the result is the ID of the created order
        /// </returns>
        public async Task<GenericResult<long>> Combine(IEnumerable<long> orderIDs)
        {
            var loadResult = await gateway.LoadOrders(orderIDs);

            if (loadResult.Failure)
            {
                messageHelper.ShowError(loadResult.Message);
                return GenericResult.FromError<long>("Error");
            }

            IEnumerable<IOrderEntity> orders = loadResult.Value;

            bool hasPermission = securityContext.HasPermission(PermissionType.OrdersModify, orders.First().StoreID);
            if (!hasPermission)
            {
                return GenericResult.FromError<long>("User does not have permission to modify orders");
            }

            var combinationDetails = userInteraction.GetCombinationDetailsFromUser(orders);
            if (combinationDetails.Failure)
            {
                return GenericResult.FromError<long>(combinationDetails.Message);
            }

            long survivingOrderId = combinationDetails.Value.Item1;
            string newOrderNumber = combinationDetails.Value.Item2;
            GenericResult<long> combineResults;

            using (ISingleItemProgressDialog dialog = messageHelper.ShowProgressDialog("Combining orders", "Combining orders"))
            {
                combineResults = await orderCombiner.Combine(survivingOrderId, orders, newOrderNumber, dialog.ProgressItem);
            }

            DisplayCombinationResults(orders, newOrderNumber, combineResults);

            return combineResults;
        }

        /// <summary>
        /// Display the results of the combination
        /// </summary>
        private void DisplayCombinationResults(IEnumerable<IOrderEntity> orders, string newOrderNumber, GenericResult<long> combineResults)
        {
            if (combineResults.Success)
            {
                userInteraction.ShowSuccessConfirmation(newOrderNumber, orders);
            }
            else
            {
                messageHelper.ShowError(combineResults.Message);
            }
        }
    }
}
