using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI
{
    /// <summary>
    /// A class to help with displaying a Popup window that acts like a dropdown.
    /// </summary>
    public class PopupController
    {
        // The editor which the user interacts with
        Control control;

        // Currently open popup window, if any
        PopupWindow popupWindow;

        // Animation to open with
        PopupAnimation animation = PopupAnimation.Off;

        // The size of the popup window
        Size size = new Size(250, 350);

        // The resizability of the popup
        PopupSizerStyle sizerStyle = PopupSizerStyle.BothDirections;

        // Indicates if the dismissing click of the popup should be passed on to the target control
        bool passOnDismissClick = true;

        // How many popups are currently visible
        static int visiblePopups = 0;

        /// <summary>
        /// Raised when the Popup is closing
        /// </summary>
        public event EventHandler PopupClosing;

        /// <summary>
        /// Constructor
        /// </summary>
        public PopupController(Control control)
        {
            this.control = control;
        }

        /// <summary>
        /// The Control that the user interacts with inside the popup
        /// </summary>
        public Control Control
        {
            get { return control; }
        }

        /// <summary>
        /// The size of the drop down when popped up
        /// </summary>
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Controls the resizability of the popup window
        /// </summary>
        public PopupSizerStyle SizerStyle
        {
            get { return sizerStyle; }
            set { sizerStyle = value; }
        }

        /// <summary>
        /// Controls if the popup will be shown with animation
        /// </summary>
        public PopupAnimation Animate
        {
            get { return animation; }
            set { animation = value; }
        }

        /// <summary>
        /// Indicates if we pass on the click tha dismisses the popup. ComboBox's, for example, eat the dismissing click
        /// </summary>
        public bool PassOnDismissClick
        {
            get { return passOnDismissClick; }
            set { passOnDismissClick = value; }
        }

        /// <summary>
        /// Indicates if the popup is currently displayed
        /// </summary>
        public bool IsPopupVisible
        {
            get { return popupWindow != null; }
        }

        /// <summary>
        /// Indiciates if there are any popups in the entire UI currently visible
        /// </summary>
        public static bool IsAnyPopupVisible
        {
            get { return visiblePopups > 0; }
        }

        /// <summary>
        /// Show the Popup in the place of the given DropDown for the given menu item.  This should be called in response to a ToolStripMenuItem.Opening.
        /// Note that the method returns right away, before the Popup is actually shown.
        /// </summary>
        public void ShowMenu(ToolStripMenuItem menuItem, Form owner)
        {
            menuItem.DropDown.Closing += new ToolStripDropDownClosingEventHandler(OnToolStripDropDownClosing);

            // Keeps the real drop down from being seen
            menuItem.DropDown.TopLevel = false;

            // Calculate the location
            Point location = menuItem.Owner.PointToScreen(new Point(menuItem.Bounds.Right, menuItem.Bounds.Top));

            // Show it asyncrhously.  If we don't, we are in the call stack of this being shown, and the regular menu mouse processing does not work.
            owner.BeginInvoke((MethodInvoker) delegate
            {
                DialogResult result = ShowCore(owner, location, -menuItem.Bounds.Height, SystemInformation.IsMenuAnimationEnabled, menuItem);

                // OK means something was chosen succssfully.
                if (result == DialogResult.OK)
                {
                    menuItem.DropDown.Close(ToolStripDropDownCloseReason.ItemClicked);
                }
            });
        }

        /// <summary>
        /// When shown in menu mode, this is raise when the "real" drop down is closing.
        /// </summary>
        void OnToolStripDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (PopupClosing != null)
            {
                PopupClosing(this, EventArgs.Empty);
            }

            // Ensure we close too
            Close(DialogResult.Cancel, false);
        }

        /// <summary>
        /// Show a Popup that acts similar to a ComboBox dropdown.  The Popup is shown relative to the given control bounds, with the given parent information.
        /// </summary>
        public DialogResult ShowDropDown(Rectangle controlBounds, Control controlParent)
        {
            Point location = controlParent.PointToScreen(new Point(controlBounds.Left, controlBounds.Bottom));
            Form owner = (Form) controlParent.TopLevelControl;

            return ShowCore(owner, location, controlBounds.Height, SystemInformation.IsComboBoxAnimationEnabled, null);
        }

        /// <summary>
        /// Core method to actually show the popup
        /// </summary>
        [NDependIgnoreLongMethod]
        private DialogResult ShowCore(Form owner, Point location, int reverseOffset, bool systemAnimation, ToolStripMenuItem menuItem)
        {
            if (control == null)
            {
                throw new InvalidOperationException("Cannot show a PopupDropDown with no editor.");
            }

            if (popupWindow != null)
            {
                throw new InvalidOperationException("The PopupWindow is already shown.");
            }

            visiblePopups++;

            // Create the new Popup
            popupWindow = new PopupWindow();

            // By default assume we have enough room to drop down
            int animateDirection = NativeMethods.AW_VER_POSITIVE;
            popupWindow.SizerLocation = PopupSizerLocation.Bottom;

            // Determine which screen we're on and how big it is.
            Screen monitor = Screen.FromPoint(location);

            int spaceToBottom = monitor.Bounds.Bottom - location.Y;
            int spaceToTop = (location.Y - reverseOffset) - monitor.Bounds.Top;

            // If there is not enough room to fit going down as-is...
            if (size.Height > spaceToBottom)
            {
                // If it fits in the top - or if it also DOESNT fit in the top, but the top has the most room - we use the top
                if (size.Height < spaceToTop || spaceToTop > spaceToBottom)
                {
                    // If its too big to fit in the top as-is, adjust its height to fit
                    if (size.Height > spaceToTop)
                    {
                        size.Height = spaceToTop - 10;
                    }

                    location.Y -= (reverseOffset + size.Height);
                    animateDirection = NativeMethods.AW_VER_NEGATIVE;
                    popupWindow.SizerLocation = PopupSizerLocation.Top;

                }
                // Otherwise, just adjust the height to fit in the bottom
                else
                {
                    size.Height = spaceToBottom - 10;
                }
            }

            popupWindow.Size = size;

            popupWindow.SizerStyle = sizerStyle;
            popupWindow.MinimumSize = new Size(Math.Max(150, control.MinimumSize.Width), Math.Max(Math.Min(popupWindow.Height, 100), control.MinimumSize.Height + 15));

            popupWindow.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Visible = true;

            popupWindow.Owner = owner;
            popupWindow.OwnerMenuItem = menuItem;
            popupWindow.PassOnDismissClick = passOnDismissClick;
            popupWindow.Location = location;
            popupWindow.StartPosition = FormStartPosition.Manual;

            if (animation == PopupAnimation.On || (animation == PopupAnimation.System && systemAnimation))
            {
                NativeMethods.AnimateWindow(popupWindow.Handle, 100, NativeMethods.AW_SLIDE | animateDirection);
            }

            DialogResult result = popupWindow.ShowPopup(popupWindow.Owner, location);

            // Update to the last size
            size = popupWindow.Size;

            // If its in menu mode, its raised in OnToolStripDropDownClosing.
            if (menuItem == null && PopupClosing != null)
            {
                PopupClosing(this, EventArgs.Empty);
            }

            popupWindow.Controls.Clear();
            popupWindow.Dispose();
            popupWindow = null;

            visiblePopups--;

            return result;
        }

        /// <summary>
        /// Closes the popup.  Does nothing if the popup is not open
        /// </summary>
        public void Close(DialogResult result)
        {
            Close(result, true);
        }

        /// <summary>
        /// Closes the popup.  Does nothing if the popup is not open.  If the popup is owned by a menu, and fullyCloseMenu is true,
        /// the entire menu is caused to close.
        /// </summary>
        public void Close(DialogResult result, bool fullyCloseMenu)
        {
            if (result == DialogResult.None)
            {
                throw new InvalidOperationException("The drop-down cannot be closed with a DialogResult of None.");
            }

            if (popupWindow != null)
            {
                popupWindow.DialogResult = result;
                popupWindow.Visible = false;

                if (fullyCloseMenu && popupWindow.OwnerMenuItem != null)
                {
                    popupWindow.OwnerMenuItem.DropDown.Close(ToolStripDropDownCloseReason.ItemClicked);
                }

                // The point of this line is for when the ribbon is in 'Minimized' mode, to make sure we close the poppuped up ribbon before opening
                // any windows.  The poppepd up ribbon is top-level always-on-top... so it can't stay open.
                if (popupWindow.Owner.Visible && popupWindow.Owner.GetType().FullName.Contains("SandRibbon"))
                {
                    popupWindow.Owner.Hide();
                }
            }
        }
    }
}
