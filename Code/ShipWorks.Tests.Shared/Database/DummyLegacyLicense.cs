using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Dummy license that can be used to write the legacy key to the database
    /// </summary>
    internal class DummyLegacyLicense : ICustomerLicense
    {
        public string AssociatedStampsUsername
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string DisabledReason
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsDisabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsLegacy
        {
            get { throw new NotImplementedException(); }
        }

        public string Key
        {
            get
            {
                return string.Empty;
            }
        }

        public string StampsUsername
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            throw new NotImplementedException();
        }

        public void AssociateUspsAccount(UspsAccountEntity uspsAccount)
        {
            throw new NotImplementedException();
        }

        public EditionRestrictionLevel CheckRestriction(EditionFeature feature, object data)
        {
            throw new NotImplementedException();
        }

        public DashboardLicenseItem CreateDashboardMessage()
        {
            throw new NotImplementedException();
        }

        public void DeleteChannel(StoreTypeCode storeType, ISecurityContext securityContext)
        {
            throw new NotImplementedException();
        }

        public void DeleteStore(StoreEntity store, ISecurityContext securityContext)
        {
            throw new NotImplementedException();
        }

        public void EnforceCapabilities(EnforcementContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context)
        {
            throw new NotImplementedException();
        }

        public void EnforceCapabilities(EnforcementContext context, IWin32Window owner)
        {
            throw new NotImplementedException();
        }

        public void ForceRefresh()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IActiveStore> GetActiveStores()
        {
            throw new NotImplementedException();
        }

        public bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}