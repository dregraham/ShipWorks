using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ShipWorks.Filters.Content.Editors;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the unit weight of an item
    /// </summary>
    [ConditionElement("Unit Weight", "OrderItem.UnitWeight")]
    public class UnitWeightCondition : NumericCondition<double>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitWeightCondition()
        {

        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.Weight), context);
        }
    }
}
