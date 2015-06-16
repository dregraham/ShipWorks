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
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    public partial class NeweggMarketplacePage : AddStoreWizardPage
    {
        public NeweggMarketplacePage()
        {
            InitializeComponent();            
        }

        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            NeweggStoreEntity newEggStore = GetStore<NeweggStoreEntity>();

            if (!storeSettingsControl.SaveToEntity(newEggStore))
            {
                e.NextPage = this;
            }
        }
    }
}
