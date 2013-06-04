using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Reusable control for editing the street portion of an address
    /// </summary>
    internal class StreetEditor : MultiValueTextBox
    {
        private int maxLines = 3;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreetEditor()
        {
            AcceptsReturn = true;
            Multiline = true;
        }

        /// <summary>
        /// Maximum number of lines allowed in an address.
        /// </summary>
        [DefaultValue(3)]
        [Range(1, 3)]
        [Browsable(true)]
        [Category("Misc")]
        public int MaxLines
        {
            get
            {
                return maxLines;
            }
            set
            {
                if (value < 1 || value > 3)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                maxLines = value;
            }
        }

        /// <summary>
        /// Line1 of the street address
        /// </summary>
        public string Line1
        {
            get
            {
                return GetStreetLine(0);
            }
            set
            {
                SetStreetLine(0, value);
            }
        }

        /// <summary>
        /// Line2 of the street address
        /// </summary>
        public string Line2
        {
            get
            {
                return GetStreetLine(1);
            }
            set
            {
                SetStreetLine(1, value);
            }
        }

        /// <summary>
        /// Line1 of the street address
        /// </summary>
        public string Line3
        {
            get
            {
                return GetStreetLine(2);
            }
            set
            {
                SetStreetLine(2, value);
            }
        }

        /// <summary>
        /// Combines the lines and returns what the Text property of the StreetEditor would be set to for the given line set.
        /// </summary>
        public static string CombinLines(string line1, string line2, string line3)
        {
            return string.Format("{0}\r\n{1}\r\n{2}", line1, line2, line3).TrimEnd();
        }

        /// <summary>
        /// Set the stree line with the given value
        /// </summary>
        private void SetStreetLine(int line, string value)
        {
            if (line < 0 || line > 2)
            {
                throw new ArgumentOutOfRangeException("line");
            }

            if (value == null)
            {
                value = string.Empty;
            }

            // If we are trying to set a line that doesnt exist to empty, we don't need to do anything
            if (line >= Lines.Length && value.Length == 0)
            {
                return;
            }

            List<string> lines = new List<string>(Lines);

            // Make sure we have enough lines
            while (lines.Count < line + 1)
            {
                lines.Add(string.Empty);
            }

            // Set the value of this line
            lines[line] = value;

            // Save it back
            Lines = lines.ToArray();
        }

        /// <summary>
        /// Gets the value of the street line
        /// </summary>
        private string GetStreetLine(int line)
        {
            if (line < 0 || line > 2)
            {
                throw new ArgumentOutOfRangeException("line");
            }

            if (line < Lines.Length)
            {
                return Lines[line];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Don't allow return when there is already 3 lines
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys keyCode = (Keys) (int) msg.WParam & Keys.KeyCode;

            if (keyCode == Keys.Enter && Lines.Length >= 3)
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Limit the number of lines to three by truncating the end
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (Lines.Length > MaxLines)
            {
                List<string> lines = new List<string>();
                for (int i = 0; i < MaxLines; i++)
                {
                    lines.Add(Lines[i]);
                }

                Lines = lines.ToArray();

                // Keeps the caret positioned at the end
                Select(Text.Length, 1);
            }
        }
    }
}
