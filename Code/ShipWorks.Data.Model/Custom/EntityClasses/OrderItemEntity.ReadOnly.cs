using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ReadOnlyOrderItemEntity
    /// </summary>
    public partial class ReadOnlyOrderItemEntity
    {
        /// <summary>
        /// Total cost of the order item
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// Copy extra data defined in the custom OrderItem entity
        /// </summary>
        partial void CopyCustomOrderItemData(IOrderItemEntity source)
        {
            TotalPrice = source.TotalPrice;
        }
    }
}