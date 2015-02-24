using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentFactory.Krypton.Toolkit;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared;
using System.ComponentModel;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls.Krypton
{
    /// <summary>
    /// A KryptonTextBox that provides watermark support
    /// </summary>
    public class WatermarkKryptonTextBox : KryptonTextBox
    {
        Color waterColor = SystemColors.GrayText;
        string waterText = string.Empty;

        string realText = string.Empty;
        Color realColor;

        bool ignoreChanges = false;

        /// <summary>
        /// Maybe
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            realColor = SystemColors.ControlText;
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The prompt text to display when there is nothing in the Text property.")]
        public string WaterText
        {
            get
            {
                return waterText;
            }
            set
            {
                waterText = value;

                UpdateTextDisplay();
            }
        }

        [Category("Appearance")]
        [Description("The color to use when displaying the PromptText.")]
        public Color WaterColor
        {
            get
            {
                return waterColor;
            }
            set
            {
                waterColor = value;

                UpdateTextDisplay();
            }
        }

        /// <summary>
        /// Listen for text changes
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!ignoreChanges)
            {
                realText = base.Text;

                UpdateTextDisplay();
 	            base.OnTextChanged(e);
            }
        }

        /// <summary>
        /// Override the text propery
        /// </summary>
        public override string Text
        {
            get
            {
                return realText;
            }
            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        /// Focus is entering the control
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            UpdateTextDisplay();
        }

        /// <summary>
        /// Control is losing focus
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            UpdateTextDisplay();
        }

        /// <summary>
        /// Update for the correct text
        /// </summary>
        private void UpdateTextDisplay()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            ignoreChanges = true;

            bool focused = Focused || TextBox.Focused;

            if (focused)
            {
                base.Text = realText;
                StateCommon.Content.Color1 = realColor;
            }
            else
            {
                if (string.IsNullOrEmpty(realText))
                {
                    base.Text = waterText;
                    StateCommon.Content.Color1 = waterColor;
                }
                else
                {
                    base.Text = realText;
                    StateCommon.Content.Color1 = realColor;
                }
            }

            ignoreChanges = false;
        }
    }
}
