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
                foreach (GenericSpreadsheetTargetField baseField in baseFields.Where(f => f.Identifier != "Item.Attribute.Name"))
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

            // Attributes - treat these differently since they are just an add-on to the items and really aren't mapped to 
            // a column on the order item itself. 
            // TODO: Maybe go back and remove the attribute from the target field group entirely
            for (int i = 0; i < settings.AttributeCountPerLine; i++)
            {
                fields.Add(new GenericSpreadsheetTargetField(
                    string.Format("{0}.{1}", "Item.Attribute.Name", (i + 1)),       // Item.Attribute.Name.1
                    string.Format("Attribute {0}", (i + 1)),                        // Attribute 1
                    typeof (string)));
            }

            return fields;
        }
    }
}
