using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupConfirmationService(
            IOrderLookupMultipleMatchesViewModel viewModel,
            IOrderLookupOrderRepository repository)
        {
            this.viewModel = viewModel;
            this.repository = repository;
        }

        /// <summary>
        /// Prompt the user to confirm which order they want
        /// </summary>
        public async Task<long?> ConfirmOrder(IEnumerable<long> orderIDs)
        {
            if (orderIDs.None())
            {
                return null;
            }

            if (orderIDs.IsCountEqualTo(1))
            {
                return orderIDs.First();
            }

            List<OrderEntity> orders = await GetOrders(orderIDs);
            viewModel.Orders = orders;

            //TODO: show dialog

            return viewModel.SelectedOrder?.OrderID;
        }

        /// <summary>
        /// Get orders for the list of ids
        /// </summary>
        private async Task<List<OrderEntity>> GetOrders(IEnumerable<long> orderIDs)
        {
            List<OrderEntity> result = new List<OrderEntity>();

            foreach (long orderID in orderIDs)
            {
                result.Add(await repository.GetOrder(orderID));
            }

            return result;
        }
    }
}
