using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Detailed information about a license retrieved from a customer's interapptive account
    /// </summary>
    public interface ILicenseAccountDetail
    {
        /// <summary>
        /// Get the current state of the license
        /// </summary>
        LicenseActivationState ActivationState { get; }

        /// <summary>
        /// If the license is active
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Readable description of the license status
        /// </summary>
        string Description { get; }

        /// <summary>
        /// If its deactivated (disabled), this is the reason why (for metered only)
        /// </summary>
        string DisabledReason { get; }

        /// <summary>
        /// The current edition of the license as it is in Tango
        /// </summary>
        IEdition Edition { get; }

        /// <summary>
        /// License key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The Tango CustomerID associated with this license
        /// </summary>
        string TangoCustomerID { get; }
    }
}