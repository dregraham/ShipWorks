using System;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// Wizard page that validates the user's freemium account before continuing
    /// </summary>
    public partial class FreemiumStoreWizardValidateAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FreemiumStoreWizardValidateAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.StepReason == WizardStepReason.StepBack)
            {
                return;
            }

            bool canContinue = CheckFreemiumStatus();

            if (canContinue)
            {
                e.Skip = true;
            }
            else
            {
                Wizard.NextEnabled = false;
            }
        }

        /// <summary>
        /// Check the freemium status of the current store-in-progress to see if its OK to continue
        /// </summary>
        private bool CheckFreemiumStatus()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                EbayStoreEntity store = GetStore<EbayStoreEntity>();
                LicenseAccountDetail accountDetail = TangoWebClient.GetFreemiumStatus(store);

                // If it's valid, that means the user has a store already for this eBay User
                if (accountDetail.Valid)
                {
                    FreemiumFreeEdition freemiumEdition = accountDetail.Edition as FreemiumFreeEdition;

                    // If its not a freemium license, then they can never do freemium
                    if (freemiumEdition == null)
                    {
                        labelInfo.Text = string.Format("You have already signed up for ShipWorks using eBay ID '{0}', and are not eligible for the free version of ShipWorks.",
                            store.EBayUserID);

                        return false;
                    }
                    else
                    {
                        // They had a freemium account, but then something happened (canceled\disabled\whatever)
                        if (accountDetail.ActivationState != LicenseActivationState.Active)
                        {
                            labelInfo.Text = string.Format(
                                "You have already signed up for ShipWorks and Endicia using eBay ID '{0}'.\n\n" +
                                "You cannot continue because of the current status of your license: {1}.",
                                store.EBayUserID,
                                accountDetail.Description);

                            return false;
                        }
                        // This means the ELS account has been created - let them continue, they'll set that up in the ELS wizard.
                        else
                        {
                            store.License = accountDetail.Key;
                            store.Edition = accountDetail.Edition.Serialize();

                            return true;
                        }
                    }
                }
                // No valid license means this is brand new (or just in a trial)
                else
                {
                    // Either the trial exists or a new one will be created

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ShipWorksLicenseException || ex is TangoException)
                {
                    labelInfo.Text = ex.Message;

                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
