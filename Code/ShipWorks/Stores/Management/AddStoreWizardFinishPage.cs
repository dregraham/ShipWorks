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
    public partial class AddStoreWizardFinishPage : AddStoreWizardPage
    {
        public AddStoreWizardFinishPage()
        {
            InitializeComponent();
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
