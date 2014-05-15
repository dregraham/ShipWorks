using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Base class for entity adapters
    /// </summary>
    public class EntityAdapter
    {
        // If there is not an entity loaded, but rather our own local values, then this holds that data
        Dictionary<string, object> localValues = null;

        // If there is an entity loaded which we represent, then this holds that data
        IEntity2 entity;
        string fieldPrefix;

        /// <summary>
        /// Creates a new instance of the adapter that maintains its own values, and has no backing entity.
        /// </summary>
        public EntityAdapter()
        {
            localValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new instance of the adapter for the entity.  All fields must
        /// be named to standard, with the optional given prefix in front of them.
        /// </summary>
        public EntityAdapter(IEntity2 entity, string fieldPrefix)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (fieldPrefix == null)
            {
                fieldPrefix = string.Empty;
            }

            this.entity = entity;
            this.fieldPrefix = fieldPrefix;
        }

        /// <summary>
        /// Get the underlying entity, if there is one
        /// </summary>
        public IEntity2 Entity
        {
            get
            {
                return entity;
            }
        }

        /// <summary>
        /// Get the prefix used for the address
        /// </summary>
        public string FieldPrefix
        {
            get
            {
                return fieldPrefix;
            }
        }

        /// <summary>
        /// Get the value of the field with the given field name.  If no field exists then the default of the type is returned
        /// </summary>
        protected T GetField<T>(string fieldName)
        {
            if (localValues != null)
            {
                object value;
                if (localValues.TryGetValue(fieldName, out value))
                {
                    return (T) value;
                }
                else
                {
                    return GetDefault<T>();
                }
            }
            else
            {
                IEntityField2 field = entity.Fields[fieldPrefix + fieldName];

                if (field == null || field.CurrentValue == null)
                {
                    return GetDefault<T>();
                }

                return (T) field.CurrentValue;
            }
        }

        /// <summary>
        /// Get our default value representation for the type
        /// </summary>
        private static T GetDefault<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T) (object) "";
            }

            return default(T);
        }

        /// <summary>
        /// Set the value of the given field name.  If the field does not exist, nothing is done.
        /// </summary>
        protected void SetField(string fieldName, object value)
        {
            if (localValues != null)
            {
                localValues[fieldName] = value;
            }
            else
            {
                IEntityField2 field = entity.Fields[fieldPrefix + fieldName];

                if (field == null)
                {
                    return;
                }

                entity.SetNewFieldValue(fieldPrefix + fieldName, value);
            }
        }

        /// <summary>
        /// Indicates if the adapter supports getting\setting the specified field
        /// </summary>
        protected bool HasField(string fieldName)
        {
            if (localValues != null)
            {
                return localValues.ContainsKey(fieldName);
            }
            else
            {
                return entity.Fields[fieldPrefix + fieldName] != null;
            }
        }

        /// <summary>
        /// Convert the current adapter to another type
        /// </summary>
        public T ConvertTo<T>() where T : EntityAdapter, new()
        {
            return new T
            {
                entity = entity, 
                fieldPrefix = fieldPrefix, 
                localValues = localValues
            };
        }
    }
}
