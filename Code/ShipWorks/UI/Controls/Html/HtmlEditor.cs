using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.UI.Utility;
using ShipWorks.UI.Controls.Colors;
using ShipWorks.UI.Controls.Html.Core;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// UserControl for editing html
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class HtmlEditor : UserControl
    {
        // Current fore color that the forecolor button will activate
        Color foreColor = Color.Black;

        // Current back color that the backcolor button will activate
        Color backColor = Color.Yellow;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlEditor()
        {
            InitializeComponent();

            ThemedBorderProvider themedBorder = new ThemedBorderProvider(panelMain);

            toolStripOne.Renderer = new NoBorderToolStripRenderer();
            toolStripTwo.Renderer = new NoBorderToolStripRenderer();

            htmlControl.EditMode = true;
            htmlControl.UpdateUI += new EventHandler(OnHtmlUpdateUI);
            htmlControl.ReadyStateChanged += new ReadyStateChangedEventHandler(OnHtmlReadyStateChanged);

            UpdateColorButtons();
        }

        /// <summary>
        /// The current HTML content of the editor
        /// </summary>
        public string Html
        {
            get { return htmlControl.Html; }
            set { htmlControl.Html = value; }
        }

        /// <summary>
        /// A change in the html editor is notifying that the UI needs to be synced
        /// </summary>
        void OnHtmlUpdateUI(object sender, EventArgs e)
        {
            UpdateToolbar();
        }

        /// <summary>
        /// Ready state of the html control is changed
        /// </summary>
        void OnHtmlReadyStateChanged(object sender, ReadyStateChangedEventArgs e)
        {
            if (e.ReadyState == HtmlReadyState.Complete)
            {
                UpdateToolbar();
            }
        }

        /// <summary>
        /// Update the state of all the toolbar buttons
        /// </summary>
        [NDependIgnoreLongMethod]
        void UpdateToolbar()
        {
            fontName.Enabled = htmlControl.CanSetFontName;
            if (fontName.Enabled)
            {
                fontName.SelectedIndexChanged -= new EventHandler(OnFontNameChanged);
                fontName.SelectedItem = htmlControl.SelectionFontName;
                fontName.SelectedIndexChanged += new EventHandler(OnFontNameChanged);
            }

            fontSize.Enabled = htmlControl.CanSetFontSize;
            if (fontSize.Enabled)
            {
                fontSize.SelectedIndexChanged -= new EventHandler(OnChangeFontSize);
                fontSize.SelectedIndex = Math.Min(Math.Max(htmlControl.SelectionFontSize - 1, 0), 6);
                fontSize.SelectedIndexChanged += new EventHandler(OnChangeFontSize);
            }

            buttonCopy.Enabled = htmlControl.CanCopy;
            buttonCut.Enabled = htmlControl.CanCut;
            buttonPaste.Enabled = htmlControl.CanPaste;

            buttonInsertLine.Enabled = htmlControl.CanInsertHr;
            buttonInsertImage.Enabled = htmlControl.CanInsertImage;
            buttonInsertLink.Enabled = htmlControl.CanInsertHyperlink;

            buttonBold.Checked = htmlControl.SelectionBold;
            buttonBold.Enabled = htmlControl.CanBold;

            buttonItalic.Checked = htmlControl.SelectionItalic;
            buttonItalic.Enabled = htmlControl.CanItalic;

            buttonUnderline.Checked = htmlControl.SelectionUnderline;
            buttonUnderline.Enabled = htmlControl.CanUnderline;

            buttonBulletList.Checked = htmlControl.SelectionBullets;
            buttonBulletList.Enabled = htmlControl.CanBullets;

            buttonLeftAlign.Checked = htmlControl.SelectionAlignment == HorizontalAlignment.Left;
            buttonLeftAlign.Enabled = htmlControl.CanAlign(HorizontalAlignment.Left);

            buttonCenterAlign.Checked = htmlControl.SelectionAlignment == HorizontalAlignment.Center;
            buttonCenterAlign.Enabled = htmlControl.CanAlign(HorizontalAlignment.Center);

            buttonRightAlign.Checked = htmlControl.SelectionAlignment == HorizontalAlignment.Right;
            buttonRightAlign.Enabled = htmlControl.CanAlign(HorizontalAlignment.Right);

            buttonNumberList.Checked = htmlControl.SelectionNumbering;
            buttonNumberList.Enabled = htmlControl.CanNumbering;

            buttonOutdent.Enabled = htmlControl.CanUnindent;
            buttonIndent.Enabled = htmlControl.CanIndent;

            buttonForeColor.Enabled = htmlControl.CanSetForeColor;
            buttonBackColor.Enabled = htmlControl.CanSetBackColor;

            buttonUndo.Enabled = htmlControl.UndoManager.CanUndo;
            buttonRedo.Enabled = htmlControl.UndoManager.CanRedo;

            buttonRowBefore.Enabled = htmlControl.TableEditor.CanInsertRow;
            buttonRowAfter.Enabled = htmlControl.TableEditor.CanInsertRow;
            buttonRowDelete.Enabled = htmlControl.TableEditor.CanDeleteRow;

            buttonColumnBefore.Enabled = htmlControl.TableEditor.CanInsertColumn;
            buttonColumnAfter.Enabled = htmlControl.TableEditor.CanInsertColumn;
            buttonColumnDelete.Enabled = htmlControl.TableEditor.CanDeleteColumn;

            buttonMergeLeft.Enabled = htmlControl.TableEditor.CanMergeLeft;
            buttonMergeUp.Enabled = htmlControl.TableEditor.CanMergeUp;
            buttonMergeRight.Enabled = htmlControl.TableEditor.CanMergeRight;
            buttonMergeDown.Enabled = htmlControl.TableEditor.CanMergeDown;

            buttonShowGuides.Checked = htmlControl.ShowBorderGuides;
        }

        /// <summary>
        /// User has changed the name of the font
        /// </summary>
        private void OnFontNameChanged(object sender, System.EventArgs e)
        {
            htmlControl.SelectionFontName = fontName.SelectedItem as string;
        }

        /// <summary>
        /// Font size for selection is changing
        /// </summary>
        private void OnChangeFontSize(object sender, System.EventArgs e)
        {
            htmlControl.SelectionFontSize = fontSize.SelectedIndex + 1;
        }

        /// <summary>
        /// Update the color buttons so they are drawn to reflect the correct
        /// colors.
        /// </summary>
        private void UpdateColorButtons()
        {
            UpdateColorButton(buttonForeColor, foreColor);
            UpdateColorButton(buttonBackColor, backColor);
        }

        /// <summary>
        /// Update the color buttons so they are drawn to reflect the correct
        /// colors.
        /// </summary>
        private void UpdateColorButton(ToolStripSplitButton button, Color color)
        {
            // Get the image
            Image image = button.Image;

            // Draw over the correct area
            using (SolidBrush brush = new SolidBrush(color))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.FillRectangle(brush, 0, 12, 16, 4);
                }
            }

            button.Owner.Refresh();
        }

        /// <summary>
        /// The drop down for choosing fore\back color is opening
        /// </summary>
        private void OnColorDropDownOpening(object sender, EventArgs e)
        {
            ToolStripSplitButton button = sender as ToolStripSplitButton;
            if (button == null)
            {
                return;
            }

            Color color = (button == buttonForeColor) ? foreColor : backColor;

            // Keeps the real drop down from being seen
            button.DropDown.TopLevel = false;

            // Calculate the location
            Point location = button.Owner.PointToScreen(new Point(button.Bounds.Left, button.Bounds.Bottom));

            // Show it asyncrhously.  If we don't, we are in the call stack of this being shown, and the regular menu mouse processing does not work.
            BeginInvoke((MethodInvoker) delegate
            {
                using (ColorPickerPopup colorPopup = new ColorPickerPopup(color))
                {
                    if (colorPopup.ShowPopup((Form) TopLevelControl, location) == DialogResult.OK)
                    {
                        if (button == buttonForeColor)
                        {
                            foreColor = colorPopup.Color;
                            htmlControl.SelectionForeColor = foreColor;
                        }
                        else
                        {
                            backColor = colorPopup.Color;
                            htmlControl.SelectionBackColor = backColor;
                        }
                    }
                }

                button.DropDown.Close();

                UpdateColorButtons();
            });
        }

        /// <summary>
        /// User clicked an action button from the toolbar
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private void OnToolbarButtonClick(object sender, EventArgs e)
        {
            if (sender == buttonCut) htmlControl.Cut();
            if (sender == buttonCopy) htmlControl.Copy();
            if (sender == buttonPaste) htmlControl.Paste();

            if (sender == buttonUndo) htmlControl.UndoManager.Undo();
            if (sender == buttonRedo) htmlControl.UndoManager.Redo();

            if (sender == buttonItalic) htmlControl.SelectionItalic = !htmlControl.SelectionItalic;
            if (sender == buttonUnderline) htmlControl.SelectionUnderline = !htmlControl.SelectionUnderline;
            if (sender == buttonBold) htmlControl.SelectionBold = !htmlControl.SelectionBold;

            if (sender == buttonBulletList) htmlControl.SelectionBullets = !htmlControl.SelectionBullets;
            if (sender == buttonNumberList) htmlControl.SelectionNumbering = !htmlControl.SelectionNumbering;

            if (sender == buttonOutdent) htmlControl.Unindent();
            if (sender == buttonIndent) htmlControl.Indent();

            if (sender == buttonForeColor) htmlControl.SelectionForeColor = foreColor;
            if (sender == buttonBackColor) htmlControl.SelectionBackColor = backColor;

            if (sender == buttonLeftAlign) htmlControl.SelectionAlignment = HorizontalAlignment.Left;
            if (sender == buttonCenterAlign) htmlControl.SelectionAlignment = HorizontalAlignment.Center;
            if (sender == buttonRightAlign) htmlControl.SelectionAlignment = HorizontalAlignment.Right;

            if (sender == buttonInsertLine) htmlControl.InsertHr();
            if (sender == buttonInsertImage) htmlControl.InsertImage();
            if (sender == buttonInsertLink) htmlControl.InsertHyperlink();

            if (sender == buttonCreateTable) htmlControl.InsertTable();

            if (sender == buttonRowBefore) htmlControl.TableEditor.InsertRow(false);
            if (sender == buttonRowAfter) htmlControl.TableEditor.InsertRow(true);
            if (sender == buttonRowDelete) htmlControl.TableEditor.DeleteRow();

            if (sender == buttonColumnBefore) htmlControl.TableEditor.InsertColumn(false);
            if (sender == buttonColumnAfter) htmlControl.TableEditor.InsertColumn(true);
            if (sender == buttonColumnDelete) htmlControl.TableEditor.DeleteColumn();

            if (sender == buttonMergeLeft) htmlControl.TableEditor.MergeLeft();
            if (sender == buttonMergeUp) htmlControl.TableEditor.MergeUp();
            if (sender == buttonMergeRight) htmlControl.TableEditor.MergeRight();
            if (sender == buttonMergeDown) htmlControl.TableEditor.MergeDown();

            if (sender == buttonShowGuides) htmlControl.ShowBorderGuides = !htmlControl.ShowBorderGuides;

            UpdateToolbar();
        }
        

        /// <summary>
        /// One of the context menu items has been clicked
        /// </summary>
        private void OnContextMenuItemClick(object sender, EventArgs e)
        {
            OnToolbarButtonClick(TranslateContextToToolbar((ToolStripMenuItem) sender), EventArgs.Empty);
        }

        /// <summary>
        /// Translate the context menu item to its corresponding toolbar button
        /// </summary>
        private ToolStripButton TranslateContextToToolbar(ToolStripMenuItem menuItem)
        {
            if (menuItem == contextCut) return buttonCut;
            if (menuItem == contextCopy) return buttonCopy;
            if (menuItem == contextPaste) return buttonPaste;

            if (menuItem == contextInsertRowAfter) return buttonRowAfter;
            if (menuItem == contextInsertRowBefore) return buttonRowBefore;
            if (menuItem == contextDeleteRow) return buttonRowDelete;

            if (menuItem == contextInsertColumnAfter) return buttonColumnAfter;
            if (menuItem == contextInsertColumnBefore) return buttonColumnBefore;
            if (menuItem == contextDeleteColumn) return buttonColumnDelete;

            if (menuItem == contextEditHyperlink) return buttonInsertLink;

            return null;
        }

        /// <summary>
        /// Context menu is being shown
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            htmlControl.UpdateCurrentElement();

            contextCopy.Enabled = htmlControl.CanCopy;
            contextCut.Enabled = htmlControl.CanCut;
            contextPaste.Enabled = htmlControl.CanPaste;

            bool showTableMenus = IsInTag("td", htmlControl.CurrentElement);
            for (int i = contextMenu.Items.IndexOf(contextTableSep); i <= contextMenu.Items.IndexOf(contextDeleteColumn); i++)
            {
                contextMenu.Items[i].Visible = showTableMenus;
            }

            bool showEditLink = IsInTag("a", htmlControl.CurrentElement);
            contextHyperlinkSep.Visible = showEditLink;
            contextEditHyperlink.Visible = showEditLink;
        }

        /// <summary>
        /// Determines if th specified tagname is parent or self of the give element
        /// </summary>
        private bool IsInTag(string tagName, HtmlApi.IHTMLElement element)
        {
            int levels = 10;

            while (levels-- > 0 && element != null)
            {
                if (tagName == element.TagName.ToLower())
                {
                    return true;
                }

                element = element.ParentElement;
            }

            return false;
        }
    }
}
