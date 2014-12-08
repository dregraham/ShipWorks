using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Exception for handling missing object labels for audits.
    /// </summary>
    public class AuditMissingObjectLabelException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuditMissingObjectLabelException(long auditID)
        {
            AuditID = auditID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditMissingObjectLabelException(string message, long auditID)
            : base(message)
        {
            AuditID = auditID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditMissingObjectLabelException(string message, Exception inner, long auditID)
            : base(message, inner)
        {
            AuditID = auditID;
        }

        /// <summary>
        /// The audit ID for which this exception occurred.
        /// </summary>
        public long AuditID
        {
            get; 
            set;
        }
    }
}
