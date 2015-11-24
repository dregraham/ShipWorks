using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Utility functinos for working with profiles
    /// </summary>
    public static class ShippingProfileUtility
    {
        /// <summary>
        /// Apply the given value to the specified entity and field, but only if the value is non-null
        /// </summary>
        public static void ApplyProfileValue<T>(T? value, EntityBase2 entity, EntityField2 field) where T : struct
        {
            if (value.HasValue)
            {
                entity.SetNewFieldValue(field.FieldIndex, value.Value);
            }
        }

        /// <summary>
        /// Apply the given value to the specified entity and field, but only if the value is non-null
        /// </summary>
        public static void ApplyProfileValue(string value, EntityBase2 entity, EntityField2 field)
        {
            if (value != null)
            {
                entity.SetNewFieldValue(field.FieldIndex, value);
            }
        }

    }
}
