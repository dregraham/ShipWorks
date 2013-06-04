using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Properties;
using System.Drawing;
using ShipWorks.Stores.Platforms;
using System.Windows.Forms;
using ShipWorks.Stores;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a single dashboard item representing a store trial
    /// </summary>
    class DashboardTrialItem : DashboardItem
    {
        TrialDetail trial;

        // So we know when something has changed
        bool wasConverted = false;
        bool wasExpired = false;
        int days = 100;
        string storeName;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardTrialItem(TrialDetail trial)
        {
            this.trial = trial;
        }

        /// <summary>
        /// The trial information represented by this item
        /// </summary>
        public TrialDetail TrialDetail
        {
            get { return trial; }
        }

        /// <summary>
        /// Set the bar that the trial information will be displayed in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            dashboardBar.Image = Resources.clock;
            dashboardBar.CanUserDismiss = false;

            UpdateTrialDisplay();
        }

        /// <summary>
        /// Update the display of the trial information
        /// </summary>
        public void UpdateTrialDisplay()
        {
            // See if the UI requires updating
            if (storeName != trial.Store.StoreName ||
                days != trial.DaysRemaining ||
                wasConverted != trial.IsConverted ||
                wasExpired != trial.IsExpired)
            {
                if (trial.IsConverted)
                {
                    DashboardBar.Image = Resources.clock_stop;
                    DashboardBar.PrimaryText = "Trial Expired";
                    DashboardBar.SecondaryText = string.Format("A license has been purchased for '{0}'.", trial.Store.StoreName);

                    DashboardBar.ApplyActions(new List<DashboardAction> {
                        new DashboardActionMethod("[link]Enter your license[/link] now.", OnEnterLicense, null) });
                }
                else if (trial.IsExpired)
                {
                    DashboardBar.Image = Resources.clock_stop;
                    DashboardBar.PrimaryText = "Trial Expired";
                    DashboardBar.SecondaryText = string.Format("The trial for '{0}' has expired.", trial.Store.StoreName);

                    if (trial.CanExtend)
                    {
                        DashboardBar.ApplyActions(new List<DashboardAction> {
                            new DashboardActionMethod("Need more time? [link]Click here [/link] to extend your trial.", OnExtendTrial, null) });
                    }
                    else
                    {
                        AddEnterAndSignUpActions();
                    }
                }
                else
                {
                    DashboardBar.Image = Resources.clock;
                    DashboardBar.PrimaryText = string.Format("{0} Days", trial.DaysRemaining);
                    DashboardBar.SecondaryText = string.Format("remaining in trial for '{0}'.", trial.Store.StoreName);

                    AddEnterAndSignUpActions();
                }

                storeName = trial.Store.StoreName;
                days = trial.DaysRemaining;
                wasConverted = trial.IsConverted;
                wasExpired = trial.IsExpired;
            }
        }

        /// <summary>
        /// Add the actions to allow for entering a license or signing up
        /// </summary>
        private void AddEnterAndSignUpActions()
        {
            StoreType storeType = StoreTypeManager.GetType(trial.Store);

            DashboardBar.ApplyActions(new List<DashboardAction> {
                new DashboardActionMethod("[link]Enter a license[/link] or ", OnEnterLicense, null),
                new DashboardActionUrl(
                    "[link]sign up[/link] now.", 
                    "https://www.interapptive.com/store/?store=" + storeType.TangoCode) });
        }

        /// <summary>
        /// User wants to extend the trial
        /// </summary>
        private void OnExtendTrial(Control owner, object userState)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                trial = TangoWebClient.ExtendTrial(trial.Store);

                UpdateTrialDisplay();
            }
            catch (ShipWorksLicenseException ex)
            {
                MessageHelper.ShowError(owner, ex.Message);
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(owner, ex.Message);
            }
        }

        /// <summary>
        /// User wants to extend the trial
        /// </summary>
        private void OnEnterLicense(Control owner, object userState)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (ChangeLicenseDlg dlg = new ChangeLicenseDlg(trial.Store))
            {
                // When the window closes, it should trigger a heartbeat right away, which should detect the store
                // changed, which should update the dashboard to remove this line.
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(trial.Store);
                    }
                }
            }
        }
    }
}
