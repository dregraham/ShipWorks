using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.ComponentModel;
using Interapptive.Shared;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;

//Original: http://blogs.msdn.com/jfoscoding/articles/491523.aspx
//Wyatt's fixes: http://wyday.com/blog/csharp-splitbutton
namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A button that has a down-arrow on the right with a context menu
    /// </summary>
    public class DropDownButton : Button
    {
        PushButtonState buttonState;
        bool skipNextOpen = false;
        Rectangle dropDownMouseArea = new Rectangle();

        bool actAsDropDown = true;

        ContextMenuStrip contextMenu = null;
        SandContextPopup sandPopup = null;

        const int pushButtonWidth = 14;
        static int borderSize = SystemInformation.Border3DSize.Width * 2;

        bool alwaysDrawSplitLine = true;
        bool splitButton = true;

        bool contextMenuVisible = false;
        public event EventHandler DropDownShowing;

       /// <summary>
        /// Constructor
        /// </summary>
        public DropDownButton()
        {
            this.AutoSize = true;
        }

        #region Properties

        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return contextMenu;
            }
            set
            {
                contextMenu = value;
            }
        }

        [DefaultValue(null)]
        public ContextMenuStrip SplitContextMenu
        {
            get
            {
                return contextMenu;
            }
            set
            {
                if (contextMenu != null)
                {
                    contextMenu.Closed -= new ToolStripDropDownClosedEventHandler(OnSplitMenuClosed);
                    contextMenu.Opening -= new CancelEventHandler(OnSplitMenuOpening);
                }

                contextMenu = value;

                if (contextMenu != null)
                {
                    SplitSandPopupMenu = null;

                    contextMenu.Closed += new ToolStripDropDownClosedEventHandler(OnSplitMenuClosed);
                    contextMenu.Opening += new CancelEventHandler(OnSplitMenuOpening);
                }
            }
        }

        [DefaultValue(null)]
        public SandContextPopup SplitSandPopupMenu
        {
            get
            {
                return sandPopup;
            }
            set
            {
                if (sandPopup != null)
                {
                    sandPopup.BeforePopup -= OnSandPopupOpening;
                }

                sandPopup = value;

                if (sandPopup != null)
                {
                    SplitContextMenu = null;

                    sandPopup.BeforePopup += OnSandPopupOpening;
                }
            }
        }

        /// <summary>
        /// Controls whether the line between the main button and the split is always drawn,
        /// or is drawn only when the control is hot.
        /// </summary>
        [DefaultValue(true)]
        public bool AlwaysDrawSplitLine
        {
            get
            {
                return alwaysDrawSplitLine;
            }
            set
            {
                if (value != alwaysDrawSplitLine)
                {
                    alwaysDrawSplitLine = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Controls if there is a splitter, or if the whole button is a drop down
        /// </summary>
        [DefaultValue(true)]
        public bool SplitButton
        {
            get
            {
                return splitButton;
            }
            set
            {
                if (value != splitButton)
                {
                    splitButton = value;
                    Invalidate();

                    if (this.Parent != null)
                    {
                        this.Parent.PerformLayout();
                    }
                }
            }
        }

        /// <summary>
        /// State of how the button is drawn
        /// </summary>
        private PushButtonState State
        {
            get
            {
                return buttonState;
            }
            set
            {
                if (!buttonState.Equals(value))
                {
                    buttonState = value;
                    Invalidate();
                }
            }
        }

        #endregion Properties

        #region Mouse \ Keyboard

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);

            if (actAsDropDown && !string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + pushButtonWidth > preferredSize.Width)
            {
                return preferredSize + new Size(pushButtonWidth + borderSize * 2, 0);
            }

            return preferredSize;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData.Equals(Keys.Down) && actAsDropDown)
            {
                return true;
            }

            else
            {
                return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnGotFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Default;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (actAsDropDown)
            {
                if (e.KeyCode.Equals(Keys.Down))
                {
                    ShowContextMenuStrip();
                }

                else if (e.KeyCode.Equals(Keys.Space) && e.Modifiers == Keys.None)
                {
                    if (splitButton)
                    {
                        State = PushButtonState.Pressed;
                    }
                    else
                    {
                        ShowContextMenuStrip();
                    }
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Space))
            {
                if (Control.MouseButtons == MouseButtons.None)
                {
                    if (splitButton)
                    {
                        State = PushButtonState.Normal;
                    }
                }
            }
            else if (e.KeyCode.Equals(Keys.Apps))
            {
                if (Control.MouseButtons == MouseButtons.None)
                {
                    ShowContextMenuStrip();
                }
            }

            base.OnKeyUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                State = PushButtonState.Normal;
            else
                State = PushButtonState.Disabled;

            base.OnEnabledChanged(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnLostFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Normal;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnMouseDown(e);
                return;
            }

            if (dropDownMouseArea.Contains(e.Location) && !contextMenuVisible && e.Button == MouseButtons.Left)
            {
                ShowContextMenuStrip();
            }
            else
            {
                State = PushButtonState.Pressed;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnMouseEnter(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Hot;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnMouseLeave(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                if (Focused)
                {
                    State = PushButtonState.Default;
                }

                else
                {
                    State = PushButtonState.Normal;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!actAsDropDown)
            {
                base.OnMouseUp(e);
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                ShowContextMenuStrip();
            }
            else if (!contextMenuVisible)
            {
                SetButtonDrawState();

                if (Bounds.Contains(Parent.PointToClient(Cursor.Position)) && !dropDownMouseArea.Contains(e.Location))
                {
                    OnClick(new EventArgs());
                }
            }
        }

        #endregion

        #region Context Menu

        private void ShowContextMenuStrip()
        {
            if (skipNextOpen)
            {
                // we were called because we're closing the context menu strip
                // when clicking the dropdown button.
                skipNextOpen = false;
                return;
            }

            if (DropDownShowing != null)
            {
                DropDownShowing(this, EventArgs.Empty);
            }

            State = PushButtonState.Pressed;

            if (contextMenu != null)
            {
                contextMenu.Width = Math.Min(contextMenu.Width, Width);
                contextMenu.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }

            if (sandPopup != null)
            {
                var selectedItem = sandPopup.ShowStandalone(this, new Point(0, Height), false);

                OnSandPopupClosed(selectedItem);
            }
        }

        void OnSplitMenuOpening(object sender, CancelEventArgs e)
        {
            if (contextMenuVisible)
            {
                e.Cancel = true;
            }
            else
            {
                contextMenuVisible = true;
            }
        }

        void OnSandPopupOpening(object sender, Divelements.SandRibbon.BeforePopupEventArgs e)
        {
            if (contextMenuVisible)
            {
                e.Cancel = true;
            }
            else
            {
                contextMenuVisible = true;
            }
        }

        void OnSplitMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            contextMenuVisible = false;

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                skipNextOpen = (dropDownMouseArea.Contains(this.PointToClient(Cursor.Position))) && Control.MouseButtons == MouseButtons.Left;
            }
        }


        private void OnSandPopupClosed(Divelements.SandRibbon.WidgetBase selectedItem)
        {
            contextMenuVisible = false;

            SetButtonDrawState();

            // Doesn't exactly tell us if they closed it by clicking, but we can try to guess
            if (selectedItem != null)
            {
                skipNextOpen = (dropDownMouseArea.Contains(this.PointToClient(Cursor.Position))) && Control.MouseButtons == MouseButtons.Left;
            }
        }


        #endregion

        #region Rendering

        /// <summary>
        /// Paint the button.  The heart of the control.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!actAsDropDown)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle bounds = this.ClientRectangle;

            // draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles)
            {
                Rectangle backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);
                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // button renderer doesnt draw the black frame when themes are off =(
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
            else
            {
                ButtonRenderer.DrawButton(g, bounds, State);
            }

            Rectangle dropDownRectangle = new Rectangle(bounds.Right - pushButtonWidth - 2, 0, pushButtonWidth, bounds.Height);

            // calculate the current dropdown rectangle.
            if (splitButton)
            {
                dropDownMouseArea = dropDownRectangle;
            }
            else
            {
                dropDownMouseArea = bounds;
            }

            int internalBorder = borderSize;
            Rectangle focusRect =
                new Rectangle(internalBorder - 1,
                              internalBorder - 1,
                              bounds.Width - internalBorder,
                              bounds.Height - (internalBorder * 2) + 2);

            if (splitButton)
            {
                focusRect.Width = focusRect.Width - dropDownRectangle.Width;
            }

            bool drawSplitLine = splitButton && (alwaysDrawSplitLine || (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles));

            if (RightToLeft == RightToLeft.Yes)
            {
                dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = dropDownRectangle.Right;

                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Left + pushButtonWidth, borderSize, bounds.Left + pushButtonWidth, bounds.Bottom - borderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Left + pushButtonWidth + 1, borderSize, bounds.Left + pushButtonWidth + 1, bounds.Bottom - borderSize);
                }
            }
            else
            {
                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Right - pushButtonWidth, borderSize, bounds.Right - pushButtonWidth, bounds.Bottom - borderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Right - pushButtonWidth - 1, borderSize, bounds.Right - pushButtonWidth - 1, bounds.Bottom - borderSize);
                }
            }

            // Draw an arrow in the correct location
            PaintArrow(g, dropDownRectangle);

            // Paint the image that goes with the button
            if (this.Image != null)
            {
                PaintImage(g, this.Image);
            }

            // Figure out how to draw the text
            TextFormatFlags formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            // If we dont' use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
            if (!UseMnemonic)
            {
                formatFlags = formatFlags | TextFormatFlags.NoPrefix;
            }
            else if (!ShowKeyboardCues)
            {
                formatFlags = formatFlags | TextFormatFlags.HidePrefix;
            }


            if (!string.IsNullOrEmpty(this.Text))
            {
                if (Enabled)
                    TextRenderer.DrawText(g, Text, Font, focusRect, SystemColors.ControlText, formatFlags);
                else
                    TextRenderer.DrawText(g, Text, Font, focusRect, SystemColors.ButtonShadow, formatFlags);
            }

            // draw the focus rectangle.
            if (State != PushButtonState.Pressed && Focused && !Application.RenderWithVisualStyles)
            {
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        /// <summary>
        /// Paint the button image
        /// </summary>
        private void PaintImage(Graphics g, Image image)
        {
            Rectangle imageBounds = new Rectangle(borderSize, (Height / 2) - (image.Height / 2), image.Width, image.Height);
            if (!Enabled)
            {
                ControlPaint.DrawImageDisabled(g, image, imageBounds.X, imageBounds.Y, BackColor);
            }
            else
            {
                g.DrawImage(image, imageBounds.X, imageBounds.Y, image.Width, image.Height);
            }
        }

        /// <summary>
        /// Draw the down arrow
        /// </summary>
        private void PaintArrow(Graphics g, Rectangle dropDownRect)
        {
            Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            //if the width is odd - favor pushing it over one pixel left.
            middle.X -= (dropDownRect.Width % 2);

            Point[] arrow = new Point[] { new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1), new Point(middle.X, middle.Y + 2) };

            if (Enabled)
                g.FillPolygon(SystemBrushes.ControlText, arrow);
            else
                g.FillPolygon(SystemBrushes.ButtonShadow, arrow);
        }

        /// <summary>
        /// Set the current state of but button based on the state of the control
        /// </summary>
        private void SetButtonDrawState()
        {
            if (Parent != null && Bounds.Contains(Parent.PointToClient(Cursor.Position)))
            {
                State = PushButtonState.Hot;
            }
            else if (Focused)
            {
                State = PushButtonState.Default;
            }
            else if (!Enabled)
            {
                State = PushButtonState.Disabled;
            }
            else
            {
                State = PushButtonState.Normal;
            }
        }

        #endregion
    }
}