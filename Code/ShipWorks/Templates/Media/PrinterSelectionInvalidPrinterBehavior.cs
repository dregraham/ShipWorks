using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Controls what the PrinterSelectionControl should do if its initialized with an invalid printer
    /// </summary>
    [Flags]
    public enum PrinterSelectionInvalidPrinterBehavior
    {
        /// <summary>
        /// Never preserve invalid \ missing printers - always just selected the default.
        /// </summary>
        NeverPreserve = 0x000,

        /// <summary>
        /// If the template had been selected, but is now not on the computer, add the missing template to the list to preserve it's selection.
        /// </summary>
        OnInvalidPreserve = 0x001,

        /// <summary>
        /// If a template has never been configured, add an empty entry to the list to preserve it
        /// </summary>
        OnNotChosenPreserve = 0x002,

        /// <summary>
        /// Add a missing or not selected printer to the list so it can be selected and preserved
        /// </summary>
        AlwaysPreserve = OnInvalidPreserve | OnNotChosenPreserve
    }
}
