using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using System.IO;
using System.Web.Script.Serialization;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using Interapptive.Shared.Metrics;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// User control for configuring mapping for generic file import
    /// </summary>
    public partial class GenericSpreadsheetMapChooserControl : UserControl
    {
        GenericSpreadsheetTargetSchema targetSchema;
        GenericSpreadsheetMap map;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetMapChooserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control with the schema to use for the chooser and creation
        /// </summary>
        public void Initialize(GenericSpreadsheetTargetSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }

            this.targetSchema = schema;

            UpdateUI();
        }

        /// <summary>
        /// Gets or sets the map for the control
        /// </summary>
        public GenericSpreadsheetMap Map
        {
            get
            {
                return map;
            }
            set
            {
                this.map = value;

                UpdateUI();
            }
        }

        /// <summary>
        /// Schema the map is targeting
        /// </summary>
        public GenericSpreadsheetTargetSchema TargetSchema
        {
            get { return targetSchema; }
        }

        /// <summary>
        /// Must be overridden by derived classes to create the map
        /// </summary>
        protected virtual GenericSpreadsheetMap CreateMap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Must be overridden by derived classes to edit an existing map
        /// </summary>
        protected virtual bool EditMap(GenericSpreadsheetMap map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Must be overridden by derived classes to create the proper dialog for saving a map
        /// </summary>
        protected virtual SaveFileDialog CreateSaveFileDialog()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Must be overridden by derived classes to create the proper open dialog.  The result is passed to LoadMap
        /// </summary>
        protected virtual OpenFileDialog CreateOpenFileDialog()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Must be overridden by derived classes to load an existing map.  The filename and filterIndex come from the dialog returned by CreateOpenFileDialog.
        /// </summary>
        protected virtual GenericSpreadsheetMap LoadMap(string filename, int filterIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the display of the UI
        /// </summary>
        private void UpdateUI()
        {
            mapName.Text = map != null ? map.Name : "";

            panelNone.Top = panelExists.Top;

            panelNone.Visible = map == null;
            panelExists.Visible = map != null;
        }

        /// <summary>
        /// Create a new import map
        /// </summary>
        private void OnCreateMap(object sender, EventArgs e)
        {
            if (targetSchema == null)
            {
                throw new InvalidOperationException("The control has not yet been initialized with the target schema.");
            }

            GenericSpreadsheetMap newMap = CreateMap();
            if (newMap != null)
            {
                this.Map = newMap;
                CollectTelemetry();
            }
        }

        /// <summary>
        /// Edit an existing map
        /// </summary>
        private void OnEditMap(object sender, EventArgs e)
        {
            if (EditMap(Map))
            {
                UpdateUI();
                CollectTelemetry();
            }
        }

        /// <summary>
        /// Load a ShipWorks import map
        /// </summary>
        private void OnLoadMap(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = CreateOpenFileDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        GenericSpreadsheetMap loadedMap = LoadMap(dlg.FileName, dlg.FilterIndex);
                        if (loadedMap != null)
                        {
                            this.Map = loadedMap;
                            CollectTelemetry();
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                    catch (GenericSpreadsheetException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Save a shipworks import map
        /// </summary>
        private void OnSaveMap(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = CreateSaveFileDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dlg.FileName, map.SerializeToXml());
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Collect Telemetry Data
        /// </summary>
        private void CollectTelemetry()
        {
            using (ITrackedEvent trackedEvent = new TrackedEvent("Import.CustomField.Count"))
            {
                try
                {
                    trackedEvent.AddProperty("CustomField.Count", GetCustomColumnUsedCount().ToString());
                }
                catch (Exception ex)
                {
                    trackedEvent.AddProperty("Error", ex.Message);
                }
            }          
        }

        /// <summary>
        /// Get the number of used Custom Columns 
        /// </summary>
        private int GetCustomColumnUsedCount()
        {
            int count = 0;
            List<string> knownFields = new List<string>();
            foreach(var m in map.Mappings)
            {
                if(m.TargetField.Identifier.Contains("Order.Custom"))
                {
                    count++;
                }

                else if (m.TargetField.Identifier.Contains("Item.Custom"))
                {
                    var id = m.TargetField.Identifier.Substring(0, m.TargetField.Identifier.Length - 2);
                    if (!knownFields.Contains(id))
                    {
                        count++;
                        knownFields.Add(id);
                    }
                }
            }
            return count;
        }
    }
}
