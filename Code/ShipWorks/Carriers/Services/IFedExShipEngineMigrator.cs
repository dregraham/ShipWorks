using System.Windows.Forms;

namespace ShipWorks.Carriers.Services
{
    /// <summary>
    /// Service to migrate FedEx accounts to ShipEngine
    /// </summary>
    public interface IFedExShipEngineMigrator
    {
        /// <summary>
        /// Perform migration of any FedEx accounts that haven't already been migrated
        /// </summary>
        void Migrate(IWin32Window owner);
    }
}
