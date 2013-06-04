using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A textbox which will only accept decimal values.
    /// </summary>
    public partial class DecimalTextBox : TextBox
    {
        private bool correcting;

        private int previousPosition;

        private string previousText;


        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalTextBox" /> class.
        /// </summary>
        public DecimalTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        private void OnTextChanged(object sender, EventArgs e)
        {
            if (correcting)
            {
                return;
            }

            // Matches a decimal value even if it is incomplete (ie "1." or ".")
            Match match = Regex.Match(Text, @"^(\d)*(\.){0,1}(\d)*$");

            if (match.Success)
            {
                previousPosition = SelectionStart;
                previousText = Text;
            }
            else
            {
                correcting = true;

                Text = previousText;

                SelectionStart = previousPosition;
                SelectionLength = 0;

                correcting = false;
            }
        }

        /// <summary>
        /// Called when [leave]. We know due to OnChange 
        /// number is valid or has a . at the end. This strips it.
        /// </summary>
        private void OnLeave(object sender, EventArgs e)
        {
            if (Text.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            QueryFocusPosition();
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            QueryFocusPosition();
        }

        /// <summary>
        /// Queries the focus position. If it has changed, but the text has not, reset focus.
        /// If the text has changed, OnChange will capture new focus position.
        /// </summary>
        private void QueryFocusPosition()
        {
            if (Text == previousText)
            {
                previousPosition = SelectionStart;
            }
        }
    }
}