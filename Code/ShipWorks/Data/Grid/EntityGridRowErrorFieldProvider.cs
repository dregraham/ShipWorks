using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Error provider based on the contents of an entity field
    /// </summary>
    public class EntityGridRowErrorFieldProvider : EntityGridRowErrorProvider
    {
        EntityField2 field;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityGridRowErrorFieldProvider(EntityField2 field)
        {
            if ((object) field == null)
            {
                throw new ArgumentNullException("field");
            }

            this.field = field;
        }

        /// <summary>
        /// Get the eror for the given entity
        /// </summary>
        public override string GetError(EntityBase2 entity)
        {
            return (string) entity.Fields[field.FieldIndex].CurrentValue;
        }
    }
}
