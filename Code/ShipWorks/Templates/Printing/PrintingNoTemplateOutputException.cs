using System;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Thrown when a print job can't do anything because there was no output returned from template processing
    /// </summary>
    public class PrintingNoTemplateOutputException : PrintingException
    {
        public PrintingNoTemplateOutputException()
        {

        }

        public PrintingNoTemplateOutputException(string message)
            : base(message)
        {

        }

        public PrintingNoTemplateOutputException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
