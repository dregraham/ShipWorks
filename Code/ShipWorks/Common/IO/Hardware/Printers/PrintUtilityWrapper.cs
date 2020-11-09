using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Class to get printer information for computer
    /// </summary>
    [Component]
    public class PrintUtilityWrapper : IPrintUtility
    {
        /// <summary>
        /// Get a list of installed printers
        /// </summary>
        /// <exception cref="PrintingException" />
        public List<string> InstalledPrinters => PrintUtility.InstalledPrinters;

        /// <summary>
        /// Get the currently selected default printer IPrinterSetting.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        public IPrinterSetting GetDefaultPrinterSettings() => PrinterSettingFactory.GetDefaultPrinterSettings();

        /// <summary>
        /// Get an IPrinterSetting by printer name.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        public IPrinterSetting GetPrinterSettings(string printerName) => PrinterSettingFactory.GetPrinterSettings(printerName);
    }
}
