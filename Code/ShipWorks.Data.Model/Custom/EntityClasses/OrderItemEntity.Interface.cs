using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'OrderItem'
    /// </summary>
    public partial interface IOrderItemEntity
    {
        /// <summary>
        /// Total cost of the order item
        /// </summary>
        decimal TotalPrice { get; }
    }
}
