﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Editing;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// User control for editing the settings of a Excel map
    /// </summary>
    public partial class GenericExcelMapEditorControl : UserControl
    {
        GenericExcelMap mapOriginal;
        GenericExcelMap mapCopy;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMapEditorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Controls whether users are allowed to change the available source columns and Excel format options of the map
        /// </summary>
        [DefaultValue(true)]
        public bool AllowChangeSourceColumns
        {
            get
            {
                return updateSourceColumns.Visible;
            }
            set
            {
                updateSourceColumns.Visible = value;
            }
        }

        /// <summary>
        /// Load the UI with the settings from the given map
        /// </summary>
        public void LoadMap(GenericExcelMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            // Create a copy so that changes don't persist to the original
            this.mapOriginal = map;
            this.mapCopy = (GenericExcelMap) map.Clone();

            name.Text = map.Name;

            sourceColumnSummary.Text = string.Format("{0} columns, at {1} on '{2}'", map.SourceSchema.Columns.Count, map.SourceSchema.StartAddress, map.SourceSchema.SheetName);

            // Summaries
            UpdateDateFormatSummary();

            // Load the mappings
            InternalLoadMappings(mapCopy);
        }

        /// <summary>
        /// Save the settings of the UI to the given map.  Return false if there is an error and it is displayed.
        /// </summary>
        public bool SaveToMap()
        {
            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter a name for this map.");
                return false;
            }

            if (!InternalSaveMappings(mapCopy))
            {
                return false;
            }

            // General properties
            mapCopy.Name = name.Text.Trim();

            // Finally, we can update the original
            mapOriginal.LoadFromXml(mapCopy.SerializeToXml());

            return true;
        }

        /// <summary>
        /// Load the data from the given map directly into the UI
        /// </summary>
        private void InternalLoadMappings(GenericSpreadsheetMap map)
        {
            optionControl.OptionPages.Clear();

            // We will create a page for each group in the schema
            foreach (GenericSpreadsheetTargetFieldGroup group in map.TargetSchema.FieldGroups)
            {
                CreateOptionPage(group, map);
            }
        }

        /// <summary>
        /// Save the data from the UI into the given map
        /// </summary>
        private bool InternalSaveMappings(GenericSpreadsheetMap map)
        {
            foreach (GenericSpreadsheetFieldMappingGroupControl groupControl in optionControl.OptionPages.Cast<OptionPage>().Select(p => p.Controls[0]))
            {
                if (!groupControl.SaveToMap(map))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Update the summary text for the date formatting
        /// </summary>
        private void UpdateDateFormatSummary()
        {
            GenericSpreadsheetMapDateSettings settings = mapCopy.DateSettings;

            string summary = "";

            // If they are all auto, its easy
            if (settings.DateFormat == "Automatic" && settings.TimeFormat == "Automatic" && settings.DateTimeFormat == "Automatic")
            {
                summary = "Automatic";
            }
            else
            {
                summary = string.Format("[{0}] | [{1}] [{2}]", settings.DateTimeFormat, settings.DateFormat, settings.TimeFormat);
            }

            summary += string.Format(", {0} timezone", (settings.TimeZoneAssumption == GenericSpreadsheetTimeZoneAssumption.Local) ? "local" : "UTC");

            dateFormatSummary.Text = summary;
        }

        /// <summary>
        /// Change the source Excel file 
        /// </summary>
        private void OnChangeSource(object sender, EventArgs e)
        {
            // Get the current set of mappings up-to-date before showing the change window, so that any removed utilized columns are caught
            if (!InternalSaveMappings(mapCopy))
            {
                return;
            }

            using (GenericExcelChangeSourceSchemaDlg dlg = new GenericExcelChangeSourceSchemaDlg(mapCopy.Mappings.UtilizedSourceColumns))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Update the source schema
                    mapCopy.SourceSchema = dlg.SourceSchema;

                    // Reload
                    InternalLoadMappings(mapCopy);
                }
            }
        }

        /// <summary>
        /// Edit the date format
        /// </summary>
        private void OnEditDateFormat(object sender, EventArgs e)
        {
            using (GenericSpreadsheetDateSettingsDlg dlg = new GenericSpreadsheetDateSettingsDlg(mapCopy.DateSettings))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateDateFormatSummary();
                }
            }
        }

        /// <summary>
        /// Create a new option page to support editing of the given  group
        /// </summary>
        private void CreateOptionPage(GenericSpreadsheetTargetFieldGroup group, GenericSpreadsheetMap map)
        {
            OptionPage page = new OptionPage(group.Name);
            optionControl.OptionPages.Add(page);
            page.BackColor = Color.White;
            page.BorderStyle = BorderStyle.Fixed3D;

            // Need a group control to load in the group of fields
            GenericSpreadsheetFieldMappingGroupControl groupControl = new GenericSpreadsheetFieldMappingGroupControl();
            groupControl.LoadGroup(group, map);

            // Add it to the page
            groupControl.Dock = DockStyle.Fill;
            page.Controls.Add(groupControl);
        }
    }
}
