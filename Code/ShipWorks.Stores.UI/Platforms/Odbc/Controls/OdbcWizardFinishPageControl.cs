using System;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// Control to display on the finish page
    /// </summary>
    public partial class OdbcWizardFinishPageControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcWizardFinishPageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open a link to the Single Scan documentation page
        /// </summary>
        private void OnClickLinkUseABarcodeScanner(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022459692", this);
        }
    }
}
