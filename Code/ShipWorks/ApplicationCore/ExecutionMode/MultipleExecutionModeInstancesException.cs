﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    [Serializable]
    public class MultipleExecutionModeInstancesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleExecutionModeInstancesException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MultipleExecutionModeInstancesException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleExecutionModeInstancesException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MultipleExecutionModeInstancesException(string message, Exception innerException)
            : base (message, innerException)
        { }
    }
}
