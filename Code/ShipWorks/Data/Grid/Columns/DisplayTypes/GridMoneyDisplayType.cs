using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying monetary values
    /// </summary>
    public class GridMoneyDisplayType : GridColumnDisplayType
    {
        bool showCurrency = true;
        bool showThousands = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridMoneyDisplayType()
        {
            // Default alignment for money will be right
            Alignment = StringAlignment.Far;
        }

        /// <summary>
        /// Show the currency symbol in front of the amount
        /// </summary>
        public bool ShowCurrency
        {
            get { return showCurrency; }
            set { showCurrency = value; }
        }

        /// <summary>
        /// Show the , for the thousands separator
        /// </summary>
        public bool ShowThousands
        {
            get { return showThousands; }
            set { showThousands = value; }
        }

        /// <summary>
        /// Create the editor used to edit the settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridMoneyDisplayEditor(this);
        }
        
        /// <summary>
        /// Get the string to use for formatting the value
        /// </summary>
        protected override string GetValueFormatString()
        {
            string format = showThousands ? "#,##0.00" : "0.00";

            if (showCurrency)
            {
                format = "$" + format;
            }

            return "{0:" + format + "}";
        }

        /// <summary>
        /// Money columns can be a little smaller
        /// </summary>
        public override int DefaultWidth
        {
            get
            {
                return 80;
            }
        }
    }
}
