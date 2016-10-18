using System.Drawing.Printing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Wrapper around PrinterSettings
    /// </summary>
    public class PrinterSetting : IPrinterSetting
    {
        private PrinterSettings printerSettings;
        private string paperSourceName = string.Empty;

        /// <summary>
        /// Constructor for creating a PrinterSetting based on a Windows PrinterSettings
        /// </summary>
        /// <param name="printerSettings"></param>
        public PrinterSetting(PrinterSettings printerSettings)
        {
            this.printerSettings = printerSettings;
        }

        /// <summary>
        ///  The printer's name
        /// </summary>
        public string PrinterName => printerSettings.PrinterName;

        /// <summary>
        ///  Is the printer valid
        /// </summary>
        public bool IsValid => printerSettings.IsValid;

        /// <summary>
        /// Default or working page settings
        /// </summary>
        public PageSettings DefaultPageSettings => printerSettings.DefaultPageSettings;

        /// <summary>
        /// The printer's paper sources
        /// </summary>
        public PrinterSettings.PaperSourceCollection PaperSources => printerSettings.PaperSources;

        /// <summary>
        /// The printer's paper sources
        /// </summary>
        public PaperSource PaperSource => DefaultPageSettings.PaperSource;

        /// <summary>
        /// The printer's selected paper source name
        /// </summary>
        public string PaperSourceName
        {
            get
            {
                if (string.IsNullOrEmpty(paperSourceName))
                {
                    foreach (PaperSource source in PaperSources)
                    {
                        if (source.RawKind == PaperSource.RawKind)
                        {
                            paperSourceName = source.SourceName;
                            break;
                        }
                    }
                }

                return paperSourceName;
            }
        }
    }
}
