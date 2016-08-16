using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Wizard for configuring a store
    /// </summary>
    public interface IStoreWizard
    {
        /// <summary>
        /// The store currently being configured by the wizard
        /// </summary>
        StoreEntity Store { get; }
    }
}