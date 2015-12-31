using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build entities for testing
    /// </summary>
    public class EntityBuilder<T> where T : EntityBase2, new()
    {
        readonly List<Action<T>> actions = new List<Action<T>>();
        readonly Dictionary<string, object> fieldValues = new Dictionary<string, object>();
        bool setDefaultValues = true;

        /// <summary>
        /// Perform a generic set action on the entity
        /// </summary>
        public EntityBuilder<T> Set(Action<T> setter)
        {
            actions.Add(setter);
            return this;
        }

        /// <summary>
        /// Set a field on the entity
        /// </summary>
        public EntityBuilder<T> SetField<TReturn>(Expression<Func<T, TReturn>> setter, TReturn value)
        {
            PropertyInfo propertyInfo = setter.Body is MemberExpression ?
                (setter.Body as MemberExpression).Member as PropertyInfo :
                (((UnaryExpression) setter.Body).Operand as MemberExpression).Member as PropertyInfo;

            string name = propertyInfo.Name;

            if (EditableFields(new T()).Any(x => x.Name == name))
            {
                if (fieldValues.ContainsKey(name))
                {
                    fieldValues[name] = value;
                }
                else
                {
                    fieldValues.Add(name, value);
                }
            }
            else
            {
                actions.Add(x => propertyInfo.SetValue(x, value));
            }

            return this;
        }

        /// <summary>
        /// Set the shipping address on the order
        /// </summary>
        public EntityBuilder<T> WithAddress(Expression<Func<T, PersonAdapter>> addressAdapter,
            string address1, string address2, string city, string state, string postalCode, string country)
        {
            SetField(addressAdapter, new PersonAdapter
            {
                Street1 = address1,
                Street2 = address2,
                City = city,
                StateProvCode = state,
                PostalCode = postalCode,
                CountryCode = country
            });

            return this;
        }

        /// <summary>
        /// Do not set default values on the built entity
        /// </summary>
        /// <returns></returns>
        public EntityBuilder<T> DoNotSetDefaults()
        {
            setDefaultValues = false;
            return this;
        }

        /// <summary>
        /// Save the built entity to the database
        /// </summary>
        public virtual T Save(SqlAdapter adapter)
        {
            T entity = Build();

            entity.Fields.IsDirty = true;
            entity.IsDirty = true;

            adapter.SaveAndRefetch(entity);
            return entity;
        }

        /// <summary>
        /// Build the entity
        /// </summary>
        public virtual T Build()
        {
            T entity = new T();

            foreach (IEntityField2 field in EditableFields(entity))
            {
                if (fieldValues.ContainsKey(field.Name))
                {
                    field.CurrentValue = fieldValues[field.Name];
                }
                else if (setDefaultValues)
                {
                    field.SetDefaultValue();
                }

                field.IsChanged = true;
            }

            foreach (Action<T> action in actions)
            {
                action(entity);
            }

            return entity;
        }

        /// <summary>
        /// Get a list of the editable fields in the entity
        /// </summary>
        private IEnumerable<IEntityField2> EditableFields(T entity)
        {
            return entity.Fields
                .OfType<IEntityField2>()
                .Where(x => !x.IsPrimaryKey)
                .Where(x => !x.IsReadOnly);
        }
    }
}
