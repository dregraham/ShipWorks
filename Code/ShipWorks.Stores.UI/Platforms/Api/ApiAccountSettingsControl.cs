using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Api
{
    /// <summary>
    /// Control for setting up an API Account (currently, directs user to the hub)
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Api, ExternallyOwned = true)]
    public partial class ApiAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the hub homepage
        /// </summary>
        private void OnSettingsLabelClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://hub.shipworks.com/products", this);
        }

        /// <summary>
        /// Don't allow user to select text
        /// </summary>
        private void OnSettingsLabelSelectionChanged(object sender, EventArgs e)
        {
            if (SettingsLabel.SelectionLength > 0)
            {
                SettingsLabel.SelectionLength = 0;
            }
        }
    }
}
