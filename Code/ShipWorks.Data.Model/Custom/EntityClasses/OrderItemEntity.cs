using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom processing of LLBLgen OrderItemEntity
    /// </summary>
    public partial class OrderItemEntity
    {
        // We cache this so we only have to look it up once
        static string baseObjectName = new OrderItemEntity().LLBLGenProEntityName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemEntity"/> class.
        /// </summary>
        /// <remarks>Sets the order and initializes nulls to default.</remarks>
        public OrderItemEntity(OrderEntity order) : this()
        {
            MethodConditions.EnsureArgumentIsNotNull(order);

            ((IEntity2)order).SetRelatedEntity(this, "OrderItems");

            InitializeNullsToDefault();
        }

        /// <summary>
        /// Specialty property used by the 2x upgrader when updating ebay order items.  Without this set, every time an EbayOrderItem is updated
        /// the base OrderItem is forced dirty as well, taking way to long to perform.
        /// </summary>
        public static bool Is2xUpgraderUpdatingEffectiveEbayFields
        {
            get;
            set;
        }

        /// <summary>
        /// Special processing before saving to ensure base table gets hit when derived tables change
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            if (!HasBaseDirtyField(baseObjectName) && !Is2xUpgraderUpdatingEffectiveEbayFields)
            {
                // Force the timestamp to update
                Fields[(int) OrderItemFieldIndex.OrderID].IsChanged = true;
                Fields.IsDirty = true;
            }

            base.OnBeforeEntitySave();
        }
    }
}
