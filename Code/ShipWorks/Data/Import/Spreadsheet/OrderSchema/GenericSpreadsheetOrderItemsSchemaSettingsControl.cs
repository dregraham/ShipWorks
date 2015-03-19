using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Editing;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// UserControl with settings for editing items
    /// </summary>
    public partial class GenericSpreadsheetOrderItemsSchemaSettingsControl : GenericSpreadsheetFieldMappingGroupSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetOrderItemsSchemaSettingsControl(GenericSpreadsheetMap map)
        {
            InitializeComponent();

            EnumHelper.BindComboBox<GenericSpreadsheetOrderMultipleItemStrategy>(multiItemStrategy);
            comboUniqueColumn.LoadSourceColumns(map.SourceSchema.Columns);

            GenericSpreadsheetOrderMapSettings settings = (GenericSpreadsheetOrderMapSettings) map.TargetSettings;

            multiItemStrategy.SelectedValue = settings.MultiItemStrategy;
            comboSingleLineCount.SelectedIndex = settings.SingleLineCount - 1;
            comboAttributeCount.SelectedIndex = settings.AttributeCountPerLine;

            // The downloading code supports more than one, but for now the UI only suppots one
            comboUniqueColumn.SelectedColumnName = settings.MultiLineKeyColumns.Count == 0 ? map.SourceSchema.Columns[0].Name : settings.MultiLineKeyColumns[0];

            panelMultiLine.Top = panelSingleLine.Top;
            UpdateUI();

            // Start listening for changes
            multiItemStrategy.SelectedIndexChanged += OnSettingsChanged;
            comboSingleLineCount.SelectedIndexChanged += OnSettingsChanged;
            comboAttributeCount.SelectedIndexChanged += OnSettingsChanged;
        }

        /// <summary>
        /// Update the UI based on the settings
        /// </summary>
        private void UpdateUI()
        {
            if ((GenericSpreadsheetOrderMultipleItemStrategy) multiItemStrategy.SelectedValue == GenericSpreadsheetOrderMultipleItemStrategy.SingleLine)
            {
                panelSingleLine.Visible = true;
                panelMultiLine.Visible = false;
                panelAttributes.Top = panelSingleLine.Bottom;

                this.Height = panelAttributes.Bottom;
            }
            else
            {
                panelMultiLine.Visible = true;
                panelSingleLine.Visible = false;
                panelAttributes.Top = panelMultiLine.Bottom;

                this.Height = panelAttributes.Bottom;
            }
        }

        /// <summary>
        /// Handle events that cause our settings to change the fields potentially displayed
        /// </summary>
        void OnSettingsChanged(object sender, EventArgs e)
        {
            UpdateUI();

            RaiseSettingsChanged();
        }

        /// <summary>
        /// Save the current settings to the map
        /// </summary>
        public override bool SaveSettings(GenericSpreadsheetTargetSchemaSettings _settings, bool validate)
        {
            GenericSpreadsheetOrderMapSettings settings = (GenericSpreadsheetOrderMapSettings) _settings;

            settings.MultiItemStrategy = (GenericSpreadsheetOrderMultipleItemStrategy) multiItemStrategy.SelectedValue;
            settings.SingleLineCount = comboSingleLineCount.SelectedIndex + 1;
            settings.AttributeCountPerLine = comboAttributeCount.SelectedIndex;

            // Only makes sense to validate if its for the relevant strategy
            if (validate)
            {
                if (settings.MultiItemStrategy == GenericSpreadsheetOrderMultipleItemStrategy.MultipleLine)
                {
                    if (string.IsNullOrWhiteSpace(comboUniqueColumn.SelectedColumnName))
                    {
                        MessageHelper.ShowInformation(this, "Items Mappings:\n\nPlease select the column that uniquely identifies an order.");
                        return false;
                    }
                }
            }

            settings.MultiLineKeyColumns.Clear();

            if (!string.IsNullOrWhiteSpace(comboUniqueColumn.SelectedColumnName))
            {
                settings.MultiLineKeyColumns.Add(comboUniqueColumn.SelectedColumnName);
            }

            return true;
        }
    }
}
