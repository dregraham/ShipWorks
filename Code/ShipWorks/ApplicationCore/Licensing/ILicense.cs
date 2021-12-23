﻿using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Users.Security;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// License describes the capabilities of the customer's license.
    /// </summary>
    public interface ILicense
    {
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
        /// Details about the trial
        /// </summary>
        TrialDetails TrialDetails { get; }

        /// <summary>
        /// Whether or not this license is UPS CTP
        /// </summary>
        bool IsCtp { get; }

        /// <summary>
        /// Refresh the license capabilities from Tango
        /// </summary>
        void Refresh();

        /// <summary>
        /// Bypasses any caching and forces a refresh of the license capabilities.
        /// </summary>
        void ForceRefresh();

        /// <summary>
        /// Activate a new store
        /// </summary>
        EnumResult<LicenseActivationState> Activate(StoreEntity store);

        /// <summary>
        /// Deletes a store
        /// </summary>
        void DeleteStore(StoreEntity store, ISecurityContext securityContext);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Throws ShipWorksLicenseException when out of compliance
        /// </summary>
        void EnforceCapabilities(EnforcementContext context);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Prompts user to take action when out of compliance
        /// </summary>
        void EnforceCapabilities(EnforcementContext context, IWin32Window owner);

        /// <summary>
        /// Enforces the capabilities for this license,
        /// related to the given EditionFeature.
        /// </summary>
        IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context);

        /// <summary>
        /// Returns a dashboard item to display to the user
        /// </summary>
        /// <returns></returns>
        DashboardLicenseItem CreateDashboardMessage();

        /// <summary>
        /// Associates a free Stamps.com account with a customer license.
        /// </summary>
        /// <param name="uspsAccount">The usps account.</param>
        void AssociateUspsAccount(UspsAccountEntity uspsAccount);

        /// <summary>
        /// Apply shipping policies for the ShipmentTypeCode to the target
        /// </summary>
        void ApplyShippingPolicy(ShipmentTypeCode shipmentTypeCode, object target);
    }
}
