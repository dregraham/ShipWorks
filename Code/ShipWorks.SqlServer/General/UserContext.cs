﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.General
{
    /// <summary>
    /// Contains contextual information about the ShipWorks user who a SQL query is running under
    /// </summary>
    public class UserContext
    {
        long userID;
        long computerID;
        bool auditingEnabled;
        bool deletingStore;

        int reasonType;
        string reasonDetail;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserContext(long userID, long computerID, int reasonType, string reasonDetail, bool auditingEnabled, bool deletingStore)
        {
            this.userID = userID;
            this.computerID = computerID;
            this.reasonType = reasonType;
            this.reasonDetail = reasonDetail;
            this.auditingEnabled = auditingEnabled;
            this.deletingStore = deletingStore;
        }

        /// <summary>
        /// The ID of the logged on user
        /// </summary>
        public long UserID
        {
            get { return userID; }
        }

        /// <summary>
        /// The computer ID that the user is working from
        /// </summary>
        public long ComputerID
        {
            get { return computerID; }
        }

        /// <summary>
        /// The audit reason type
        /// </summary>
        public int AuditReasonType
        {
            get { return reasonType; }
        }

        /// <summary>
        /// Supplemental detail to the audit reason type
        /// </summary>
        public string AuditReasonDetail
        {
            get
            {
                if (reasonDetail == null)
                {
                    return null;
                }

                int maxLength = 100;

                if (reasonDetail.Length <= maxLength)
                {
                    return reasonDetail;
                }
                else
                {
                    return reasonDetail.Substring(0, maxLength - 3) + "...";
                }
            }
        }

        /// <summary>
        /// Indicates if auditing is enabled
        /// </summary>
        public bool AuditingEnabled
        {
            get { return auditingEnabled; }
        }

        /// <summary>
        /// Indicates if a store is currently being deleted
        /// </summary>
        public bool DeletingStore
        {
            get { return deletingStore; }
        }
    }
}
