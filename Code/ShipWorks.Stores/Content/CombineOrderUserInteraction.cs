using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Stores.Content;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.Controls;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Wraps user interaction required for the order combination process
    /// </summary>
    [Component]
    public class CombineOrderUserInteraction : IOrderCombinationUserInteraction
    {
        private readonly ICombineOrdersViewModel combineOrdersViewModel;
        private readonly ICombineOrderSuccessViewModel successViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderUserInteraction(
            ICombineOrdersViewModel combineOrdersViewModel,
            ICombineOrderSuccessViewModel successViewModel)
        {
            this.combineOrdersViewModel = combineOrdersViewModel;
            this.successViewModel = successViewModel;
        }

        /// <summary>
        /// Get order combination details from user
        /// </summary>
        public GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders) =>
            combineOrdersViewModel.GetCombinationDetailsFromUser(orders);

        /// <summary>
        /// Show the combination successful dialog
        /// </summary>
        public void ShowSuccessConfirmation(string orderNumber, IEnumerable<IOrderEntity> orders) =>
            successViewModel.ShowSuccessConfirmation(orderNumber, orders);
    }
}