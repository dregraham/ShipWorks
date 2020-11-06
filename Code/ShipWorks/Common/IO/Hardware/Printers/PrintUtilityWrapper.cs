using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Class to get printer information for computer
    /// </summary>
    public class PrintUtilityWrapper : IPrintUtility
    {
        /// <summary>
        /// Get a list of installed printers
        /// </summary>
        /// <exception cref="PrintingException" />
        public List<string> InstalledPrinters => PrintUtility.InstalledPrinters;
    }
}
