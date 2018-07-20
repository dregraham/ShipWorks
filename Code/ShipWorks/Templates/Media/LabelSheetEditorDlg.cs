using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Editor for custom label sheets
    /// </summary>
    public partial class LabelSheetEditorDlg : Form
    {
        LabelSheetEntity labelSheet;

        /// <summary>
        /// Constructor for creating a new sheet
        /// </summary>
        public LabelSheetEditorDlg()
        {
            InitializeComponent();

            CreateNewSheet();
        }

        /// <summary>
        /// Constructor for editing an existing sheet
        /// </summary>
        public LabelSheetEditorDlg(long sheetID)
        {
            InitializeComponent();

            labelSheet = LabelSheetManager.GetLabelSheet(sheetID);

            if (labelSheet == null)
            {
                throw new InvalidOperationException("Non existant label sheet " + sheetID);
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            sheetName.Text = labelSheet.Name;

            paperDimensions.PaperWidth = labelSheet.PaperSizeWidth;
            paperDimensions.PaperHeight = labelSheet.PaperSizeHeight;

            height.Value = (decimal) labelSheet.LabelHeight;
            width.Value = (decimal) labelSheet.LabelWidth;

            rows.Value = labelSheet.Rows;
            columns.Value = labelSheet.Columns;

            marginLeft.Value = (decimal) labelSheet.MarginLeft;
            marginTop.Value = (decimal) labelSheet.MarginTop;

            spacingVertical.Value = (decimal) labelSheet.VerticalSpacing;
            spacingHorizontal.Value = (decimal) labelSheet.HorizontalSpacing;
        }

        /// <summary>
        /// If editing a label sheet, the ID of the sheet being edited.  If creating a label sheet, the ID of the created sheet, but
        /// only if DialogResult = OK on return.
        /// </summary>
        public long LabelSheetID
        {
            get { return labelSheet.LabelSheetID; }
        }

        /// <summary>
        /// Create a new label sheet with default values
        /// </summary>
        private void CreateNewSheet()
        {
            labelSheet = new LabelSheetEntity();

            labelSheet.Name = "";

            labelSheet.PaperSizeWidth = PaperDimensions.Default.Width;
            labelSheet.PaperSizeHeight = PaperDimensions.Default.Height;

            labelSheet.LabelHeight = 1;
            labelSheet.LabelWidth = 1;

            labelSheet.Rows = 1;
            labelSheet.Columns = 1;

            labelSheet.MarginLeft = 0;
            labelSheet.MarginTop = 0;

            labelSheet.VerticalSpacing = 0;
            labelSheet.HorizontalSpacing = 0;
        }

        /// <summary>
        /// Ensure that the values entered for the sheet make physical sense
        /// </summary>
        private bool CheckValues()
        {
            double vertical = (double) (marginTop.Value + (height.Value * rows.Value) + (spacingVertical.Value * (rows.Value - 1)));
            double horizontal = (double) (marginLeft.Value + (width.Value * columns.Value) + (spacingHorizontal.Value * (columns.Value - 1)));

            // Make sure the values are OK
            if (vertical > paperDimensions.PaperHeight|| horizontal > paperDimensions.PaperWidth)
            {
                MessageHelper.ShowError(this, "The values you have chosen would produce labels that do not fit on the page.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Save the values from the UI to the entity
        /// </summary>
        private void SaveToEntity()
        {
            labelSheet.Name = sheetName.Text.Trim();

            labelSheet.PaperSizeWidth = paperDimensions.PaperWidth;
            labelSheet.PaperSizeHeight = paperDimensions.PaperHeight;

            labelSheet.LabelHeight = (double) height.Value;
            labelSheet.LabelWidth = (double) width.Value;

            labelSheet.Rows = (int) rows.Value;
            labelSheet.Columns = (int) columns.Value;

            labelSheet.MarginLeft = (double) marginLeft.Value;
            labelSheet.MarginTop = (double) marginTop.Value;

            labelSheet.VerticalSpacing = (double) spacingVertical.Value;
            labelSheet.HorizontalSpacing = (double) spacingHorizontal.Value;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            string name = sheetName.Text.Trim();

            if (name.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the custom label sheet.");
                sheetName.Focus();
                return;
            }

            if (!CheckValues())
            {
                return;
            }

            try
            {
                bool isNew = labelSheet.IsNew;

                SaveToEntity();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(labelSheet);
                }

                if (isNew)
                {
                    LabelSheetManager.CheckForChangesNeeded();
                }

                DialogResult = DialogResult.OK;
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_SWDefault_LabelSheet_Name"))
                {
                    MessageHelper.ShowMessage(this, "A custom label sheet with the selected name already exists.");
                }
                else
                {
                    throw;
                }
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the label sheet.");

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                labelSheet.RollbackChanges();
            }
        }
    }
}