using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.ValueProviders
{
    /// <summary>
    /// Value provider based on a user defined function
    /// </summary>
    public class GridColumnFunctionValueProvider : GridColumnValueProvider
    {
        Func<EntityBase2, object> valueFunction;

        /// <summary>
        /// Constructor
        public GridColumnFunctionValueProvider(Func<EntityBase2, object> valueFunction)
            : base(null)
        {
            if (valueFunction == null)
            {
                throw new ArgumentNullException("valueFunction");
            }

            this.valueFunction = valueFunction;
        }

        /// <summary>
        /// Get the value based on the specified entity
        /// </summary>
        protected override object InternalGetValue(EntityBase2 entity)
        {
            return valueFunction(entity);
        }
    }
}
