using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Defines an error discovered while validating a task
    /// </summary>
    public class TaskValidationError
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskValidationError()
        {
            Details = new List<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Friendly message for the error</param>
        public TaskValidationError(string message) : this()
        {
            Message = message;
        }

        /// <summary>
        /// Friendly description of the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// List of details about the error
        /// </summary>
        public ICollection<string> Details { get; private set; }

        /// <summary>
        /// Provides a string representation of the error
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Details.Any())
            {
                return Message + Environment.NewLine + Details.Select(x => "  - " + x).Combine(Environment.NewLine);  
            }

            return Message;
        }
    }
}
