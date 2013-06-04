using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Stores;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition based on the source of the order
    /// </summary>
    [ConditionElement("Manually Entered", "Order.Manual")]
    public class ManualCondition : Condition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ManualCondition()
        {

        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return string.Format("{0} = 1", context.GetColumnReference(OrderFields.IsManual));
        }

        /// <summary>
        /// Create the editor for the condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            // No options to configure - just return a blank value editor.
            return new ValueEditor();
        }
    }
}
