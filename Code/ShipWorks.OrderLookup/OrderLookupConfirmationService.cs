using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.OrderConfirmationDialog;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Order Lookup Confirmation Service
    /// </summary>
    public class OrderLookupConfirmationService : IOrderLookupConfirmationService
    {
        private readonly IOrderConfirmationViewModel viewModel;
        private readonly IOrderLookupOrderRepository repository;
        private readonly Func<IOrderConfirmationViewModel, IOrderConfirmationDialog> dialogFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupConfirmationService(
            IOrderConfirmationViewModel viewModel,
            IOrderLookupOrderRepository repository,
            Func<IOrderConfirmationViewModel, IOrderConfirmationDialog> dialogFactory)
        {
            this.viewModel = viewModel;
            this.repository = repository;
            this.dialogFactory = dialogFactory;
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

            IOrderConfirmationDialog confirmationDialog = dialogFactory(viewModel);
            viewModel.Orders = orders;
            viewModel.SelectedOrder = null;

            bool? dialogResult = confirmationDialog.ShowDialog();

            return dialogResult.HasValue && dialogResult.Value ?
                viewModel.SelectedOrder?.OrderID : null;
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
