using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Order Lookup Confirmation Service
    /// </summary>
    public class OrderLookupConfirmationService : IOrderLookupConfirmationService
    {
        private readonly IOrderLookupMultipleMatchesViewModel viewModel;
        private readonly IOrderLookupOrderRepository repository;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupConfirmationService(
            IOrderLookupMultipleMatchesViewModel viewModel,
            IOrderLookupOrderRepository repository,
            IMessageHelper messageHelper)
        {
            this.viewModel = viewModel;
            this.repository = repository;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Prompt the user to confirm which order they want
        /// </summary>
        public async Task<long?> ConfirmOrder(List<long> list)
        {
            if (list.IsCountLessThan(2))
            {
                return list.FirstOrDefault();
            }

            if (list.IsCountGreaterThan(5))
            {
                messageHelper.ShowError("Too many orders, try a different search!");
                return null;
            }

            // now we know we have 5 or less orders
            List<OrderEntity> orders = await GetOrders(list);
            viewModel.Load(orders);



            return viewModel.SelectedOrder.OrderID;
        }

        /// <summary>
        /// Get orders for the list of ids
        /// </summary>
        private async Task<List<OrderEntity>> GetOrders(List<long> list)
        {
            var result = new List<OrderEntity>();

            foreach (long orderID in list)
            {
                result.Add(await repository.GetOrder(orderID));
            }

            return result;
        }
    }
}
