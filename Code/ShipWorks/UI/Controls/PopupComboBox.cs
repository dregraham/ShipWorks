using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Interapptive.Shared;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A ComboBox that shows a PopupDropDown as its drop-down content
    /// </summary>
    public class PopupComboBox : ComboBox
    {
        // The drop down to show
        PopupController popupController;

        // We have to keep track of this ourself, we can't let the underlying window control know what it really
        // is to prevent it from drawing it.
        int selectedIndex = -1;

        Size? lastSize = null;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PopupComboBox()
        {
            DropDownMinimumHeight = 200;

            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Designer generated
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomComboBox
            // 
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResumeLayout(false);
        }

        [Category("Behavior"), DefaultValue(200)]
        public int DropDownMinimumHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets \ sets the control to display as the drop-down portion of the ComboBox
        /// </summary>
        public PopupController PopupController
        {
            get
            {
                return popupController;
            }
            set
            {
                popupController = value;
            }
        }

        /// <summary>
        /// Draw the item that the user has currently selected
        /// </summary>
        protected virtual void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            // This should probably have an event that gets raised.  For how we use it now, we dont need it.
        }

        /// <summary>
        /// Get the ideal size that the popup portion should be
        /// </summary>
        protected virtual Size GetIdealPopupSize()
        {
            return new Size(DropDownWidth, DropDownHeight);
        }

        /// <summary>
        /// Intercept selected index
        /// </summary>
        public override int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (selectedIndex == value)
                {
                    return;
                }

                selectedIndex = value;

                Invalidate();
                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Can be overridden by derived classes for notification of when the drop down is shown
        /// </summary>
        protected virtual void OnShowingDropDown()
        {

        }

        /// <summary>
        /// Intercept the mouse down
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // On mouse down, we show our color chooser
            if (m.Msg == NativeMethods.WM_LBUTTONDOWN || m.Msg == NativeMethods.WM_LBUTTONDBLCLK)
            {
                if (GetStyle(ControlStyles.Selectable))
                {
                    Focus();
                }

                Refresh();

                if (popupController != null)
                {
                    Size popupSize;

                    if (lastSize != null)
                    {
                        popupSize = lastSize.Value;
                    }
                    else
                    {
                        Size idealSize = GetIdealPopupSize();

                        // Account for the bottom resizer bar
                        idealSize.Height += SystemInformation.HorizontalScrollBarHeight;

                        // Make it start out at least as wide as the ComboBox
                        idealSize.Width = Math.Max(idealSize.Width, Width);

                        // But don't let it start out wider than 400px (Unless the combo is already that big)
                        idealSize.Width = Math.Min(idealSize.Width, Math.Max(400, Width));

                        // Start out at least 200px tall
                        idealSize.Height = Math.Max(idealSize.Height, DropDownMinimumHeight);

                        // But don't let it start out more than 1200px tall
                        idealSize.Height = Math.Min(idealSize.Height, 1200);

                        popupSize = idealSize;
                    }

                    OnShowingDropDown();

                    popupController.PassOnDismissClick = false;
                    popupController.Size = popupSize;
                    popupController.ShowDropDown(Bounds, Parent);

                    popupController.PopupClosing += new EventHandler(OnPopupClosing);
                }

                Invalidate();
            }

            else
            {
                base.WndProc(ref m);

                if (m.Msg == NativeMethods.WM_PAINT)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        Color color = ForeColor;
                        Rectangle bounds = new Rectangle(3, 3, Width - 5 - SystemInformation.VerticalScrollBarWidth, Height - 6);

                        if (popupController == null || !PopupController.IsPopupVisible)
                        {
                            if (Focused)
                            {
                                // ControlPaint.DrawFocusRectangle(g, bounds, ForeColor, BackColor);
                            }
                        }

                        if (!Enabled)
                        {
                            color = SystemColors.GrayText;
                        }

                        OnDrawSelectedItem(g, color, bounds);
                    }
                }
            }
        }

        /// <summary>
        /// The popup controller is closing
        /// </summary>
        private void OnPopupClosing(object sender, EventArgs e)
        {
            lastSize = popupController.Size;

            popupController.PopupClosing -= this.OnPopupClosing;
        }
    }
}
