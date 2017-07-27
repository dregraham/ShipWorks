using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Utility functions for security
    /// </summary>
    [Component]
    public class AuditUtilityWrapper : IAuditUtility
    {
        public readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSession"></param>
        public AuditUtilityWrapper(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        /// <summary>
        /// The hard-coded "EntityID" that represents "Various" for audit logs that are for multiple objects.
        /// </summary>
        public long VariousEntityID => AuditUtility.VariousEntityID;

        /// <summary>
        /// Audit the given event for the logged on user on the current computer
        /// </summary>
        public void Audit(AuditActionType action) => AuditUtility.Audit(action);

        /// <summary>
        /// Audit the given event for the given user on the current computer.
        /// </summary>
        public void Audit(AuditActionType action, long userID) => AuditUtility.Audit(action, userID);

        /// <summary>
        /// Audit the given event for the current user on the current computer.
        /// </summary>
        public async Task AuditAsync(long entityID, AuditActionType action, AuditReason auditReason, ISqlAdapter sqlAdapter)
        {
            AuditEntity audit = new AuditEntity
            {
                TransactionID = await GetTransactionID(sqlAdapter),
                UserID = userSession.User.UserID,
                Computer = userSession.Computer,
                Reason = (int) auditReason.ReasonType,
                ReasonDetail = auditReason.ReasonDetail,
                Date = DateTime.UtcNow,
                Action = (int) action,
                EntityID = entityID,
                HasEvents = false
            };

            await sqlAdapter.SaveEntityAsync(audit).ConfigureAwait(false);
        }

        /// <summary>
        /// Show the detail for the given audit record
        /// </summary>
        public void ShowAuditDetail(IWin32Window owner, long auditID) => AuditUtility.ShowAuditDetail(owner, auditID);

        /// <summary>
        /// Get the latest and greatest transaction ID value
        /// </summary>
        private static async Task<long> GetTransactionID(ISqlAdapter sqlAdapter)
        {
            ParameterValue transactionIdParam = new ParameterValue(ParameterDirection.InputOutput, dbType: DbType.Int64);
            await sqlAdapter.ExecuteSQLAsync(@"SELECT @Id=dbo.GetTransactionID();", new { Id = transactionIdParam })
                            .ConfigureAwait(false);
            
            return Convert.ToInt64(transactionIdParam.Value);
        }
    }
}
