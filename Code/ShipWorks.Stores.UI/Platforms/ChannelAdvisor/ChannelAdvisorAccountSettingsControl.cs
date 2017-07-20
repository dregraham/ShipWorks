using System;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Account settings control for ChannelAdvisor
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Management.AccountSettingsControlBase" />
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.ChannelAdvisor)]
    public partial class ChannelAdvisorAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IChannelAdvisorAccountSettingsViewModel restViewModel;
        private ChannelAdvisorStoreEntity channelAdvisorStore;
        private bool restUser;
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorAccountSettingsControl"/> class.
        /// </summary>
        public ChannelAdvisorAccountSettingsControl(IChannelAdvisorAccountSettingsViewModel restViewModel)
        {
            this.restViewModel = restViewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        /// <exception cref="System.ArgumentException">A non ChannelAdvisor store was passed to ChannelAdvisor account settings.</exception>
        public override void LoadStore(StoreEntity store)
        {
            channelAdvisorStore = store as ChannelAdvisorStoreEntity;
            if (channelAdvisorStore == null)
            {
                throw new ArgumentException("A non ChannelAdvisor store was passed to ChannelAdvisor account settings.");
            }
            // REST user if refresh token exists
            restUser = !string.IsNullOrWhiteSpace(channelAdvisorStore.RefreshToken);

            if (restUser)
            {
                LoadRestControl(store);
            }
            else
            {
                LoadSoapControl(store);
            }
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            return restUser ? restSettingsControl.SaveToEntity(store) : soapSettingsControl.SaveToEntity(store);
        }

        /// <summary>
        /// Loads the SOAP control.
        /// </summary>
        private void LoadSoapControl(StoreEntity store)
        {
            soapPanel.Visible = true;
            soapPanel.Enabled = true;
            restSettingsControl.Visible = false;
            restSettingsControl.Enabled = false;

            soapSettingsControl.LoadStore(store);
        }

        /// <summary>
        /// Loads the REST control.
        /// </summary>
        private void LoadRestControl(StoreEntity store)
        {
            soapPanel.Visible = false;
            soapPanel.Enabled = false;
            restSettingsControl.Visible = true;
            restSettingsControl.Enabled = true;

            restSettingsControl.LoadStore(store, restViewModel);
        }

        /// <summary>
        /// Called when [click upgrade].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClickUpgrade(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this,
                "Are you sure you want to begin using the ChannelAdvisor REST API? You will not be able to go back to the SOAP API.");

            if (result == DialogResult.OK)
            {
                LoadRestControl(channelAdvisorStore);
                restUser = true;
            }
        }
    }
}
