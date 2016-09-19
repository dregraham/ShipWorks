using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using log4net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Contains settings for a print job
    /// </summary>
    public class PrintJobSettings
    {
        // The printer and tray to use
        string printerName;
        int paperSource;
        string _paperSourceName;

        // Number of copies of the processed output desired
        int copies = 1;
        bool collate = false;

        // Page and label settings
        PrintJobPageSettings pageSettings;
        PrintJobLabelSettings labelSettings;

        // Indicates if the print job is for a thermal label
        bool isThermal = false;

        /// <summary>
        /// Construct using non-label page settings
        /// </summary>
        public PrintJobSettings(PrintJobPageSettings pageSettings, bool isThermal)
        {
            if (pageSettings == null)
            {
                throw new ArgumentNullException("pageSettings");
            }

            this.pageSettings = pageSettings;
            this.isThermal = isThermal;
        }

        /// <summary>
        /// Construct using label settings
        /// </summary>
        public PrintJobSettings(PrintJobLabelSettings labelSettings)
        {
            if (labelSettings == null)
            {
                throw new ArgumentNullException("labelSettings");
            }

            this.labelSettings = labelSettings;
        }

        /// <summary>
        /// The name of the printer the job will be printed to
        /// </summary>
        public string PrinterName
        {
            get
            {
                return printerName;
            }
            set
            {
                printerName = value;

                _paperSourceName = null;
            }
        }

        /// <summary>
        /// The source tray the job will be printed to
        /// </summary>
        public int PaperSource
        {
            get
            {
                return paperSource;
            }
            set
            {
                paperSource = value;

                _paperSourceName = null;
            }
        }

        /// <summary>
        /// The name of the paper source used
        /// </summary>
        public string PaperSourceName
        {
            get
            {
                if (_paperSourceName == null)
                {
                    _paperSourceName = string.Empty;

                    IPrinterSetting settings = PrinterSettingFactory.GetPrinterSettings(printerName);
                    _paperSourceName = settings.PaperSourceName;
                }

                return _paperSourceName;
            }
        }

        /// <summary>
        /// Number of printed copies desired
        /// </summary>
        public int Copies
        {
            get { return copies; }
            set { copies = value; }
        }

        /// <summary>
        /// Indicates if multiple copies should be collated
        /// </summary>
        public bool Collate
        {
            get { return collate; }
            set { collate = value; }
        }

        /// <summary>
        /// Indicates if the print job represents a therma label
        /// </summary>
        public bool IsThermal
        {
            get { return isThermal; }
        }

        /// <summary>
        /// The page settings for the current print job.
        /// </summary>
        public PrintJobPageSettings PageSettings
        {
            get { return pageSettings; }
        }

        /// <summary>
        /// The label settings for the current print job.  Only applies if its a label template
        /// </summary>
        public PrintJobLabelSettings LabelSettings
        {
            get { return labelSettings; }
        }
    }
}
