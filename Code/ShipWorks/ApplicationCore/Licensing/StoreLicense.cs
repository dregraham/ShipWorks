using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class StoreLicense : ILicense
    {
        private readonly StoreEntity store;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreLicense(StoreEntity store, Func<Type, ILog> logFactory)
        {
            this.store = store;
            log = logFactory(GetType());
            Key = store.License;
        }

        /// <summary>
        /// The reason the store is disabled.
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the store disabled?
        /// </summary>
        public bool IsDisabled => !string.IsNullOrEmpty(DisabledReason);

        /// <summary>
        /// License key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Is the license legacy
        /// </summary>
        public bool IsLegacy => true;

        /// <summary>
        /// Store licenses do not have channel limits
        /// </summary>
        /// <remarks>
        /// Always returns false
        /// </remarks>
        public bool IsOverChannelLimit => false;

        /// <summary>
        /// Store licenses do not have shipment limits
        /// </summary>
        /// <remarks>
        /// Always returns false
        /// </remarks>
        public bool IsShipmentLimitReached => false;

        /// <summary>
        /// Store license do not have channel limits
        /// </summary>
        /// <remarks>
        /// Always return 0
        /// </remarks>
        public int NumberOfChannelsOverLimit => 0;

        /// <summary>
        /// Activate a new store
        /// </summary>
        /// <remarks>
        /// Make sure store is populated with the appropriate license key.
        /// </remarks>
        public EnumResult<LicenseActivationState> Activate(StoreEntity newStore)
        {
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            return license.IsTrial ?
                new EnumResult<LicenseActivationState>(LicenseActivationState.Active) :
                LicenseActivationHelper.ActivateAndSetLicense(newStore, newStore.License);
        }

        /// <summary>
        /// Nothing to enforce
        /// </summary>
        /// <param name="owner"></param>
        public void EnforceChannelLimit(IWin32Window owner)
        {
        }

        /// <summary>
        /// Nothing to enforce
        /// </summary>
        public void EnforceShipmentLimit(IWin32Window owner)
        {
        }

        /// <summary>
        /// Refreshes store license data.
        /// </summary>
        public void Refresh()
        {
            try
            {
                LicenseActivationHelper.EnsureActive(store);
                DisabledReason = string.Empty;
            }
            catch (Exception ex)
                when (ex is ShipWorksLicenseException || ex is TangoException)
            {
                log.Warn(ex.Message, ex);
                DisabledReason = ex.Message;
            }
        }

        /// <summary>
        /// Deletes the store
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            DeletionService.DeleteStore(store);
        }

        /// <summary>
        /// No enforcement for store licenses
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context)
        {
        }

        /// <summary>
        /// No enforcement for store licenses
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context, IWin32Window owner)
        {
        }

        /// <summary>
        /// No enforcement for store licenses
        /// </summary>
        public IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context)
        {
            return new List<EnumResult<ComplianceLevel>>
            {
                new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty)
            };
        }

        /// <summary>
        /// There are no dashboard items for Storelicense so it returns null
        /// </summary>
        public DashboardLicenseItem CreateDashboardMessage()
        {
            return null;
        }

        /// <summary>
        /// Does nothing for store licenses
        /// </summary>
        public void AssociateStampsUsername(UspsAccountEntity uspsAccount)
        {
        }
    }
}