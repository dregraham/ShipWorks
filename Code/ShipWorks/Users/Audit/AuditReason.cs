using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Audit reason information
    /// </summary>
    public class AuditReason
    {
        AuditReasonType reasonType;
        string reasonDetail;

        /// <summary>
        /// The default AuditReason
        /// </summary>
        public static AuditReason Default
        {
            get { return new AuditReason(AuditReasonType.Default); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditReason(AuditReasonType reasonType)
            : this(reasonType, null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditReason(AuditReasonType reasonType, string reasonDetail)
        {
            this.reasonType = reasonType;
            this.reasonDetail = reasonDetail;

            if (reasonDetail != null && reasonDetail.Length > 100)
            {
                reasonDetail = reasonDetail.Substring(0, 100);
            }

            // Can't set detail if its the default
            if (reasonType == AuditReasonType.Default && reasonDetail != null)
            {
                throw new InvalidOperationException("Cannot set reason detail for the default reason.");
            }
        }

        /// <summary>
        /// The type of reason
        /// </summary>
        public AuditReasonType ReasonType
        {
            get { return reasonType; }
        }

        /// <summary>
        /// Optional detail about the reason
        /// </summary>
        public string ReasonDetail
        {
            get { return reasonDetail; }
        }
    }
}
