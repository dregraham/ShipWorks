﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// License describes the capabilities of the customer's license.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        void Refresh();

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        bool IsDisabled { get; }

        /// <summary>
        /// The license key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        EnumResult<LicenseActivationState> Activate(StoreEntity store);
    }
}
