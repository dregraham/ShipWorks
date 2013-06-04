using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.UI;
using System.Data.SqlTypes;
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

            return true;
        }
    }
}
