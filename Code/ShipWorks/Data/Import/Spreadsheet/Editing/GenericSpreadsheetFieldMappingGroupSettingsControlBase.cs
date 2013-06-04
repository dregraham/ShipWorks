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
    /// Base control for settings at the field mapping group level
    /// </summary>
    public partial class GenericSpreadsheetFieldMappingGroupSettingsControlBase : UserControl
    {
        /// <summary>
        /// Event raised when the settings have changed in some way that affects the fields for the schema
        /// </summary>
        public event EventHandler SettingsChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetFieldMappingGroupSettingsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raised when the settings have changed in some way that affects the fields for the schema
        /// </summary>
        protected void RaiseSettingsChanged()
        {
            if (SettingsChanged != null)
            {
                SettingsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Save the settings of the control to the given settings object
        /// </summary>
        public virtual bool SaveSettings(GenericSpreadsheetTargetSchemaSettings settings, bool validate)
        {
            return true;
        }
    }
}
