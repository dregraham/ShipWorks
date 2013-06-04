using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    [ConditionElement("Not Supported", "NotSupported.MigratedV2")]
    public class NotSupportedV2Condition : Condition
    {
        string detail;

        /// <summary>
        /// The detail information to display to the user in the filter editor UI
        /// </summary>
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        /// <summary>
        /// Create the editor for this condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new NotSupportedV2ValueEditor(this);
        }

        /// <summary>
        /// Generate the SQL for this condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Has no affect on the filter
            return string.Empty;
        }
    }
}
