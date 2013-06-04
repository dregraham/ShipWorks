using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Thrown when the configuration of a printer could not be changed due to access denied.
    /// </summary>
    public class PrinterConfigurationSecurityException : PrintingException
	{
        private static string message =
            "There was an error configuring the printer for printing.\n\n" +
            "See the help topic 'Appendix\\Configuring a network printer for ShipWorks' for assistance.";

        // Name of the printer
        string printer;

        /// <summary>
        /// Constructor
        /// </summary>
		public PrinterConfigurationSecurityException(string printer) : base(message)
		{
            this.printer = printer;
        }

        /// <summary>
        /// The printer that we had the problem with
        /// </summary>
        public string Printer
        {
            get { return printer; }
        }
    }
}
