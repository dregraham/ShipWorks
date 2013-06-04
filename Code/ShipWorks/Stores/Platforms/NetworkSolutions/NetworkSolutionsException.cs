using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class NetworkSolutionsException : Exception
    {
        /// <summary>
        /// Type of failure
        /// </summary>
        NetworkSolutionsFailureSeverity severity = NetworkSolutionsFailureSeverity.Failure;

        /// <summary>
        /// Collection of errors returned by the NetworkSolutions web service
        /// </summary>
        List<NetworkSolutionsError> errors = new List<NetworkSolutionsError>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public NetworkSolutionsException(string message)
            : base(message)
        {
        }

        public NetworkSolutionsException(NetworkSolutionsFailureSeverity severity, List<NetworkSolutionsError> errors)
            : base(ConstructErrorMessage(errors))
        {
            this.errors.AddRange(errors);
            this.severity = severity;
        }

        /// <summary>
        /// Creates an error message from a list of errors
        /// </summary>
        private static string ConstructErrorMessage(List<NetworkSolutionsError> errors)
        {
            string message = "";

            // construct an error string
            foreach (NetworkSolutionsError error in errors)
            {
                if (message.Length > 0)
                {
                    message += "\r\n";
                }

                message += String.Format("(Error #{0}) {1}", error.Number, error.Message);
            }

            return message;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected NetworkSolutionsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            severity = (NetworkSolutionsFailureSeverity)info.GetValue("severity", typeof(int));
            errors = (List<NetworkSolutionsError>)info.GetValue("errors", typeof(List<NetworkSolutionsError>));
        }

        /// <summary>
        /// Deserilization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("severity", (int)severity);
            info.AddValue("errors", errors);
        }
    }
}
