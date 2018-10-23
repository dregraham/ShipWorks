﻿using System.Collections.Generic;
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
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupConfirmationService(
            IOrderConfirmationViewModel viewModel,
            IOrderLookupOrderRepository repository,
            IWin32Window owner)
        {
            this.viewModel = viewModel;
            this.repository = repository;
            this.owner = owner;
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

            OrderConfirmationDialog confirmationDialog = new OrderConfirmationDialog(owner, viewModel);

            bool? dialogResult = confirmationDialog.ShowDialog();

            return dialogResult.HasValue && dialogResult.Value ? 
                viewModel.SelectedOrder?.OrderID :
                null;

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
