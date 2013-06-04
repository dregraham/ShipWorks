using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Window for managing custom label sheets
    /// </summary>
    public partial class LabelSheetManagerDlg : Form
    {
        long initialSheetID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelSheetManagerDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelSheetManagerDlg(long initialSheetID) : this()
        {
            this.initialSheetID = initialSheetID;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadLabelSheets();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the sheets into the grid
        /// </summary>
        private void LoadLabelSheets()
        {
            sandGrid.Rows.Clear();

            foreach (LabelSheetEntity sheet in LabelSheetManager.CustomSheets)
            {
                GridRow row = new GridRow(new string[] { sheet.Name, GetSheetSize(sheet), GetLabelCounts(sheet) });
                sandGrid.Rows.Add(row);
                row.Tag = sheet.LabelSheetID;

                if (sheet.LabelSheetID == initialSheetID)
                {
                    row.Selected = true;
                }
            }
        }

        /// <summary>
        /// Get the sheet size description for the sheet
        /// </summary>
        private string GetSheetSize(LabelSheetEntity sheet)
        {
            PaperDimensions paperSize = PaperDimensions.FromDimensions(sheet.PaperSizeWidth, sheet.PaperSizeHeight);

            if (paperSize.IsCustom)
            {
                return string.Format("Custom ({0:0.00}\"  x  {1:0.00}\")",
                    sheet.PaperSizeWidth,
                    sheet.PaperSizeHeight);
            }
            else
            {
                return paperSize.Description;
            }
        }

        /// <summary>
        /// Get the label count description for the sheet
        /// </summary>
        private string GetLabelCounts(LabelSheetEntity sheet)
        {
            return string.Format("{0} rows  x  {1} cols,  {2} labels",
                    sheet.Rows,
                    sheet.Columns,
                    sheet.Rows * sheet.Columns);
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;
        }

        /// <summary>
        /// Selected sheet is changing
        /// </summary>
        private void OnChangeSelectedSheet(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected sheet
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            initialSheetID = (long) sandGrid.SelectedElements[0].Tag;

            using (LabelSheetEditorDlg dlg = new LabelSheetEditorDlg(initialSheetID))
            {
                dlg.ShowDialog(this);

                LabelSheetManager.CheckForChangesNeeded();
                LoadLabelSheets();
            }
        }

        /// <summary>
        /// Double-clicked a row (do Edit)
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new custom sheet
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            using (LabelSheetEditorDlg dlg = new LabelSheetEditorDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    initialSheetID = dlg.LabelSheetID;

                    LoadLabelSheets();
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            LabelSheetEntity sheet = LabelSheetManager.GetLabelSheet((long) row.Tag);

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the custom label sheet '{0}'?", sheet.Name);

            // But if the label sheet is in use, we need to tell them that
            if (LabelSheetManager.IsSheetUsed(sheet.LabelSheetID))
            {
                question = string.Format(
                    "The custom label sheet '{0}' is used by one or more templates.\n\n" +
                    "Continue and delete it?", sheet.Name);
            }

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntity(sheet);
                }

                LabelSheetManager.CheckForChangesNeeded();
                LoadLabelSheets();
            }
        }
    }
}