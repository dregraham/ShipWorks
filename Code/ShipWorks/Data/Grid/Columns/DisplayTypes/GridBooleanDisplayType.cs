using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column DisplayType for showing a Boolean
    /// </summary>
    class GridBooleanDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridBooleanDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;

            TrueText = "True";
            FalseText = "False";
        }

        /// <summary>
        /// Text to display when true
        /// </summary>
        public string TrueText { get; set; }

        /// <summary>
        /// Text to display when false
        /// </summary>
        public string FalseText { get; set; }
       
        /// <summary>
        /// Return TrueText or FalseText (or blank)
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            string displayText = string.Empty;
            
            if (value != null)
            {
                bool state = (bool)value;
            
                displayText = state ? TrueText : FalseText;
            }
            
            return displayText;
        }
    }
}
