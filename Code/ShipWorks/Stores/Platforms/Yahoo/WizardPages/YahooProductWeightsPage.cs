using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Yahoo.WizardPages
{
    /// <summary>
    /// Page for allowing the user to import yahoo product weight information
    /// </summary>
    public partial class YahooProductWeightsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooProductWeightsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            importProductsControl.InitializeForStore(GetStore<YahooStoreEntity>().StoreID);
        }

        /// <summary>
        /// Stepping next from the prodcut import page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (radioNoWeights.Checked)
            {
                return;
            }

            if (!importProductsControl.ImportComplete)
            {
                MessageHelper.ShowInformation(this, "Import your Yahoo! product catalog before continuing.");
                e.NextPage = this;
                return;
            }
        }

        /// <summary>
        /// Changing wether to import weights or not
        /// </summary>
        private void OnRadioSelectionChanged(object sender, EventArgs e)
        {
            importProductsControl.Enabled = radioUseWeights.Checked;
        }
    }
}
