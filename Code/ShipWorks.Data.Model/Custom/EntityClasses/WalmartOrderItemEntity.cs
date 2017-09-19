namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom Walmart order item
    /// </summary>
    public partial class WalmartOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartOrderItemEntity(OrderEntity order)
            : base(order)
        {
        }
    }
}
