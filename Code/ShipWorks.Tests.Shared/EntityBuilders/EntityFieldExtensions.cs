using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Extension methods for entity fields
    /// </summary>
    public static class EntityFieldExtensions
    {
        private static readonly IDictionary<Type, object> defaultValues =
               new Dictionary<Type, object>
               {
                { typeof(byte[]), new byte[0] },
                { typeof(byte), default(byte) },
                { typeof(char), default(char) },
                { typeof(int), default(int) },
                { typeof(long), default(long) },
                { typeof(float), default(float) },
                { typeof(double), default(double) },
                { typeof(decimal), default(decimal) },
                { typeof(bool), false },
                { typeof(DateTime), new DateTime(2015, 12, 22) },
                { typeof(string), string.Empty },
                { typeof(Guid), Guid.Empty }
               };

        /// <summary>
        /// Get the default value of an entity field based on type
        /// </summary>
        public static void SetDefaultValue(this IEntityField2 field, bool setValueIfNullable)
        {
            Type type = field.DataType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (!setValueIfNullable)
                {
                    return;
                }

                type = type.GenericTypeArguments[0];
            }

            if (defaultValues.ContainsKey(type))
            {
                field.CurrentValue = defaultValues[type];
            }
        }
    }
}
