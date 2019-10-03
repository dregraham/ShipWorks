using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Hub;
using ShipWorks.Stores.Orders.Split.Local;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// User interaction required for splitting an order
    /// </summary>
    [Component]
    public class OrderSplitUserInteraction : IOrderSplitUserInteraction
    {
        private readonly IOrderSplitViewModel splitViewModel;
        private readonly IOrderSplitSuccessViewModel successViewModel;
        private readonly IRoutingErrorViewModel routingErrorViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitUserInteraction(
            IOrderSplitViewModel splitViewModel, 
            IOrderSplitSuccessViewModel successViewModel, 
            IRoutingErrorViewModel routingErrorViewModel)
        {
            this.splitViewModel = splitViewModel;
            this.successViewModel = successViewModel;
            this.routingErrorViewModel = routingErrorViewModel;
        }

        /// <summary>
        /// Get details about splitting an order from a user
        /// </summary>
        public Task<OrderSplitDefinition> GetSplitDetailsFromUser(OrderEntity order, string newOrderNumber) =>
            splitViewModel.GetSplitDetailsFromUser(order, newOrderNumber);

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        public Task ShowSuccessConfirmation(IEnumerable<string> orderNumbers) =>
            successViewModel.ShowSuccessConfirmation(orderNumbers);

        /// <summary>
        /// Show an error while splitting orders
        /// </summary>
        public Task ShowError(Exception ex) => routingErrorViewModel.ShowError(ex);
    }
}
