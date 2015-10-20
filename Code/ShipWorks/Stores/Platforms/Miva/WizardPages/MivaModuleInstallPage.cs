using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// Wizard page for showing the user how to install the miva module
    /// </summary>
    public partial class MivaModuleInstallPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaModuleInstallPage()
        {
            InitializeComponent();

            MivaStoreType store = new MivaStoreType(new StoreEntity() {TypeCode =  (int) StoreTypeCode.Miva}) ;

            linkInstructions.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// Stepping into
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // Skip if they say its already installed
            e.Skip = ((MivaModuleQuestionPage) Wizard.FindPage(typeof(MivaModuleQuestionPage))).IsModuleInstalled;
        }

        /// <summary>
        /// Open the link for downloading the module
        /// </summary>
        private void OnLinkDownloadPage(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/shipworks/download.php", this);
        }
    }
}
