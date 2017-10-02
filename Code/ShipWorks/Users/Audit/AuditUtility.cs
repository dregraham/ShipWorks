using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using System.Data;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Utility functions for security
    /// </summary>
    public static class AuditUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AuditUtility));

        /// <summary>
        /// The hard-coded "EntityID" that represents "Various" for audit logs that are for multiple objects.
        /// </summary>
        public static long VariousEntityID => -999;

        /// <summary>
        /// Audit the given event for the logged on user on the current computer
        /// </summary>
        public static void Audit(AuditActionType action)
        {
            Audit(action, UserSession.User.UserID);
        }

        /// <summary>
        /// Audit the given event for the given user on the current computer.
        /// </summary>
        public static void Audit(AuditActionType action, long userID)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                AuditEntity audit = new AuditEntity();

                audit.TransactionID = GetTransactionID(con);

                audit.UserID = userID;
                audit.Computer = UserSession.Computer;
                audit.Reason = (int) UserSession.AuditReason.ReasonType;
                audit.ReasonDetail = UserSession.AuditReason.ReasonDetail;
                audit.Date = DateTime.UtcNow;

                audit.Action = (int) action;
                audit.EntityID = null;

                audit.HasEvents = false;

                using (SqlAdapter adapter = new SqlAdapter(con))
                {
                    adapter.SaveEntity(audit);
                }
            }
        }

        /// <summary>
        /// Get the latest and greatest transaction ID value
        /// </summary>
        private static long GetTransactionID(DbConnection con)
        {
            return DbCommandProvider.ExecuteScalar<long>(con, "SELECT dbo.GetTransactionID()");
        }

        /// <summary>
        /// Show the detail for the given audit record
        /// </summary>
        public static void ShowAuditDetail(IWin32Window owner, long auditID)
        {
            AuditEntity audit = new AuditEntity(auditID);
            SqlAdapter.Default.FetchEntity(audit);

            // Its possible its been deleted, if it was an audit that had no actual changes and the audit processor cleaned it up
            if (audit.Fields.State != EntityState.Fetched)
            {
                MessageHelper.ShowInformation(owner, "The audit entry has no further detail to show.");
                return;
            }

            if (!audit.HasEvents)
            {
                MessageHelper.ShowInformation(owner, "The audit entry has no further detail to show.");
                return;
            }

            if (audit.Action == (int) AuditActionType.Undetermined)
            {
                MessageHelper.ShowError(owner, "ShipWorks is still preparing the audit entry.");
                return;
            }

            using (AuditDetailDlg dlg = new AuditDetailDlg(audit))
            {
                dlg.ShowDialog(owner);
            }
        }
    }
}
