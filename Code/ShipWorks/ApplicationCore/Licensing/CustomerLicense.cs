﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class to store customer license information
    /// </summary>
    public class CustomerLicense : ICustomerLicense
    {
        public const string UpgradeUrl = "https://www.interapptive.com/account/changeplan.php";
        private const float ShipmentLimitWarningThreshold = 0.8f;

        private readonly ITangoWebClient tangoWebClient;
        private readonly ICustomerLicenseWriter licenseWriter;
        private readonly ILog log;
        private readonly IDeletionService deletionService;
        private readonly IEnumerable<IFeatureRestriction> featureRestrictions;
        private readonly IEnumerable<ILicenseEnforcer> licenseEnforcers;
        private readonly IMessenger messenger;

        private readonly TimeSpan capabilitiesTimeToLive;
        private DateTime lastRefreshTimeInUtc;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public CustomerLicense(
            string key,
            ITangoWebClient tangoWebClient,
            ICustomerLicenseWriter licenseWriter,
            Func<Type, ILog> logFactory,
            IDeletionService deletionService,
            IEnumerable<ILicenseEnforcer> licenseEnforcers,
            IEnumerable<IFeatureRestriction> featureRestrictions,
            IMessenger messenger)
        {
            this.messenger = messenger;
            Key = key;
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
            log = logFactory(typeof(CustomerLicense));
            this.deletionService = deletionService;
            this.featureRestrictions = featureRestrictions;
            this.licenseEnforcers = licenseEnforcers.OrderByDescending(e => (int) e.Priority);

            // The license info/capabilities should be cached for 10 minutes
            capabilitiesTimeToLive = new TimeSpan(0, 10, 0);
            lastRefreshTimeInUtc = DateTime.MinValue;

            EnsureOnlyOneFeatureRestrictionPerEditionFeature();
        }

        /// <summary>
        /// Ensures there is only one restriction per edition-feature.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when there is more than one restriction per edition-feature.</exception>
        private void EnsureOnlyOneFeatureRestrictionPerEditionFeature()
        {
            IEnumerable<EditionFeature> editionFeatures = featureRestrictions
                .GroupBy(f => f.EditionFeature)
                .Where(grouping => grouping.Count() > 1)
                .Select(grouping => grouping.Key)
                .ToList();

            if (editionFeatures.Any())
            {
                string featureNames = string.Join(Environment.NewLine, editionFeatures.Select(f => f.ToString()));
                throw new InvalidOperationException(
                    "The following EditionFeatures have more than one associated Feature Restriction:" +
                    $"{Environment.NewLine}{featureNames}");
            }
        }

        /// <summary>
        /// The license key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        public bool IsDisabled => (!string.IsNullOrEmpty(DisabledReason));

        /// <summary>
        /// Details about the trial
        /// </summary>
        public TrialDetails TrialDetails => LicenseCapabilities?.TrialDetails ?? new TrialDetails();

        /// <summary>
        /// Gets or sets the user name of the SDC account associated with this license.
        /// </summary>
        public string AssociatedStampsUsername { get; set; }

        /// <summary>
        /// Gets or sets the stamps username to create when creating a new stamps account.
        /// </summary>
        public string StampsUsername { get; set; }

        /// <summary>
        /// Whether or not this license is UPS CTP
        /// </summary>
        public bool IsCtp => LicenseCapabilities != null && LicenseCapabilities?.UpsStatus != UpsStatus.None && LicenseCapabilities?.UpsStatus != UpsStatus.Discount;

        /// <summary>
        /// Gets the CustomerID.
        /// </summary>
        public string CustomerID {
            get
            {
                Refresh();

                return LicenseCapabilities.CustomerID;
            }
        }

        /// <summary>
        /// The license capabilities.
        /// </summary>
        private ILicenseCapabilities LicenseCapabilities { get; set; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            IAddStoreResponse response = tangoWebClient.AddStore(Key, store);
            store.License = response.Key;

            return new EnumResult<LicenseActivationState>(response.Result, EnumHelper.GetDescription(response.Result));
        }

        /// <summary>
        /// Bypasses any caching and forces a refresh of the license capabilities.
        /// </summary>
        public void ForceRefresh()
        {
            try
            {
                // Refresh the license capabilities and note the time they were refreshed
                LicenseCapabilities = tangoWebClient.GetLicenseCapabilities(this);
                lastRefreshTimeInUtc = DateTime.UtcNow;

                // Reset disabled reason now that we can reconnect
                DisabledReason = string.Empty;

                // Let anyone who cares know that enabled carriers may have changed.
                messenger.Send(new EnabledCarriersChangedMessage(this, new List<ShipmentTypeCode>(), new List<ShipmentTypeCode>()));
            }
            catch (Exception ex) when (ex.GetType() == typeof(TangoException) || ex.GetType() == typeof(ShipWorksLicenseException))
            {
                LicenseCapabilities = null; // may want to use a null object pattern here...
                DisabledReason = ex.Message;
                log.Warn(ex);
            }
        }

        /// <summary>
        /// Refresh the license capabilities from Tango
        /// </summary>
        public void Refresh()
        {
            // If the license capabilities is null ,possibly due to Tango returning an empty node,
            // or we've passed our time limit, force a refresh
            if (LicenseCapabilities == null || DateTime.UtcNow.Subtract(lastRefreshTimeInUtc) > capabilitiesTimeToLive)
            {
                // The license capabilities have expired
                ForceRefresh();
            }
        }

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IActiveStore> GetActiveStores()
        {
            try
            {
                return tangoWebClient.GetActiveStores(this);
            }
            catch (TangoException ex)
            {
                throw new ShipWorksLicenseException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes the given store
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store, ISecurityContext securityContext)
        {
            if (store == null)
            {
                return;
            }

            // Save the key to use later
            string licenseKey = store.License;

            // Remove the store from the database
            log.Warn($"Deleting store: {store.StoreName}");
            deletionService.DeleteStore(store, securityContext);

            // Tell tango to delete the licenseKey
            tangoWebClient.DeleteStore(this, licenseKey);
        }

        /// <summary>
        /// Delete the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode storeType, ISecurityContext securityContext)
        {
            if (storeType == StoreTypeCode.Invalid)
            {
                return;
            }

            // Delete all of the local stores for the given StoreTypeCode
            log.Warn($"Deleting channel: {EnumHelper.GetDescription(storeType)}");
            deletionService.DeleteChannel(storeType, securityContext);

            // Get a list of licenses that are active in tango and match the channel we are deleting
            // but are not in ShipWorks and tell tango to delete them
            IEnumerable<string> licensesToDelete = GetActiveStores()
                                                    .Where(a => a.StoreType == storeType)
                                                    .Select(a => a.StoreLicenseKey);

            tangoWebClient.DeleteStores(this, licensesToDelete);
        }

        /// <summary>
        /// Enforces license capabilities and throws ShipWorksLicenseException
        /// if any enforcer returns not compliant
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context)
        {
            Refresh();

            // Enforce restrictions when not in the trial period
            EnumResult<ComplianceLevel> enforcerNotInCompliance =
                licenseEnforcers
                    .Where(enforcer => enforcer.AppliesTo(LicenseCapabilities))
                    .Select(enforcer => enforcer.Enforce(LicenseCapabilities, context))
                    .FirstOrDefault(result => result.Value == ComplianceLevel.NotCompliant);

            if (enforcerNotInCompliance != null)
            {
                throw new ShipWorksLicenseException(enforcerNotInCompliance.Message);
            }
        }

        /// <summary>
        /// Enforces capabilities and shows dialog on the given owner
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context, IWin32Window owner)
        {
            if (!Program.ExecutionMode.IsUISupported)
            {
                EnforceCapabilities(context);
                return;
            }

            Refresh();

            // Enforce restrictions when not in the trial period
            licenseEnforcers
                .Where(enforcer => enforcer.AppliesTo(LicenseCapabilities)).ToList()
                .ForEach(e => e.Enforce(LicenseCapabilities, context, owner));
        }

        /// <summary>
        /// Enforces capabilities based on the given feature
        /// </summary>
        public IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature,
            EnforcementContext context)
        {
            List<EnumResult<ComplianceLevel>> result = new List<EnumResult<ComplianceLevel>>();

            Refresh();
            // Enforce restrictions when not in the trial period
            licenseEnforcers.Where(enforcer => enforcer.AppliesTo(LicenseCapabilities) && enforcer.EditionFeature == feature)
                .ToList()
                .ForEach(e => result.Add(e.Enforce(LicenseCapabilities, context)));

            return result;
        }

        /// <summary>
        /// Returns a dashboard item to notify the user about an aspect/status of his/her license. A null
        /// value is returned when there is nothing to display.
        /// </summary>
        /// <remarks>
        /// A dashboard item is returned when the customer has exceeded 80% of the
        /// shipping volume allowed by the license during the current billing cycle.
        /// </remarks>
        public DashboardLicenseItem CreateDashboardMessage()
        {
            DashboardLicenseItem dashboardItem = null;

            Refresh();

            if (LicenseCapabilities != null && !LicenseCapabilities.TrialDetails.IsInTrial)
            {
                // The dashboard item should not be created when in the trial period since
                // the shipment count is unlimited.
                float currentShipmentPercentage = (float) LicenseCapabilities.ProcessedShipments / LicenseCapabilities.ShipmentLimit;
                if (currentShipmentPercentage >= ShipmentLimitWarningThreshold)
                {
                    // This shipping volume is within the warning threshold. Create the dashboard item.
                    dashboardItem = new DashboardLicenseItem(LicenseCapabilities.BillingEndDate, currentShipmentPercentage);
                }
            }

            return dashboardItem;
        }

        /// <summary>
        /// Associates the given UspsAccount with this license in tango
        /// </summary>
        public void AssociateUspsAccount(UspsAccountEntity uspsAccount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uspsAccount?.Username) || string.IsNullOrWhiteSpace(uspsAccount.Password))
                {
                    throw new ShipWorksLicenseException("Cannot associate empty Usps account.");
                }

                tangoWebClient.AssociateStampsUsernameWithLicense(Key, uspsAccount.Username,
                    SecureText.Decrypt(uspsAccount.Password, uspsAccount.Username));
            }
            catch (Exception ex)
            {
                // gobble up any exception and log, this is to ensure that if
                // the association fails ShipWorks continues to function.
                log.Error("Error when associating stamps account with license.", ex);
            }
        }

        /// <summary>
        /// Apply Shipping Policy for the ShipmentTypeCode to the target
        /// </summary>
        /// <remarks>Currently this only applies to Best Rate</remarks>
        public void ApplyShippingPolicy(ShipmentTypeCode shipmentTypeCode, object target)
        {
            if (shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                BestRateUpsRestrictionShippingPolicy bestRateUpsRestriction = new BestRateUpsRestrictionShippingPolicy();

                string bestRateShippingPolicy = LicenseCapabilities.GetShipmentTypeFunctionality(ShipmentTypeCode.BestRate, ShippingPolicyType.BestRateUpsRestriction);

                bestRateUpsRestriction.Configure(bestRateShippingPolicy != string.Empty
                    ? bestRateShippingPolicy
                    : "True");

                if (bestRateUpsRestriction.IsApplicable(target))
                {
                    bestRateUpsRestriction.Apply(target);
                }

                RateResultCountShippingPolicy rateResultCount = new RateResultCountShippingPolicy();
                rateResultCount.Configure("5");
                if (rateResultCount.IsApplicable(target))
                {
                    rateResultCount.Apply(target);
                }
            }
            else if (shipmentTypeCode == ShipmentTypeCode.AmazonSFP ||
                shipmentTypeCode == ShipmentTypeCode.AmazonSWA)
            {
                ShippingPolicies.Current.Apply(shipmentTypeCode, target);
            }
        }

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        public EnumResult<EditionRestrictionLevel> CheckRestriction(EditionFeature feature, object data)
        {
            Refresh();

            if (IsDisabled)
            {
                return EditionRestrictionLevel.Forbidden.AsEnumResult();
            }
            if(feature == EditionFeature.ShipmentType && (ShipmentTypeCode)data == ShipmentTypeCode.AmazonSWA)
            {
                return EditionRestrictionLevel.None.AsEnumResult();
            }
            IFeatureRestriction restriction = featureRestrictions.SingleOrDefault(r => r.EditionFeature == feature);
            return restriction?.CheckWithReason(LicenseCapabilities, data) ?? EditionRestrictionLevel.None.AsEnumResult();
        }

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        public bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner)
        {
            Refresh();

            return featureRestrictions
                .Where(r => r.EditionFeature == feature)
                .Select(r => (bool?) r.Handle(owner, LicenseCapabilities, data))
                .SingleOrDefault() ?? true;
        }
    }
}