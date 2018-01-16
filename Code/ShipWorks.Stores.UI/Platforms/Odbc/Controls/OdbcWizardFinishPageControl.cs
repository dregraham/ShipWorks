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
    [KeyedComponent(typeof(IStoreWizardFinishPageControl), StoreTypeCode.Odbc)]
    public partial class OdbcWizardFinishPageControl : UserControl, IStoreWizardFinishPageControl
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
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000096210-searching-for-orders-using-single-scan-v5-10-or-greater-", this);
        }
    }
}
