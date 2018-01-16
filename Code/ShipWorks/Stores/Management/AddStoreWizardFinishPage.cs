using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Finish page for the add store wizard
    /// </summary>
    public partial class AddStoreWizardFinishPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreWizardFinishPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the given control into the download section of the page 
        /// </summary>
        /// <param name="downloadControl"></param>
        public void SetDownloadSection(UserControl downloadControl)
        {
            downloadPanel.Controls.Clear();
            downloadPanel.Controls.Add(downloadControl);
        }

        /// <summary>
        /// User clicked the link to open the getting started guide
        /// </summary>
        private void OnLinkGettingStartedGuide(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/104800-getting-started-with-shipworks", this);
        }
    }
}
