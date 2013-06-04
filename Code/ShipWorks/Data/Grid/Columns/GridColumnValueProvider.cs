using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Base for all types that are used to provide the value of a grid column
    /// </summary>
    public abstract class GridColumnValueProvider
    {
        EntityField2 primaryField;

        /// <summary>
        /// Constructor.  primaryField can be null if there is not a field that makes since as the singular field the data comes from
        /// </summary>
        protected GridColumnValueProvider(EntityField2 primaryField)
        {
            this.primaryField = primaryField;
        }

        /// <summary>
        /// The field that is the primary driving force behind where the value of this value provider comes from.  Sometimes final
        /// values can be represented by calculations or algorithms - but this is the core underlying data used.
        /// </summary>
        public EntityField2 PrimaryField
        {
            get { return primaryField; }
        }

        /// <summary>
        /// Get the value based on the specified entity.
        /// </summary>
        public object GetValue(EntityBase2 entity)
        {
            if (entity == null)
            {
                return null;
            }

            return InternalGetValue(entity);
        }

        /// <summary>
        /// Get the value of the specified entity.
        /// </summary>
        protected abstract object InternalGetValue(EntityBase2 entity);
    }
}
