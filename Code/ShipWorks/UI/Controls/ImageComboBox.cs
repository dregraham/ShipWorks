using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Image = System.Drawing.Image;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Class for displaying images next to ComboBox items
    /// </summary>
    public class ImageComboBox : PopupComboBox
	{
        const int imageSize = 16;

        #region DropDownListBox

        // Custom ListBox for displaying the dropdown items
        class DropDownListBox : ListBox
        {
            int hoverIndex = -1;

            /// <summary>
            /// Constructor
            /// </summary>
            public DropDownListBox()
            {
                this.DrawMode = DrawMode.OwnerDrawFixed;
                this.ItemHeight = 18;
                this.IntegralHeight = false;

                SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            }

            /// <summary>
            /// Overridden to draw each item.
            /// </summary>
            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (Items.Count == 0 || e.Index < 0)
                {
                    base.OnDrawItem(e);
                    return;
                }

                // Use original bounds here
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                Color textColor = SystemColors.WindowText;

                object item = Items[e.Index];
                ImageComboBoxItem imageItem = item as ImageComboBoxItem;

                // If it's a plain item, or an ImageComboBoxItem that actually is selectable...
                if (imageItem == null || imageItem.Selectable)
                {
                    // Draw selected
                    if ((e.State & DrawItemState.Selected) != 0 && hoverIndex < 0)
                    {
                        hoverIndex = e.Index;

                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                        textColor = SystemColors.HighlightText;
                    }
                    // Draw hovered
                    else if (hoverIndex == e.Index)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                        textColor = SystemColors.HighlightText;
                    }
                }

                // Shrink the bounds
                Rectangle bounds = e.Bounds;
                bounds.Inflate(-1, -1);

                DrawImageAndText(item, e.Graphics, Font, textColor, bounds);
            }

            /// <summary>
            /// Draw the image and text for the given item at the specified bounds
            /// </summary>
            public static void DrawImageAndText(object item, Graphics g, Font font, Color textColor, Rectangle bounds)
            {
                string text;
                Image image;

                // Check if item is an ImageComboBoxItem
                if (item is ImageComboBoxItem)
                {
                    ImageComboBoxItem imageItem = (ImageComboBoxItem) item;

                    text = imageItem.Text;
                    image = imageItem.Image;
                }
                // Just a standard item to be drawn
                else
                {
                    text = item.ToString();
                    image = null;
                }

                // See if there is an image
                if (image != null)
                {
                    g.DrawImage(image, bounds.Left, bounds.Top, image.Width, image.Height);

                    bounds.X += 18;
                    bounds.Width -= 18;
                }

                Divelements.SandRibbon.Rendering.TextFormattingInformation format = new Divelements.SandRibbon.Rendering.TextFormattingInformation();
                format.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
                format.TextFormatFlags = TextFormatFlags.VerticalCenter;

                Divelements.SandRibbon.Rendering.IndependentText.DrawText(g, text, font, bounds, format, textColor);

                format.Dispose();
            }

            /// <summary>
            /// Track mouse movements
            /// </summary>
            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                int newHoverIndex = this.IndexFromPoint(this.PointToClient(Control.MousePosition));

                if (newHoverIndex != hoverIndex)
                {
                    int oldHoverIndex = hoverIndex;
                    hoverIndex = newHoverIndex;

                    if (oldHoverIndex >= 0 && oldHoverIndex < Items.Count)
                    {
                        Invalidate(GetItemRectangle(oldHoverIndex));
                    }

                    if (hoverIndex >= 0 && hoverIndex < Items.Count)
                    {
                        Invalidate(GetItemRectangle(hoverIndex));
                    }
                }
            }

            /// <summary>
            /// The mouse is leaving the bounds of the control
            /// </summary>
            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);

                // Ensure the hover look goes away
                if (hoverIndex != -1)
                {
                    int oldHoverIndex = hoverIndex;
                    hoverIndex = -1;

                    if (oldHoverIndex >= 0 && oldHoverIndex < Items.Count)
                    {
                        Invalidate(GetItemRectangle(oldHoverIndex));
                    }
                }
            }

            /// <summary>
            /// Invalidate the rect that contains the given image
            /// </summary>
            public void InvalidateImage(Image image)
            {
                int index = -1;

                foreach (ImageComboBoxItem imageItem in Items.OfType<ImageComboBoxItem>())
                {
                    if (imageItem.Image == image)
                    {
                        index = Items.IndexOf(imageItem);
                    }
                }

                if (index >= 0)
                {
                    Rectangle rect = GetItemRectangle(index);
                    rect.Width = 18;

                    Invalidate(rect);
                }
            }
        }

        #endregion

        DropDownListBox listBox = new DropDownListBox();

		/// <summary>
		/// Constructor
		/// </summary>
		public ImageComboBox()
		{
            // Create the drop-down
            PopupController popupController = new PopupController(listBox);
            popupController.Animate = PopupAnimation.System;
            popupController.SizerStyle = PopupSizerStyle.None;
            popupController.PopupClosing += OnClosingDropDown;

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
		}

        /// <summary>
        /// Disposing
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Dropdown is showing, we have to fill the box
        /// </summary>
        protected override void OnShowingDropDown()
        {
            RefreshItemList();

            AnimateImages(true);
        }

        /// <summary>
        /// The DropDown is closing
        /// </summary>
        private void OnClosingDropDown(object sender, EventArgs e)
        {
            AnimateImages(false);
        }

        /// <summary>
        /// Start or stop animating all images in our current list
        /// </summary>
        private void AnimateImages(bool animate)
        {
            // Go through each image we have and see if any need animated
            foreach (ImageComboBoxItem imageItem in listBox.Items.OfType<ImageComboBoxItem>())
            {
                if (imageItem.Image != null && ImageAnimator.CanAnimate(imageItem.Image))
                {
                    if (animate)
                    {
                        ImageAnimator.Animate(imageItem.Image, OnImageAnimateFrameChanged);
                    }
                    else
                    {
                        ImageAnimator.StopAnimate(imageItem.Image, OnImageAnimateFrameChanged);
                    }
                }
            }
        }

        /// <summary>
        /// Frames have been updated and are ready to be redrawn
        /// </summary>
        private void OnImageAnimateFrameChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => OnImageAnimateFrameChanged(sender, e)));
                return;
            }

            Image image = (Image)sender;

            ImageAnimator.UpdateFrames(image);

            listBox.InvalidateImage(image);
        }

        /// <summary>
        /// Refresh the item list from the Items collection
        /// </summary>
        public void RefreshItemList()
        {
            AnimateImages(false);

            listBox.SelectedIndexChanged -= new EventHandler(OnSelectItem);
            listBox.Items.Clear();

            foreach (object item in Items)
            {
                listBox.Items.Add(item);
            }

            if (SelectedIndex >= 0 && SelectedIndex < listBox.Items.Count)
            {
                listBox.SelectedIndex = SelectedIndex;
            }

            listBox.SelectedIndexChanged += new EventHandler(OnSelectItem);
            
            // If the popup is visible, go ahead and restart the animation
            if (PopupController.IsPopupVisible)
            {
                AnimateImages(true);
            }
        }

        /// <summary>
        /// Selected item has changed
        /// </summary>
        void OnSelectItem(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0)
            {
                return;
            }

            ImageComboBoxItem imageItem = Items[listBox.SelectedIndex] as ImageComboBoxItem;

            // Not selectable
            if (imageItem != null && !imageItem.Selectable)
            {
                return;
            }

            SelectedIndex = listBox.SelectedIndex;

            // Check for closable
            if (imageItem == null || imageItem.CloseOnSelect)
            {
                PopupController.Close(DialogResult.OK);
            }
        }

        /// <summary>
        /// Draw the selected item.  Only applies when style is DropDownList
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            if (SelectedIndex < 0 || DropDownStyle != ComboBoxStyle.DropDownList)
            {
                return;
            }

            object item = Items[SelectedIndex];

            DropDownListBox.DrawImageAndText(item, g, Font, foreColor, bounds);
        }

        /// <summary>
        /// Override command key processing
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // The user wants to select the previous item in the list
            if (keyData == Keys.Down || keyData == Keys.Right)
            {
                if (listBox.SelectedIndex < Items.Count - 1)
                {
                    listBox.SelectedIndex++;    
                }
                
                return true;
            }

            // The user wants to select the next item in the list
            if (keyData == Keys.Up || keyData == Keys.Left)
            {
                if (listBox.SelectedIndex > 0)
                {
                    listBox.SelectedIndex--;    
                }
                
                return true;
            }

            if (keyData >= Keys.A && keyData <= Keys.Z)
            {
                // Find the first occurrance of an item that begins with the specified letter that's AFTER the currently selected item.
                // If that fails, try from the beginning.  That will let someone cycle through all the items that begin with a specific letter.
                object foundItem = Items.Cast<Object>().Skip(listBox.SelectedIndex + 1).FirstOrDefault(x => ItemTextBeginsWith(x, keyData)) ??
                                   Items.Cast<Object>().Take(listBox.SelectedIndex + 1).FirstOrDefault(x => ItemTextBeginsWith(x, keyData));

                if (foundItem != null)
                {
                    listBox.SelectedIndex = Items.IndexOf(foundItem);
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Gets whether the specified combo box item's text begins with the specified letter
        /// </summary>
        /// <param name="item">Combo box item to check</param>
        /// <param name="key">Letter that will be tested</param>
        /// <returns>True if the item begins with the specified letter</returns>
        private static bool ItemTextBeginsWith(object item, Keys key)
        {
            ImageComboBoxItem listItem = item as ImageComboBoxItem;
            string itemText = (listItem != null) ? listItem.Text : item.ToString();

            return itemText.StartsWith(key.ToString(), StringComparison.OrdinalIgnoreCase);
        }
	}
}