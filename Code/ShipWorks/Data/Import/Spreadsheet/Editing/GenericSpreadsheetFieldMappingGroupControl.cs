using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// User control for editing a group of mappings
    /// </summary>
    public partial class GenericSpreadsheetFieldMappingGroupControl : UserControl
    {
        GenericSpreadsheetTargetFieldGroup group;
        GenericSpreadsheetMap mapCopy;

        Panel panelFields = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetFieldMappingGroupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the mappings for the group into the UI
        /// </summary>
        public void LoadGroup(GenericSpreadsheetTargetFieldGroup group, GenericSpreadsheetMap map)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            this.group = group;
            this.mapCopy = map.Clone();

            GenericSpreadsheetFieldMappingGroupSettingsControlBase oldSettingsControl = SettingsControl;

            GenericSpreadsheetFieldMappingGroupSettingsControlBase settingsControl = group.CreateSettingsControl(mapCopy);
            if (settingsControl != null)
            {
                settingsControl.Location = new Point(0, 0);

                panelSettings.Controls.Add(settingsControl);
                panelSettings.Height = settingsControl.Height;

                // Listen for changes to the settings
                settingsControl.SettingsChanged += new EventHandler(OnGroupSettingsChanged);
            }
            else
            {
                panelSettings.Height = 0;
            }

            // Unhook from the old settings control
            if (oldSettingsControl != null)
            {
                oldSettingsControl.SettingsChanged -= OnGroupSettingsChanged;
                oldSettingsControl.Dispose();
            }

            // Put the fields right below the settings
            panelHeader.Top = panelSettings.Bottom;
            Refresh();

            LoadFields(group, mapCopy);
        }

        /// <summary>
        /// Load the fields for the given group and settings
        /// </summary>
        private void LoadFields(GenericSpreadsheetTargetFieldGroup group, GenericSpreadsheetMap map)
        {
            Panel panel = new Panel();
            panel.SuspendLayout();

            // For each field in the group add it to our container
            int top = 2;
            foreach (GenericSpreadsheetTargetField field in group.GetFields(map.TargetSettings))
            {
                GenericSpreadsheetFieldMappingLineControl line = new GenericSpreadsheetFieldMappingLineControl(field, map);
                line.Location = new Point(0, top);
                line.Width = this.Width;
                line.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panel.Controls.Add(line);

                top += line.Height;
            }

            panel.AutoScroll = true;
            panel.Top = panelHeader.Bottom;
            panel.Height = this.Height - panel.Top;
            panel.Width = this.Width;
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(panel);
            panel.ResumeLayout();

            if (panelFields != null)
            {
                panelFields.Dispose();
            }

            panelFields = panel;
        }

        /// <summary>
        /// Return the current settings control, if any
        /// </summary>
        private GenericSpreadsheetFieldMappingGroupSettingsControlBase SettingsControl
        {
            get 
            {
                if (panelSettings.Controls.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ((GenericSpreadsheetFieldMappingGroupSettingsControlBase) panelSettings.Controls[0]);
                }
            }
        }

        /// <summary>
        /// Save the group settings and mappings to the given map
        /// </summary>
        public bool SaveToMap(GenericSpreadsheetMap map)
        {
            return InternalSaveToMap(map, true);
        }

        /// <summary>
        /// Our internal save, which allows for control over validation
        /// </summary>
        private bool InternalSaveToMap(GenericSpreadsheetMap map, bool validate)
        {
            if (SettingsControl != null)
            {
                if (!SettingsControl.SaveSettings(map.TargetSettings, validate))
                {
                    return false;
                }
            }

            foreach (GenericSpreadsheetFieldMappingLineControl line in panelFields.Controls)
            {
                if (!line.SaveToMap(map))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The settings of the group have changd in some way that may affect the current fields
        /// </summary>
        void OnGroupSettingsChanged(object sender, EventArgs e)
        {
            // We are in the change callback - don't want to redo the UI out from under the control inititing this message
            BeginInvoke(new MethodInvoker(() =>
            {
                InternalSaveToMap(mapCopy, false);
                LoadGroup(group, mapCopy);
            }));
        }
    }
}
