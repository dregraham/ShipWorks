﻿using System;
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
        static string baseObjectName = ((IEntityCore) new OrderItemEntity()).LLBLGenProEntityName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemEntity"/> class.
        /// </summary>
        /// <remarks>Sets the order and initializes nulls to default.</remarks>
        public OrderItemEntity(OrderEntity order)
            : this()
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            ((IEntity2) order).SetRelatedEntity(this, "OrderItems");

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
        /// PreProcess a value before it gets set.
        /// </summary>
        protected override void PreProcessValueToSet(IFieldInfo fieldToSet, ref object valueToSet)
        {
            // Round weights to 4 decimal places because LLBLGEN will throw when setting a double property
            // to a decimal database field
            if (fieldToSet.FieldIndex == (int) OrderItemFieldIndex.Weight)
            {
                valueToSet = Math.Round((double) valueToSet, 4);
            }

            base.PreProcessValueToSet(fieldToSet, ref valueToSet);
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

            if (IsNew && OriginalOrderID == default(long))
            {
                OriginalOrderID = OrderID;
            }
        }
    }
}
