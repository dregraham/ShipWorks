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
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.UI.Platforms.Api
{
    /// <summary>
    /// Control for setting up an API Account (currently, directs user to the hub)
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Api, ExternallyOwned = true)]
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.BrightpearlHub, ExternallyOwned = true)]
    public partial class ApiAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiAccountSettingsControl(WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            InitializeComponent();
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
        }

        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);
            this.store = store;
            SettingsLabel.Text = $"To update {EnumHelper.GetDescription(store.StoreTypeCode)} store settings, go to https://hub.shipworks.com.";
        }

        /// <summary>
        /// Open the hub homepage
        /// </summary>
        private void OnSettingsLabelClick(object sender, EventArgs e)
        {
            string url = webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl;
            var builder = new UriBuilder(url);
            builder.Path = store.StoreTypeCode == StoreTypeCode.Api ? "/apiSettings" : "/settings";
            WebHelper.OpenUrl(builder.Uri, this);
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
