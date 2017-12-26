using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// User interaction required for splitting an order
    /// </summary>
    [Component]
    public class OrderSplitUserInteraction : IOrderSplitUserInteraction
    {
        readonly IOrderSplitViewModel splitViewModel;
        readonly IOrderSplitSuccessViewModel successViewModel;

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
        public GenericResult<OrderSplitDefinition> GetSplitDetailsFromUser(IOrderEntity order, string newOrderNumber) =>
            splitViewModel.GetSplitDetailsFromUser(order, newOrderNumber);

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        public void ShowSuccessConfirmation(IEnumerable<string> orderNumbers) =>
            successViewModel.ShowSuccessConfirmation(orderNumbers);
    }
}
