using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Details about what type of printer a printe ris
    /// </summary>
    public class PrinterType
    {
        PrinterTechnology technology;
        ThermalLanguage thermalLanguage;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterType(PrinterTechnology technology, ThermalLanguage thermalLanguage = ThermalLanguage.None)
        {
            this.technology = technology;
            this.thermalLanguage = thermalLanguage;
        }

        /// <summary>
        /// The technology this printer uses to print
        /// </summary>
        public PrinterTechnology Technology
        {
            get { return technology; }
        }

        /// <summary>
        /// The thermal language used, if this printer is a thermal printer
        /// </summary>
        public ThermalLanguage ThermalLanguage
        {
            get { return thermalLanguage; }
        }
    }
}
