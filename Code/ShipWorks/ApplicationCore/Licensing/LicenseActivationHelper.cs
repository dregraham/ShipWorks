using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Utility class for working with licenses
    /// </summary>
    public static class LicenseActivationHelper
    {
        /// <summary>
        /// Attempt to activate the given license key.  Any problems or failures with activation will be displayed
        /// as a message with owner as the window parent.  If the license is successfully activated, then it is set to the in-memory StoreEntity.
        /// </summary>
        public static LicenseActivationState ActivateAndSetLicense(StoreEntity store, string licenseKey, IWin32Window owner)
        {
            EnumResult<LicenseActivationState> activateLicenseResult = ActivateAndSetLicense(store, licenseKey);
            if (!string.IsNullOrEmpty(activateLicenseResult.Message))
            {
                MessageHelper.ShowError(owner, activateLicenseResult.Message);
            }

            return activateLicenseResult.Value;
        }

        /// <summary>
        /// Attempt to activate the given license key.  Any problems or failures with activation will be returned
        /// in the message.  If the license is successfully activated, then it is set to the in-memory StoreEntity.
        /// </summary>
        public static EnumResult<LicenseActivationState> ActivateAndSetLicense(StoreEntity store, string licenseKey)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            EnumResult<LicenseActivationState> licenseValidity = CheckLicenseValidity(store, licenseKey);

            // Before connecting to the web client, make sure its valid
            if (licenseValidity.Value == LicenseActivationState.Invalid)
            {
                return licenseValidity;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                var accountDetail = TangoWebClient.GetLicenseStatus(licenseKey, store, false);

                if (accountDetail.ActivationState == LicenseActivationState.Active)
                {
                    store.License = licenseKey;
                    store.Edition = accountDetail.Edition.Serialize();
                }

                return new EnumResult<LicenseActivationState>(
                    accountDetail.ActivationState,
                    GetActivationStateMessage(accountDetail));
            }
            catch (ShipWorksLicenseException ex)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "There is a problem with the license that was entered.\n\n" +
                    $"Details: {ex.Message}");
            }
            catch (TangoException ex)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "The license entered is valid, but ShipWorks was unable to connect\n" +
                    "to the license server to determine the status of the license.\n\n" +
                    $"Details: {ex.Message}");
            }
        }

        /// <summary>
        /// Ensure the license for the given store is active and allowed to download and process shipments.  It if is
        /// not, a LicenseException is thrown.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static ILicenseAccountDetail EnsureActive(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (!store.Enabled)
            {
                throw new ShipWorksLicenseException("The store has been disabled for downloading and shipping in your ShipWorks store settings.");
            }

            ShipWorksLicense license = new ShipWorksLicense(store.License);

            if (!license.IsValid)
            {
                throw new ShipWorksLicenseException("The license is not a valid ShipWorks license.");
            }

            if (store.StoreTypeCode != license.StoreTypeCode)
            {
                throw new ShipWorksLicenseException(
                    string.Format(
                    "The license entered is a valid ShipWorks license, but it can only " +
                    "be used for {0} stores.", StoreTypeManager.GetType(license.StoreTypeCode).StoreTypeName));
            }

            if (!license.IsMetered)
            {
                throw new ShipWorksLicenseException(
                    "Your ShipWorks license is not valid for this version of ShipWorks.\n\n" +
                    "A license that is billed monthly is required to use the current version of " +
                    "ShipWorks.  Please visit http://www.interapptive.com to sign up " +
                    "for a new license.");
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var accountDetail = lifetimeScope.Resolve<ITangoWebClient>().GetLicenseStatus(store.License, store);

                if (accountDetail.InTrial)
                {
                    // Trial expired
                    if (accountDetail.TrialIsExpired)
                    {
                        throw new ShipWorksLicenseException(
                            "Your ShipWorks trial period has expired.\n\n" +
                            "Please enter a credit card at https://hub.shipworks.com/account to continue using ShipWorks.");
                    }

                    EditionManager.UpdateStoreEdition(store, accountDetail.Edition);
                }
                else
                {
                    if (accountDetail.ActivationState == LicenseActivationState.Active ||
                        accountDetail.ActivationState == LicenseActivationState.ActiveElsewhere ||
                        accountDetail.ActivationState == LicenseActivationState.ActiveNowhere)
                    {
                        EditionManager.UpdateStoreEdition(store, accountDetail.Edition);
                    }

                    if (accountDetail.ActivationState != LicenseActivationState.Active)
                    {
                        switch (accountDetail.ActivationState)
                        {
                            case LicenseActivationState.ActiveNowhere:
                                throw new ShipWorksLicenseException(
                                    "Your ShipWorks license is not activated. The license can be activated in the Store Manager.");

                            case LicenseActivationState.ActiveElsewhere:
                                throw new ShipWorksLicenseException(
                                    "Your ShipWorks license is activated to another store. Please contact ShipWorks support to reset your license activation.");

                            case LicenseActivationState.Deactivated:
                                throw new ShipWorksLicenseException(
                                    "Your ShipWorks license has been disabled.\n\n" +
                                    "Reason: " + accountDetail.DisabledReason);

                            case LicenseActivationState.Canceled:
                                throw new ShipWorksLicenseException(
                                    "Your ShipWorks account for this license has been canceled.\n\n" +
                                    "Please log on to your account at https://www.interapptive.com/account " +
                                    "to activate your license.");

                            case LicenseActivationState.Invalid:
                                throw new ShipWorksLicenseException(
                                    "The ShipWorks license is not valid.");
                        }
                    }
                }


                // Check store count limitation
                EditionRestrictionIssue issue =
                    EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.SingleStore);
                if (issue.Level != EditionRestrictionLevel.None)
                {
                    if (StoreManager.GetAllStores().Count > 1)
                    {
                        throw new ShipWorksLicenseException(
                            "Your ShipWorks license only supports a single store. You must delete extra stores, or contact us at 1-800-95-APPTIVE to upgrade your ShipWorks edition.");
                    }
                }

                return accountDetail;
            }
        }

        /// <summary>
        /// Check the account detail for validity and return message for the user.
        /// </summary>
        public static string GetActivationStateMessage(ILicenseAccountDetail accountDetail)
        {
            if (accountDetail == null)
            {
                throw new ArgumentNullException("accountDetail");
            }

            switch (accountDetail.ActivationState)
            {
                // Everything is ok
                case LicenseActivationState.Active:
                {
                    return string.Empty;
                }
                // This shouldn't happen - if it was active nowhere,
                // then it should have been activated to us
                case LicenseActivationState.ActiveNowhere:
                {
                    return "An unknown problem occurred activating the license.\n\n" +
                           "Please contact ShipWorks support for assistance.";
                }
                case LicenseActivationState.ActiveElsewhere:
                {
                    return "The ShipWorks license you entered is already being used by another store. \n\n" +
                           "You can reset the license and make it available for use by logging in to your \n" +
                           "ShipWorks account using the following link.\n\n" +
                           "https://www.interapptive.com/account";
                }
                case LicenseActivationState.Deactivated:
                {
                    return "Your ShipWorks license has been disabled.\n\n" +
                           $"Reason: {accountDetail.DisabledReason}";

                }
                case LicenseActivationState.Canceled:
                {
                    return "The ShipWorks license you entered has been canceled. \n\n" +
                           "You can activate the license and by logging in to your\n" +
                           " ShipWorks account using the following link. \n\n" +
                           "https://www.interapptive.com/account";
                }
                case LicenseActivationState.Invalid:
                {
                    return "The license entered is a valid ShipWorks license, but was\n" +
                           "not found.\n\n" +
                           "Please contact ShipWorks support for assistance.";
                }
                default:
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Check that the given license and store combination represents a potentially valid ShipWorks license.
        /// </summary>
        private static EnumResult<LicenseActivationState> CheckLicenseValidity(StoreEntity store, string licenseKey)
        {
            // Check for empty
            if (licenseKey.Length == 0)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "Please enter a license key.");
            }

            // Parse the license
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);

            // Check for validity
            if (!license.IsValid)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "The license is not valid.\n\n" +
                    "Please check that the license was entered correctly.");
            }

            if ((StoreTypeCode) store.TypeCode != license.StoreTypeCode)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "The license entered is a valid ShipWorks license, but it can only\n" +
                    $"be used for {StoreTypeManager.GetType(license.StoreTypeCode).StoreTypeName} stores.");
            }

            // See if its a metered license
            if (!license.IsMetered)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid,
                    "Your ShipWorks license is not valid for this version of ShipWorks. \n\n" +
                    "A license that is billed monthly is required to use the current version " +
                    "of ShipWorks.  Use the link below to go to the ShipWorks website and " +
                    "sign up for a new license.\n\n" +
                    "https://www.interapptive.com/store");
            }

            return new EnumResult<LicenseActivationState>(LicenseActivationState.Active);
        }
    }
}
