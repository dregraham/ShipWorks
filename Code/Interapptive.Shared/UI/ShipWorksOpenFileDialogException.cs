using System;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Exception raised by the ShipWorksOpenFileDialog
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ShipWorksOpenFileDialogException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOpenFileDialogException"/> class.
        /// </summary>
        public ShipWorksOpenFileDialogException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}