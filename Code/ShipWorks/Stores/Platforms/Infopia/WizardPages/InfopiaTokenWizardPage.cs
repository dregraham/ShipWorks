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
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Infopia.WizardPages
{
    /// <summary>
    /// Setup Wizard page for inputting the Infopia User Token
    /// </summary>
    public partial class InfopiaTokenWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaTokenWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping to the next page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            InfopiaStoreEntity store = GetStore<InfopiaStoreEntity>();

            if (!accountSettings.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            store.StoreName = "Infopia Store";
        }
    }
}
