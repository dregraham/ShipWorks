using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Extraction of PrinterSettings into an interface 
    /// </summary>
    public interface IPrinterSetting
    {
        /// <summary>
        ///  The printer's name
        /// </summary>
        string PrinterName { get; }

        /// <summary>
        ///  Is the printer valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Default or working page settings
        /// </summary>
        PageSettings DefaultPageSettings { get; }

        /// <summary>
        /// The printer's paper sources
        /// </summary>
        PrinterSettings.PaperSourceCollection PaperSources { get; }

        /// <summary>
        /// The printer's paper sources
        /// </summary>
        PaperSource PaperSource { get; }

        /// <summary>
        /// The printer's selected paper source name
        /// </summary>
        string PaperSourceName { get; }
    }
}