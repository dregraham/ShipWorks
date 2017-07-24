using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Connection;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Interface for utility functions for auditing
    /// </summary>
    public interface IAuditUtility
    {
        /// <summary>
        /// The hard-coded "EntityID" that represents "Various" for audit logs that are for multiple objects.
        /// </summary>
        long VariousEntityID { get; }

        /// <summary>
        /// Audit the given event for the logged on user on the current computer
        /// </summary>
        void Audit(AuditActionType action);

        /// <summary>
        /// Audit the given event for the given user on the current computer.
        /// </summary>
        void Audit(AuditActionType action, long userID);

        /// <summary>
        /// Audit the given event for the current user on the current computer.
        /// </summary>
        Task AuditAsync(long entityID, AuditActionType action, AuditReason auditReason, ISqlAdapter sqlAdapter);

        /// <summary>
        /// Show the detail for the given audit record
        /// </summary>
        void ShowAuditDetail(IWin32Window owner, long auditID);
    }
}
