using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using System.Net;
using System.Xml;
using System.IO;
using ShipWorks.UI;
using log4net;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Control for configuring eBay's account settings
    /// </summary>
    public partial class EbayAccountSettingsControl : AccountSettingsControlBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EbayAccountSettingsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            EbayStoreEntity ebayStore = store as EbayStoreEntity;
            if (ebayStore == null)
            {
                throw new InvalidOperationException("A non eBay store was passed to the eBay Account Settings Control.");
            }

            tokenManageControl.InitializeForStore(ebayStore);
        }
    }
}
