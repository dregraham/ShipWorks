using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Store-specific configuration settings for Amazon.  This does not include
    /// connectivity settings, those are in the AccountSettingsControl
    /// </summary>
    public partial class AmazonMwsStoreSettingsControl : StoreSettingsControlBase
    {
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

        /// <summary>
        /// Should we show the VATS UI
        /// </summary>
        private bool ShowVATS(IAmazonStoreEntity store) => store.AmazonApiRegion == "UK";
    }
}
