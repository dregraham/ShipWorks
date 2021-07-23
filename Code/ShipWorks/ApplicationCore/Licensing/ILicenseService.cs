﻿using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for license service
    /// </summary>
    public interface ILicenseService
    {
        string CustomerKey { get; }
        
        string LegacyCustomerKey { get; }
        
        
        /// <summary>
        /// True if Legacy Customer
        /// </summary>
        /// <remarks>True if CustomerKey is null or empty</remarks>
        /// <exception cref="EncryptionException" />
        bool IsLegacy { get; }

        /// <summary>
        /// True if Hub customer
        /// </summary>
        bool IsHub { get; }

        /// <summary>
        /// Gets all licenses for ShipWorks
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILicense> GetLicenses();

        /// <summary>
        /// Can the customer Logon?
        /// </summary>
        EnumResult<LogOnRestrictionLevel> AllowsLogOn();

        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        ILicense GetLicense(StoreEntity store);

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        EditionRestrictionLevel CheckRestriction(EditionFeature feature, object data);

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        EnumResult<EditionRestrictionLevel> CheckRestrictionWithReason(EditionFeature feature, object data);

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner);
    }
}