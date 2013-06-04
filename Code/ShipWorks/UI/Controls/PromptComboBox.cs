using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using ShipWorks.Common;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// ComboBox that can display prompt text on top of it without having to be an item
    /// </summary>
    public class PromptComboBox : ComboBox
    {
        string promptText = string.Empty;

        /// <summary>
        /// This is used to draw the prompt when the ComboBox is a DropDown, and not a DropDownList.  When its a 
        /// DropDown, it uses an enter Edit control, which we subclass to do the drawing.
        /// </summary>
        class InnerTextBoxHook : NativeWindow
        {
            PromptComboBox parent;

            /// <summary>
            /// Constructor
            /// </summary>
            public InnerTextBoxHook(PromptComboBox parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// WndPrc of subclassed child edit control
            /// </summary>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == NativeMethods.WM_PAINT && !parent.Focused && parent.ShouldDrawPrompt())
                {
                    using (Graphics g = Graphics.FromHwnd(m.HWnd))
                    {
                        parent.DrawTextPrompt(g, new Rectangle(0, 0, parent.Width - 10, parent.Height - 4));
                    }
                }
            }
        }

        InnerTextBoxHook textBoxHook = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public PromptComboBox()
        {

        }

        /// <summary>
        /// The text that will be displayed as the prompt.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public virtual string PromptText
        {
            get 
            { 
                return promptText; 
            }
            set
            {
                promptText = value;

                if (promptText == null)
                {
                    promptText = string.Empty;
                }

                Invalidate();
            }
        }
    
        /// <summary>
        /// Can be overridden by derived class to control when the prompt is displayed.  By default it's when there are no Items.
        /// </summary>
        protected virtual bool ShouldDrawPrompt()
        {
            return Items.Count == 0 && !string.IsNullOrEmpty(PromptText);
        }

        /// <summary>
        /// When our handle is created we need to update the hook
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            UpdateTextBoxHook();
        }

        /// <summary>
        /// The DropDown style has changed
        /// </summary>
        protected override void OnDropDownStyleChanged(EventArgs e)
        {
            base.OnDropDownStyleChanged(e);

            UpdateTextBoxHook();
        }

        /// <summary>
        /// Update the hooking for our drawing
        /// </summary>
        private void UpdateTextBoxHook()
        {
            if (textBoxHook != null)
            {
                textBoxHook.ReleaseHandle();
                textBoxHook = null;
            }

            if (DropDownStyle == ComboBoxStyle.DropDown)
            {
                object data = IntPtr.Zero;
                NativeMethods.EnumChildWindows(Handle, new NativeMethods.EnumWindowsCallback(CreateTextBoxHookCallback), ref data);
            }
        }

        /// <summary>
        /// Callback to pass to EnumChildWindows.
        /// </summary>
        private bool CreateTextBoxHookCallback(IntPtr hWnd, ref object lParam)
        {
            textBoxHook = new InnerTextBoxHook(this);
            textBoxHook.AssignHandle(hWnd);

            return false;
        }

        /// <summary>
        /// Overridden for custom drawing of the prompt
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Only draw the prompt on the WM_PAINT event and its multivalued
            if (m.Msg == NativeMethods.WM_PAINT && DropDownStyle == ComboBoxStyle.DropDownList && ShouldDrawPrompt())
            {
                using (Graphics g = CreateGraphics())
                {
                    Rectangle rect = this.ClientRectangle;
                    rect.Offset(3, 3);

                    DrawTextPrompt(g, rect);
                }
            }
        }

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        protected void DrawTextPrompt(Graphics g, Rectangle rect)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.Left;

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, PromptText, Font, rect, SystemColors.GrayText, Color.Transparent, flags);
        }

        /// <summary>
        /// Selected value was changed. Handle deprecated enumeration values. 
        /// </summary>
        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);

            // if this control is enabled and attached to a ShipWorksBindingSource, we need to make sure only valid items are selected
            if (DataSource != null && Enabled && SelectedValue != null && DataSource is ActiveEnumerationBindingSource)
            {
                ActiveEnumerationBindingSource bindingSource = DataSource as ActiveEnumerationBindingSource;

                if (EnumHelper.GetDeprecated((Enum)SelectedValue))
                {
                    // a Deprecated value was selected in the dropdown.  Deprecated values are put at the end, 
                    // so select the first one instead.
                    if (Items.Count > 1)
                    {
                        // select the first option
                        SelectedIndex = 0;

                        // signal the binding source to remove any obsolete values from its list
                        bindingSource.RemoveAllDeprecated();
                    }
                }
            }
        }
    }
}
