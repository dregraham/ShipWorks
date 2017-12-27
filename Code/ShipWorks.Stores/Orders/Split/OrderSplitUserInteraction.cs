using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitUserInteraction(IOrderSplitViewModel splitViewModel, IOrderSplitSuccessViewModel successViewModel)
        {
            this.splitViewModel = splitViewModel;
            this.successViewModel = successViewModel;
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
    }
}
