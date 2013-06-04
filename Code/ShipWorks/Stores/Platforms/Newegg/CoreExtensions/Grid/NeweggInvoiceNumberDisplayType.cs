using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;

namespace ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Grid
{
    public class NeweggInvoiceNumberDisplayType : GridTextDisplayType
    {
        /// <summary>
        /// Get the display text to use for the given value and entity
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetDisplayText(object value)
        {
            string displayText = string.Empty;

            if (value != null && value.ToString() != "0")
            {
                displayText = value.ToString();
            }

            return displayText;

        }
    }
}
