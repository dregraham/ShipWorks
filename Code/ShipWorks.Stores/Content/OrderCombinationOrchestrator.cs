using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Stores.Content;
using ShipWorks.Data.Model.EntityInterfaces;
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
        readonly ICombineOrdersViewModel viewModel;
        readonly IOrderCombiner orderCombiner;
        readonly ISecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombinationOrchestrator(ICombineOrdersGateway gateway,
            ICombineOrdersViewModel viewModel,
            IOrderCombiner orderCombiner,
            ISecurityContext securityContext,
            IMessageHelper messageHelper)
        {
            this.securityContext = securityContext;
            this.orderCombiner = orderCombiner;
            this.gateway = gateway;
            this.viewModel = viewModel;
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

            var combinationDetails = viewModel.GetCombinationDetailsFromUser(orders);
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
                string message = $"{CombineOrderNumbers(orders)} combined into Order #{newOrderNumber}";
                messageHelper.ShowUserConditionalInformation("Combine Orders", message, UserConditionalNotificationType.CombineOrders);
            }
            else
            {
                messageHelper.ShowError(combineResults.Message);
            }
        }

        /// <summary>
        /// Combine the order numbers to display
        /// </summary>
        private object CombineOrderNumbers(IEnumerable<IOrderEntity> orders)
        {
            List<string> orderNumbers = orders.Select(x => x.OrderNumberComplete).ToList();

            if (orderNumbers.Count == 1)
            {
                return $"Order #{orders.Single().OrderNumberComplete} was";
            }

            if (orderNumbers.Count == 2)
            {
                return $"Orders #{orderNumbers.First()} and #{orderNumbers.Last()} were";
            }

            string firstPart = orders.Take(orders.Count() - 1).Select(x => $"#{x.OrderNumberComplete}, ").Combine();
            return $"Orders {firstPart}and #{orders.Last().OrderNumberComplete} were";
        }
    }
}
