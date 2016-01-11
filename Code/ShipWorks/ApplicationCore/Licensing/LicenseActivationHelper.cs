using System;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.ApplicationCore.Licensing.MessageBoxes;
using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Utility class for working with licenses
    /// </summary>
    public static class LicenseActivationHelper
    {
        /// <summary>
        /// Attempt to activate the given license key.  Any problems or failues with activation will be displayed
        /// as a message with owner as the window parent.  If the license is succesfully activated, then it is set to the in-memory StoreEntity.
        /// </summary>
        public static LicenseActivationState ActivateAndSetLicense(StoreEntity store, string licenseKey, IWin32Window owner)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Before connecting to the web client, make sure its valid
            if (!CheckLicenseValidity(store, licenseKey, owner))
            {
                return LicenseActivationState.Invalid;
            }

            Cursor.Current = Cursors.WaitCursor;
            LicenseAccountDetail accountDetail;

            try
            {
                accountDetail = TangoWebClient.ActivateLicense(licenseKey, store);

                CheckAccountValidity(accountDetail, owner);

                if (accountDetail.ActivationState == LicenseActivationState.Active)
                {
                    store.License = licenseKey;
                    store.Edition = EditionSerializer.Serialize(accountDetail.Edition);
                }

                return accountDetail.ActivationState;
            }
            catch (ShipWorksLicenseException ex)
            {
                MessageHelper.ShowError(owner,
                    "There is a problem with the license that was entered.\n\n" +
                    "Details: " + ex.Message);

                return LicenseActivationState.Invalid;
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(owner,
                    "The license entered is valid, but ShipWorks was unable to connect\n" +
                    "to the license server to determine the status of the license.\n\n" +
                    "Details: " + ex.Message);

                return LicenseActivationState.Invalid;
            }
        }

        /// <summary>
        /// Ensure the license for the given store is active and allowed to download and process shipments.  It if is
        /// not, a LicenseException is thrown.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void EnsureActive(StoreEntity store)
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

            if ((StoreTypeCode) store.TypeCode != license.StoreTypeCode)
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

            if (license.IsTrial)
            {
                TrialDetail trialDetail = TangoWebClient.GetTrial(store);

                // If its converted, they have to enter their license
                if (trialDetail.IsConverted)
                {
                    throw new ShipWorksLicenseException(
                        "A ShipWorks license has been purchased for this trial.  Please enter " +
                        "your license to continue using ShipWorks.");
                }

                // Trial expired
                if (trialDetail.IsExpired)
                {
                    throw new ShipWorksLicenseException(
                        "Your ShipWorks trial period has expired.\n\n" +
                        "Please sign up at http://www.interapptive.com to continue using ShipWorks.");
                }

                EditionManager.UpdateStoreEdition(store, trialDetail.Edition);
            }
            else
            {
                LicenseAccountDetail accountDetail = new TangoWebClientFactory().CreateWebClient().GetLicenseStatus(store.License, store);

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
                                "Your ShipWorks license is activated to another store. Please contact Interapptive to reset your license activation.");

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
            EditionRestrictionIssue issue = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.SingleStore);
            if (issue.Level != EditionRestrictionLevel.None)
            {
                if (StoreManager.GetAllStores().Count > 1)
                {
                    throw new ShipWorksLicenseException("Your ShipWorks license only supports a single store. You must delete extra stores, or contact us at 1-800-95-APPTIVE to upgrade your ShipWorks edition.");
                }
            }
        }

        /// <summary>
        /// Check the account detail for validity and display any problems to the user
        /// </summary>
        public static void CheckAccountValidity(LicenseAccountDetail accountDetail, IWin32Window owner)
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
                        break;
                    }

                // This shouldnt happen - if it was active nowhere,
                // then it should have been activated to us
                case LicenseActivationState.ActiveNowhere:
                    {
                        MessageHelper.ShowError(owner,
                            "An unknown problem occurred activating the license.\n\n" +
                            "Please contact Interapptive for support.");

                        break;
                    }

                case LicenseActivationState.ActiveElsewhere:
                    {
                        using (LicenseActiveElsewhereDlg dlg = new LicenseActiveElsewhereDlg())
                        {
                            dlg.ShowDialog(owner);
                        }

                        break;
                    }

                case LicenseActivationState.Deactivated:
                    {
                        MessageHelper.ShowError(owner,
                            "Your ShipWorks license has been disabled.\n\n" +
                            "Reason: " + accountDetail.DisabledReason);

                        break;
                    }

                case LicenseActivationState.Canceled:
                    {
                        using (LicenseCanceledDlg dlg = new LicenseCanceledDlg())
                        {
                            dlg.ShowDialog(owner);
                        }

                        break;
                    }

                case LicenseActivationState.Invalid:
                    {
                        MessageHelper.ShowError(owner,
                            "The license entered is a valid ShipWorks license, but was\n" +
                            "not found in the Interapptive database.\n\n" +
                            "Please contact Interapptive for support.");

                        break;
                    }
            }
        }

        /// <summary>
        /// Check that the given license and store combination represents a potentiall valid ShipWorks license.
        /// </summary>
        private static bool CheckLicenseValidity(StoreEntity store, string licenseKey, IWin32Window owner)
        {
            // Check for empty
            if (licenseKey.Length == 0)
            {
                MessageHelper.ShowMessage(owner, "Please enter a license key.");

                return false;
            }

            // Parse the license
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);

            // Check for validity
            if (!license.IsValid)
            {
                MessageHelper.ShowError(owner,
                    "The license is not valid.\n\n" +
                    "Please check that the license was entered correctly.");

                return false;
            }

            if ((StoreTypeCode) store.TypeCode != license.StoreTypeCode)
            {
                MessageHelper.ShowInformation(owner,
                    string.Format(
                    "The license entered is a valid ShipWorks license, but it can only\n" +
                    "be used for {0} stores.", StoreTypeManager.GetType(license.StoreTypeCode).StoreTypeName));

                return false;
            }

            // See if its a metered license
            if (!license.IsMetered)
            {
                using (NeedMeteredLicenseDlg dlg = new NeedMeteredLicenseDlg())
                {
                    dlg.ShowDialog(owner);
                }

                return false;
            }

            return true;
        }
    }
}
