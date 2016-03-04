using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;

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
            IEnumerable<IFeatureRestriction> featureRestrictions)
        {
            Key = key;
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
            log = logFactory(typeof (CustomerLicense));
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
        /// Is the license legacy
        /// </summary>
        public bool IsLegacy => false;

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        public bool IsDisabled => (!string.IsNullOrEmpty(DisabledReason));

        /// <summary>
        /// Gets or sets the user name of the SDC account associated with this license.
        /// </summary>
        public string AssociatedStampsUsername { get; set; }

        /// <summary>
        /// The license capabilities.
        /// </summary>
        private ILicenseCapabilities LicenseCapabilities { get; set; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            IAddStoreResponse response = tangoWebClient.AddStore(this, store);

            store.License = response.Key;

            // The license activation state will be one of three values: Active, Invalid,
            // or OverChannelLimit. Rely on the success flag initially then dig into
            // the response to determine if the license is over the channel limit.
            LicenseActivationState activationState = response.Success
                ? LicenseActivationState.Active
                : LicenseActivationState.Invalid;

            if ((response.Error?.IndexOf("CreateStoreActivity_103", StringComparison.Ordinal) ?? -1) >= 0)
            {
                // The response from Tango indicates the license will be over the channel limit
                activationState = LicenseActivationState.OverChannelLimit;
            }

            return new EnumResult<LicenseActivationState>(activationState, response.Error);
        }

        /// <summary>
        /// Uses the ILicenseWriter provided in the constructor to save this instance
        /// to a data source.
        /// </summary>
        public void Save()
        {
            licenseWriter.Write(this);
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
            }
            catch (TangoException ex)
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
            if (DateTime.UtcNow.Subtract(lastRefreshTimeInUtc) > capabilitiesTimeToLive)
            {
                // The license capabilities have expired
                ForceRefresh();
            }
        }

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActiveStore> GetActiveStores()
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
        public void DeleteStore(StoreEntity store)
        {
            if (store == null)
            {
                return;
            }

            // Save the key to use later
            string licenseKey = store.License;

            // Remove the store from the database
            log.Warn($"Deleting store: {store.StoreName}");
            deletionService.DeleteStore(store);

            // Tell tango to delete the licenseKey
            tangoWebClient.DeleteStore(this, licenseKey);
        }

        /// <summary>
        /// Delete the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode storeType)
        {
            if (storeType == StoreTypeCode.Invalid)
            {
                return;
            }

            // Delete all of the local stores for the given StoreTypeCode
            log.Warn($"Deleting channel: {EnumHelper.GetDescription(storeType)}");
            deletionService.DeleteChannel(storeType);

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
            if (LicenseCapabilities == null)
            {
                Refresh();
            }

            EnumResult<ComplianceLevel> enforcerNotInCompliance =
                licenseEnforcers
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
            if (LicenseCapabilities == null)
            {
                Refresh();
            }

            licenseEnforcers.ToList().ForEach(e => e.Enforce(LicenseCapabilities, context, owner));

            Refresh();
        }

        /// <summary>
        /// Enforces capabilities based on the given feature
        /// </summary>
        public IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context)
        {
            if (LicenseCapabilities == null)
            {
                Refresh();
            }

            List<EnumResult<ComplianceLevel>> result = new List<EnumResult<ComplianceLevel>>();

            licenseEnforcers.Where(e => e.EditionFeature == feature)
                .ToList()
                .ForEach(e => result.Add(e.Enforce(LicenseCapabilities, context)));

            return result;
        }

        /// <summary>
        /// There are no dashboard items for Storelicense so it returns null
        /// </summary>
        public DashboardLicenseItem CreateDashboardMessage()
        {
            if (LicenseCapabilities == null)
            {
                Refresh();
            }
            
            if (LicenseCapabilities.IsInTrial)
            {
                return null;
            }

            float currentShipmentPercentage = (float) LicenseCapabilities.ProcessedShipments / LicenseCapabilities.ShipmentLimit;

            return currentShipmentPercentage >= ShipmentLimitWarningThreshold ?
                new DashboardLicenseItem(LicenseCapabilities.BillingEndDate) :
                null;
        }

        /// <summary>
        /// Associates the given UspsAccount with this license in tango
        /// </summary>
        public void AssociateStampsUsername(UspsAccountEntity uspsAccount)
        {
            try
            {
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
        /// Checks the restriction for a specific feature
        /// </summary>
        public EditionRestrictionLevel CheckRestriction(EditionFeature feature, object data)
        {
            if (LicenseCapabilities == null)
            {
                Refresh();
            }

            IFeatureRestriction restriction = featureRestrictions.SingleOrDefault(r => r.EditionFeature == feature);
            return restriction?.Check(LicenseCapabilities, data) ?? EditionRestrictionLevel.None;
        }

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        public bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner)
        {
            if (LicenseCapabilities == null)
            {
                Refresh();
            }

            return featureRestrictions
                .Where(r => r.EditionFeature == feature)
                .Select(r => (bool?) r.Handle(owner, LicenseCapabilities, data))
                .SingleOrDefault() ?? true;
        }
    }
}