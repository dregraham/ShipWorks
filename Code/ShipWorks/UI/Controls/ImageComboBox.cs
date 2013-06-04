using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Diagnostics;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Class for displaying images next to ComboBox items
    /// </summary>
    public class ImageComboBox : PopupComboBox
	{
        const int imageSize = 16;

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

                SetStyle(ControlStyles.ResizeRedraw, true);
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

                object item = Items[e.Index];

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

                    if (oldHoverIndex != -1)
                    {
                        Invalidate(GetItemRectangle(oldHoverIndex));
                    }

                    if (hoverIndex != -1)
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

                    Invalidate(GetItemRectangle(oldHoverIndex));
                }
            }
        }

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

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
		}

        /// <summary>
        /// Dropdown is showing, we have to fill the box
        /// </summary>
        protected override void OnShowingDropDown()
        {
            listBox.SelectedIndexChanged -= new EventHandler(OnSelectItem);

            listBox.Items.Clear();

            foreach (object item in Items)
            {
                listBox.Items.Add(item);
            }

            if (SelectedIndex >= 0)
            {
                listBox.SelectedIndex = SelectedIndex;
            }

            listBox.SelectedIndexChanged += new EventHandler(OnSelectItem);
        }

        /// <summary>
        /// Selected item has changed
        /// </summary>
        void OnSelectItem(object sender, EventArgs e)
        {
            SelectedIndex = listBox.SelectedIndex;

            PopupController.Close(DialogResult.OK);
        }

        /// <summary>
        /// Draw the selected filter
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            if (SelectedIndex < 0)
            {
                return;
            }

            object item = Items[SelectedIndex];

            DropDownListBox.DrawImageAndText(item, g, Font, foreColor, bounds);
        }
	}
}