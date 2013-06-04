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

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Page for configuring what the online store's TimeZone setting is
    /// </summary>
    public partial class VolusionTimeZonePage : VolusionAddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionTimeZonePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            store.ServerTimeZone = timeZoneControl.SelectedTimeZone.Id;

        }

        /// <summary>
        /// Load the TimeZone information from the store entity
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            timeZoneControl.SelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(store.ServerTimeZone);
        }
    }
}
