using System.Collections.Generic;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface for the order lookup confirmation service
    /// </summary>
    public interface IOrderLookupConfirmationService
    {
        /// <summary>
        /// Confirm a list of orders. Return the selected order of null
        /// </summary>
        long? ConfirmOrder(List<long> list);
    }
}