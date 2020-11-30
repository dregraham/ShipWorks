using System.Collections.Generic;
using ShipWorks.Templates.Printing;

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

        /// <summary>
        /// Get an IPrinterSetting by printer name.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        IPrinterSetting GetPrinterSettings(string printerName);

        /// <summary>
        /// Get the default printer settings
        /// </summary>
        IPrinterSetting GetDefaultPrinterSettings();
    }
}