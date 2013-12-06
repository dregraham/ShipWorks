using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Specified what type of technology a printer uses to print
    /// </summary>
    public enum PrinterTechnology
    {
        /// <summary>
        /// Inkjet \ laster
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Thermal (may not support EPL or ZPL though)
        /// </summary>
        Thermal = 1
    }
}
