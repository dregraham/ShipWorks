using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared;
using ShipWorks.Templates.Printing;
using ShipWorks.UI.Controls.Design;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMainMenuItem = Divelements.SandRibbon.MainMenuItem;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// UserControl for selecting and configuring a label sheet to use
    /// </summary>
    public partial class LabelSheetControl : UserControl
    {
        // Text to use when no label sheet is selected
        string noneText = "None Selected";

        long labelSheetID = long.MaxValue;

        /// <summary>
        /// Raised when the label sheet is changed or edited
        /// </summary>
        public event EventHandler LabelSheetChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelSheetControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the current LabelSheetID
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long LabelSheetID
        {
            get 
            {
                return labelSheetID; 
            }
            set 
            {
                if (labelSheetID != value)
                {
                    labelSheetID = value;

                    UpdateLabelSheetDetails();
                }
            }
        }

        /// <summary>
        /// Update the details shown for the selected label sheet.
        /// </summary>
        private void UpdateLabelSheetDetails()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            UpdateLabelSheetName();

            LabelSheetEntity sheet = LabelSheetManager.GetLabelSheet(labelSheetID);

            if (sheet == null)
            {
                editCustomSheet.Visible = false;

                dimensions.Text = "";
                pageSize.Text = "";
                quantity.Text = "";

            }
            else
            {
                editCustomSheet.Visible = !sheet.IsBuiltin;

                dimensions.Text = string.Format("{0:0.00}\"  x  {1:0.00}\"",
                    sheet.LabelWidth,
                    sheet.LabelHeight);

                PaperDimensions paperSize = PaperDimensions.FromDimensions(sheet.PaperSizeWidth, sheet.PaperSizeHeight);

                if (paperSize.IsCustom)
                {
                    pageSize.Text = string.Format("Custom ({0:0.00}\"  x  {1:0.00}\")",
                        sheet.PaperSizeWidth,
                        sheet.PaperSizeHeight);
                }
                else
                {
                    pageSize.Text = paperSize.Description;
                }

                quantity.Text = string.Format("{0} rows  x  {1} cols,  {2} labels",
                    sheet.Rows,
                    sheet.Columns,
                    sheet.Rows * sheet.Columns);
            }

            if (LabelSheetChanged != null)
            {
                LabelSheetChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the display text
        /// </summary>
        public void UpdateLabelSheetName()
        {
            LabelSheetEntity sheet = LabelSheetManager.GetLabelSheet(labelSheetID);
            if (sheet == null)
            {
                labelSheetName.Text = noneText;
            }
            else
            {
                labelSheetName.Text = string.Format("{0} - {1}", sheet.BrandName, sheet.Name);
            }

            sheetNameChooser.Left = labelSheetName.Right + 5;
            editCustomSheet.Left = sheetNameChooser.Right + 5;
        }

        /// <summary>
        /// The drop down to select a custom sheet is showing
        /// </summary>
        private void OnSelectSheetDropDownShowing(object sender, EventArgs e)
        {
            sheetNameChooser.SplitSandPopupMenu = LoadContextMenu();
        }

        /// <summary>
        /// Load the context menu
        /// </summary>
        [NDependIgnoreLongMethod]
        private SandContextPopup LoadContextMenu()
        {
            SandContextPopup contextMenu = new SandContextPopup();
            contextMenu.Font = new Font(Font, FontStyle.Regular);

            SandMenu rootItems = new SandMenu();
            contextMenu.Items.Add(rootItems);

            foreach (LabelSheetBrand brand in LabelSheetManager.BuiltinBrands)
            {
                SandMenuItem itemBrand = new SandMenuItem(brand.Name);
                rootItems.Items.Add(itemBrand);

                SandMenu sheetItems = new SandMenu();
                itemBrand.Items.Add(sheetItems);

                int count = 0;

                foreach (LabelSheetEntity sheet in brand.Sheets)
                {
                    AddLabelSheetMenuItem(sheetItems, sheet);

                    count++;
                    if (count >= 20)
                    {
                        // To create a new veritical partition, create a new sheet
                        sheetItems = new SandMenu();
                        itemBrand.Items.Add(sheetItems);

                        // Make sure the parent Popup is set to horizontal
                        ((Divelements.SandRibbon.Popup) sheetItems.ParentItem).LayoutDirection = Divelements.SandRibbon.LayoutDirection.Horizontal;

                        count = 0;
                    }
                }
            }

            // Add a seperateor before custom
            SandMenuItem itemCustom = new SandMenuItem("Custom");
            itemCustom.GroupName = "Custom";
            rootItems.Items.Add(itemCustom);

            SandMenu customItems = new SandMenu();
            itemCustom.Items.Add(customItems);

            // Add in custom sheets
            if (LabelSheetManager.CustomSheets.Count > 0)
            {
                foreach (LabelSheetEntity customSheet in LabelSheetManager.CustomSheets)
                {
                    AddLabelSheetMenuItem(customItems, customSheet);
                }
            }

            SandMenuItem createCustomItem = new SandMenuItem("Create New...");
            createCustomItem.GroupName = "CreateCustom";
            createCustomItem.Activate += new EventHandler(OnCreateCustomLabelSheet);
            customItems.Items.Add(createCustomItem);

            return contextMenu;
        }

        /// <summary>
        /// Add a label sheet menu item for the sheet to the given parent colleciton
        /// </summary>
        private void AddLabelSheetMenuItem(SandMenu menuItems, LabelSheetEntity sheet)
        {
            SandMenuItem menuItem = new SandMenuItem(sheet.Name);
            menuItems.Items.Add(menuItem);
            menuItem.Tag = sheet.LabelSheetID;
            menuItem.Activate += new EventHandler(OnSelectSheet);

            if (LabelSheetID == sheet.LabelSheetID)
            {
                menuItem.Checked = true;
            }
        }

        /// <summary>
        /// User has chosen a menu item
        /// </summary>
        private void OnSelectSheet(object sender, EventArgs e)
        {
            SandMenuItem item = (SandMenuItem) sender;

            LabelSheetID = (long) item.Tag;
        }

        /// <summary>
        /// Open the custom sheet editor
        /// </summary>
        private void OnCustomSheets(object sender, EventArgs e)
        {
            long sheetID = 0;

            LabelSheetEntity sheet = LabelSheetManager.GetLabelSheet(labelSheetID);
            if (sheet != null && !sheet.IsBuiltin)
            {
                sheetID = sheet.LabelSheetID;
            }

            using (LabelSheetManagerDlg dlg = new LabelSheetManagerDlg(sheetID))
            {
                dlg.ShowDialog(this);

                // See if it was deleted
                if (LabelSheetManager.GetLabelSheet(labelSheetID) == null)
                {
                    LabelSheetID = 0;
                }

                UpdateLabelSheetDetails();
            }
        }

        /// <summary>
        /// Create a new custom sheet
        /// </summary>
        private void OnCreateCustomLabelSheet(object sender, EventArgs e)
        {
            using (LabelSheetEditorDlg dlg = new LabelSheetEditorDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LabelSheetID = dlg.LabelSheetID;
                }
            }
        }

        /// <summary>
        /// Edit the currently selected custom sheet.
        /// </summary>
        private void OnEditCustomSheet(object sender, EventArgs e)
        {
            using (LabelSheetEditorDlg dlg = new LabelSheetEditorDlg(labelSheetID))
            {
                dlg.ShowDialog(this);

                LabelSheetManager.CheckForChangesNeeded();

                UpdateLabelSheetDetails();
            }
        }
    }
}
