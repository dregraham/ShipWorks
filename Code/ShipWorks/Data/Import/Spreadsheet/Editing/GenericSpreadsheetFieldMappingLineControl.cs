using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// User control for editing a single generic field mapping
    /// </summary>
    public partial class GenericSpreadsheetFieldMappingLineControl : UserControl
    {
        GenericSpreadsheetMap map;
        GenericSpreadsheetTargetField field;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetFieldMappingLineControl(GenericSpreadsheetTargetField field, GenericSpreadsheetMap map)
        {
            InitializeComponent();

            this.field = field;
            this.map = map;

            labelName.Text = field.DisplayName;
            comboSourceColumn.LoadSourceColumns(map.SourceSchema.Columns);

            // Find the mapping for this field
            GenericSpreadsheetFieldMapping mapping = map.Mappings[field];
            if (mapping != null)
            {
                comboSourceColumn.SelectedColumnName = mapping.SourceColumnName;
            }
        }

        /// <summary>
        /// Save the current selection to the given map
        /// </summary>
        public bool SaveToMap(GenericSpreadsheetMap map)
        {
            map.Mappings.SetMapping(new GenericSpreadsheetFieldMapping(field, comboSourceColumn.SelectedColumnName));

            if (field.IsRequired && map.Mappings[field] == null)
            {
                MessageHelper.ShowInformation(this, string.Format("Column Mappings:\n\n'{0}' is a required field.", field.DisplayName));

                return false;
            }

            return true;
        }
    }
}
