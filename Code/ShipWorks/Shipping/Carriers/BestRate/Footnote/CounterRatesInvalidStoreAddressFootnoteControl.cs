using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Footnote that notifies user that a store does not have an address
    /// </summary>
    public partial class CounterRatesInvalidStoreAddressFootnoteControl : RateFootnoteControl
    {
        private readonly FootnoteParameters parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        public CounterRatesInvalidStoreAddressFootnoteControl(FootnoteParameters parameters)
        {
            this.parameters = parameters;
            InitializeComponent();

            storeAddressLink.Visible = UserSession.Security?.HasPermission(PermissionType.ManageStores) ?? false;
        }

        /// <summary>
        /// The user has clicked the "Enter store address" link
        /// </summary>
        private void OnShowAddress(object sender, EventArgs e)
        {
            StoreEntity store = parameters.GetStoreAction();

            using (StoreSettingsDlg dialog = new StoreSettingsDlg(store))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StoreManager.CheckForChanges();
                    parameters.ReloadRatesAction();
                }
            }
        }
    }
}
