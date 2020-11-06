using System.Collections.Generic;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Gets printer information for computer
    /// </summary>
    public interface IPrintUtility
    {
        /// <summary>
        /// Get a list of installed printers
        /// </summary>
        /// <exception cref="PrintingException" />
        List<string> InstalledPrinters { get; }
    }
}