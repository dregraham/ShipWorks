using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// If a miva site has multiple stores this selects the correct store code
    /// </summary>
    public partial class MivaSelectStorePage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaSelectStorePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the store selection page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            MivaModuleLoginPage loginPage = (MivaModuleLoginPage) Wizard.FindPage(typeof(MivaModuleLoginPage));

            // Load the combo
            comboStores.DisplayMember = "Key";
            comboStores.ValueMember = "Value";
            comboStores.DataSource = loginPage.AvailableStores.Select(s => new KeyValuePair<string, string>(s.Name, s.Code)).ToList();
            comboStores.SelectedIndex = 0;

            // If there is only one we can just automatically choose it
            if (loginPage.AvailableStores.Count == 1)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Step next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MivaStoreEntity mivaStore = GetStore<MivaStoreEntity>();

            mivaStore.ModuleOnlineStoreCode = (string) comboStores.SelectedValue;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                MivaStoreType storeType = (MivaStoreType) StoreTypeManager.GetType(mivaStore);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = this;
                return;
            }
        }
    }
}
