using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Notes
{
    /// <summary>
    /// Condition for when a note was last edited
    /// </summary>
    [ConditionElement("Last Edited", "Note.Edited")]
    public class NoteEditedCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(NoteFields.Edited), context);
        }
    }
}
