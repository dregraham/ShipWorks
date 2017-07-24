using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;

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
        public async Task AuditAsync(long entityID, AuditActionType action, AuditReason auditReason, ISqlAdapter sqlAdapter) =>
                    await AuditUtility.AuditAsync(entityID, action, auditReason, sqlAdapter, userSession);

        /// <summary>
        /// Show the detail for the given audit record
        /// </summary>
        public void ShowAuditDetail(IWin32Window owner, long auditID) => AuditUtility.ShowAuditDetail(owner, auditID);
    }
}
