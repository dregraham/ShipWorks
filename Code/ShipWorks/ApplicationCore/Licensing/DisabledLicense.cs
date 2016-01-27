using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// If an error occurs we can return a disabled license because we can't verify the license.
    /// </summary>
    public class DisabledLicense : ILicense
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledLicense(string disabledReason)
        {
            DisabledReason = disabledReason;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Refresh()
        {
            // Do nothing
        }

        /// <summary>
        /// Reason license is disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Returns True
        /// </summary>
        public bool IsDisabled => true;

        /// <summary>
        /// Throws - We shouldn't try to access the key if the license is disabled.
        /// </summary>
        public string Key
        {
            get
            {
                throw new ShipWorksLicenseException("Key not a valid property for a disabled license.");
            }
        }

        /// <summary>
        /// Not Legacy. 
        /// </summary>
        public bool IsLegacy => false;

        /// <summary>
        /// Not over limit, but I did debate throwing for this...
        /// </summary>
        public bool IsOverChannelLimit => false;

        /// <summary>
        /// Throws - We shouldn't try to activate a disabled license.
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            throw new System.NotImplementedException();
        }
    }
}