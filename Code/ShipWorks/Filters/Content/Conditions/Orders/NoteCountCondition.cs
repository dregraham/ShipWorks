using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("Note Count", "Order.NoteCount")]
    public class NoteCountCondition : NumericCondition<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoteCountCondition()
        {
            minimumValue = 0;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.RollupNoteCount), context);
        }
    }
}
