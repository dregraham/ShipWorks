using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Drawing;
using Interapptive.Shared.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// TextBox that formats its display as a truncated path.
    /// </summary>
    public class PathTextBox : TextBox
    {
        string original;

        // Flag thats on when we are updating the displayed text.
        bool updating = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public PathTextBox()
        {
            SizeChanged += new EventHandler(OnSizeChanged);
        }

        /// <summary>
        /// Get \ sets the path.  The original path string is always returned, regardless of what is displayed.
        /// </summary>
        public override string Text
        {
            get
            {
                if (!updating && original != null)
                {
                    return original;
                }

                return base.Text;
            }
            set
            {
                original = value;

                UpdateDisplayedText();
            }
        }

        /// <summary>
        /// When the size of the text box changes we need to update the truncation.
        /// </summary>
        void OnSizeChanged(object sender, EventArgs e)
        {
            UpdateDisplayedText();
        }

        /// <summary>
        /// Update the text thats displayed in the control.
        /// </summary>
        private void UpdateDisplayedText()
        {
            string trimmed = string.IsNullOrEmpty(original) ? "" : PathUtility.CompactPath(original, Width - 2);

            if (trimmed != base.Text)
            {
                updating = true;
                base.Text = trimmed;
                updating = false;
            }
        }

        /// <summary>
        /// Text of the control is changing
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            // If not updating, the user is typing
            if (!updating)
            {
                original = base.Text;
            }

            base.OnTextChanged(e);
        }
    }
}
