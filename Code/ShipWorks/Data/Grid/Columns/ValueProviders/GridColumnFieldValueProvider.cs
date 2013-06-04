using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.FactoryClasses;

namespace ShipWorks.Data.Grid.Columns.ValueProviders
{
    /// <summary>
    /// Provides the value of an entity from the configured field.
    /// </summary>
    public class GridColumnFieldValueProvider : GridColumnValueProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnFieldValueProvider(EntityField2 field)
            : base(field)
        {
            if ((object) field == null)
            {
                throw new ArgumentNullException("field");
            }
        }

        /// <summary>
        /// Get the value of the specified entity.
        /// </summary>
        protected override object InternalGetValue(EntityBase2 entity)
        {
            return EntityUtility.GetFieldValue(entity, PrimaryField);
        }
    }
}
