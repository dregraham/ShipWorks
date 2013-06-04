using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Import.Spreadsheet.Editing;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// A group of target fields.  Grouping has no real meaning other than UI display
    /// </summary>
    public class GenericSpreadsheetTargetFieldGroup
    {
        string name;
        List<GenericSpreadsheetTargetField> fields;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetTargetFieldGroup(string name, IEnumerable<GenericSpreadsheetTargetField> fields)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The parameter cannot be empty.", "name");
            }

            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }

            this.name = name;
            this.fields = fields.ToList();
        }

        /// <summary>
        /// Name of the target group
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The fields in the group, which can be based on the current map settings
        /// </summary>
        public virtual IEnumerable<GenericSpreadsheetTargetField> GetFields(GenericSpreadsheetTargetSchemaSettings settings)
        {
            return fields;
        }

        /// <summary>
        /// Create an (optional) settings control for editing additional settings of the map
        /// </summary>
        public virtual GenericSpreadsheetFieldMappingGroupSettingsControlBase CreateSettingsControl(GenericSpreadsheetMap map)
        {
            return null;
        }
    }
}
