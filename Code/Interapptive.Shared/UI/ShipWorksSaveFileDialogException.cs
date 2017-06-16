using System;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Exception raised by the ShipWorksSaveFileDialog
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ShipWorksSaveFileDialogException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksSaveFileDialogException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ShipWorksSaveFileDialogException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}