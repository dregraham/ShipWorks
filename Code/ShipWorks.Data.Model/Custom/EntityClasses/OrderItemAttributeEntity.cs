namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class OrderItemAttributeEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemEntity"/> class.
        /// </summary>
        /// <remarks>
        ///     Sets the order item and initializes nulls to default.
        /// </remarks>
        public OrderItemAttributeEntity(OrderItemEntity orderItem) : this()
        {
            _orderItem = orderItem;

            InitializeNullsToDefault();
        }
    }
}
