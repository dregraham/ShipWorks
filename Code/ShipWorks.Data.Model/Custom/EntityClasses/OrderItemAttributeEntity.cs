using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class OrderItemAttributeEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemAttributeEntity"/> class.
        /// </summary>
        /// <remarks>
        ///     Sets the order item and initializes nulls to default.
        /// </remarks>
        public OrderItemAttributeEntity(OrderItemEntity orderItem) : this()
        {
            MethodConditions.EnsureArgumentIsNotNull(orderItem);

            _orderItem = orderItem;
            _orderItem.SetRelatedEntity(this, "OrderItemAttributes");

            InitializeNullsToDefault();
        }
    }
}
