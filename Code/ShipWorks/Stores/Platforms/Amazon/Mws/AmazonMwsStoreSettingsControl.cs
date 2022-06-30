using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Store-specific configuration settings for Amazon.  This does not include
    /// connectivity settings, those are in the AccountSettingsControl
    /// </summary>
    public partial class AmazonMwsStoreSettingsControl : StoreSettingsControlBase
    {
        private const string amazonVATSWarning =
            "Enabling the Amazon VATS toggle will stop ShipWorks from downloading \"Tax\" order charges, because the VATS tax is already included in each item total. Are you sure you want to do this?";

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsStoreSettingsControl()
        {
            InitializeComponent();

            amazonVATS.ValueMember = "Key";
            amazonVATS.DisplayMember = "Value";
            amazonVATS.DataSource = new Dictionary<bool, string>
            {
                { false, "Disabled" },
                { true, "Enabled" }
            }.ToList();
        }

        /// <summary>
        /// Load configuration from the store entity into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            amazonVATS.SelectedValueChanged -= OnAmazonVATSSelectedValueChanged;

            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon store settings control.");
            }

            excludeFba.Checked = amazonStore.ExcludeFBA;

            bool showVATS = ShowVATS(amazonStore);

            amazonVATS.SelectedValue = amazonStore.AmazonVATS;
            amazonVATS.Visible = showVATS;
            amazonVATSLabel.Visible = showVATS;

            if (!showVATS)
            {
                Height -= 20;
            }

            amazonVATS.SelectedValueChanged += OnAmazonVATSSelectedValueChanged;
        }

        /// <summary>
        /// Save user-entered data back to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon store settings control.");
            }

            amazonStore.ExcludeFBA = excludeFba.Checked;
            if (ShowVATS(amazonStore))
            {
                amazonStore.AmazonVATS = (bool) amazonVATS.SelectedValue;
            }

            return true;
        }

        public override async Task<bool> SaveToPlatform(StoreEntity store)
        {
            AmazonStoreEntity storeEntity = store as AmazonStoreEntity;
            if(storeEntity.ExcludeFBA != excludeFba.Checked)
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var orderSourceClient = lifetimeScope.Resolve<IHubOrderSourceClient>();
                    try
                    {
                        await orderSourceClient.UpdateAmazonFbaCriteria(store.OrderSourceID, excludeFba.Checked, storeEntity.AmazonApiRegion).ConfigureAwait(false);
                        return true;
                    }
                    catch(Exception ex)
                    {
                        var messageHelper = lifetimeScope.Resolve<IMessageHelper>();
                        var loggerFactory = lifetimeScope.Resolve<Func<Type, ILog>>();
                        var logger = loggerFactory(typeof(AmazonMwsStoreSettingsControl));

                        logger.Error("An error occured updateing the amazon store settings in platform", ex);
                        messageHelper.ShowError("Failed to update settings. Please try again.");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Should we show the VATS UI
        /// </summary>
        private bool ShowVATS(IAmazonStoreEntity store) => store.AmazonApiRegion == "UK";

        /// <summary>
        /// Handle changing the Amazon VATS option
        /// </summary>
        private void OnAmazonVATSSelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox vats = sender as ComboBox;
            if (vats == null || (bool) vats.SelectedValue == false)
            {
                return;
            }

            vats.SelectedValue = MessageBox.Show(this, amazonVATSWarning, "Amazon VATS", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }
}
