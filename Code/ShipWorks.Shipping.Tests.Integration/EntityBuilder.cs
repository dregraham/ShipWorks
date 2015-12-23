using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tests.Integration
{
    public static class EntityBuilder
    {
        public static IEntityBuilder<T> Create<T>() where T : EntityBase2, new() =>
            new EntityBuilder<T>();
    }

    /// <summary>
    /// Build entities for testing
    /// </summary>
    public class EntityBuilder<T> : IEntityBuilder<T> where T : EntityBase2, new()
    {
        private static readonly IDictionary<Type, object> defaultValues =
            new Dictionary<Type, object>
            {
                { typeof(byte), 0 },
                { typeof(char), 0 },
                { typeof(int), 0 },
                { typeof(long), 0L },
                { typeof(float), 0F },
                { typeof(double), 0D },
                { typeof(decimal), 0M },
                { typeof(bool), false },
                { typeof(DateTime), new DateTime(2015, 12, 22) },
                { typeof(string), string.Empty },
                { typeof(Guid), Guid.Empty }
            };

        T entity;

        public EntityBuilder()
        {
            entity = new T();
        }

        /// <summary>
        /// Create an instance of an entity that has all its fields set to a non-null default
        /// </summary>
        public IEntityBuilder<T> WithDefaults()
        {
            foreach (IEntityField2 field in entity.Fields
                .OfType<IEntityField2>()
                .Where(x => !x.IsPrimaryKey)
                .Where(x => !x.IsReadOnly))
            {
                if (defaultValues.ContainsKey(field.DataType))
                {
                    field.CurrentValue = defaultValues[field.DataType];
                    field.IsChanged = true;
                }
            }

            entity.Fields.IsDirty = true;
            entity.IsDirty = true;

            return this;
        }

        public IEntityBuilder<T> Configure(Action<T> configuration)
        {
            configuration(entity);
            return this;
        }

        public T Save(SqlAdapter adapter)
        {
            adapter.SaveAndRefetch(entity);
            return entity;
        }
    }
}
