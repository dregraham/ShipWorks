using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Exception that is thrown when a known error occurs during printing
    /// </summary>
    public class PrintingException : Exception
    {
        public PrintingException()
        {

        }

        public PrintingException(string message)
            : base(message)
        {

        }

        public PrintingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
