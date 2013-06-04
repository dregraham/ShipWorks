using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Editing;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// Custom field group for order items
    /// </summary>
    public class GenericSpreadsheetOrderItemsSchemaGroup : GenericSpreadsheetTargetFieldGroup
    {
        List<GenericSpreadsheetTargetField> baseFields;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetOrderItemsSchemaGroup(string name, IEnumerable<GenericSpreadsheetTargetField> fields)
            : base(name, fields)
        {
            this.baseFields = fields.ToList();
        }

        /// <summary>
        /// Create the custom UI for editing this group
        /// </summary>
        public override GenericSpreadsheetFieldMappingGroupSettingsControlBase CreateSettingsControl(GenericSpreadsheetMap map)
        {
            return new GenericSpreadsheetOrderItemsSchemaSettingsControl(map);
        }

        /// <summary>
        /// Get the available fields based on the given settings
        /// </summary>
        public override IEnumerable<GenericSpreadsheetTargetField> GetFields(GenericSpreadsheetTargetSchemaSettings _settings)
        {
            GenericSpreadsheetOrderMapSettings settings = (GenericSpreadsheetOrderMapSettings) _settings;

            int fieldSets = (settings.MultiItemStrategy == GenericSpreadsheetOrderMultipleItemStrategy.SingleLine) ? settings.SingleLineCount : 1;

            List<GenericSpreadsheetTargetField> fields = new List<GenericSpreadsheetTargetField>();

            for (int i = 0; i < fieldSets; i++)
            {
                foreach (GenericSpreadsheetTargetField baseField in baseFields)
                {
                    string display = baseField.DisplayName;

                    if (settings.MultiItemStrategy == GenericSpreadsheetOrderMultipleItemStrategy.SingleLine)
                    {
                        string itemNumber = string.Format("Item {0}: ", i + 1);
                        display = itemNumber + display;
                    }

                    fields.Add(new GenericSpreadsheetTargetField(string.Format("{0}.{1}", baseField.Identifier, (i + 1)), display, baseField.DataType));
                }
            }

            return fields;
        }
    }
}
