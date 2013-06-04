using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Collection of GridColumnDefinition objects
    /// </summary>
    public class GridColumnDefinitionCollection : List<GridColumnDefinition>
    {
        /// <summary>
        /// Convenience getter for indexing by entity field.  If no definition is found with the given
        /// field null is returned.
        /// </summary>
        public GridColumnDefinition this[EntityField2 field]
        {
            get
            {
                foreach (GridColumnDefinition definition in this)
                {
                    if (EntityUtility.IsSameField(definition.DisplayValueProvider.PrimaryField, field))
                    {
                        return definition;
                    }
                }

                return null;
            }
        }
    }
}
