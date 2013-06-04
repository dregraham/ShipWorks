using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShipWorks.UI.Controls
{
	/// <summary>
	/// TextBox for only entering a number
	/// </summary>
	public class NumericTextBox : TextBox
	{
        const int ES_NUMBER = 0x2000;

        string lastText = string.Empty;

        /// <summary>
        /// When the control is created set the ES_NUMBER style
        /// </summary>
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams parms = base.CreateParams;

                parms.Style |= ES_NUMBER;

                return parms;
            }
        }

        /// <summary>
        /// Text has changed
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // On XP, even with ES_NUMBER set, you can still paste in bad data
            if (!Regex.Match(Text, "^[0-9]*$").Success)
            {
                Text = lastText;
            }

            lastText = Text;
        }

        /// <summary>
        /// Gets the numeric value by parsing the text in the textbox. Null if blank.
        /// </summary>
	    public int? NumericValue
	    {
	        get
	        {
	            int testParse;
	            
                if (int.TryParse(Text, out testParse))
	            {
	                return testParse;
	            }

	            return null;
	        }
	    }
    }
}
