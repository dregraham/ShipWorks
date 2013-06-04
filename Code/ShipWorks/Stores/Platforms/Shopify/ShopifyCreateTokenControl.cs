﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Controls.Html;
using log4net;
using System.Net;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Control to manage the Shopify Access Token
    /// </summary>
    public partial class ShopifyCreateTokenControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyCreateTokenControl));

        ShopifyStoreEntity store = null;

        bool showTokenInfo = true;

        /// <summary>
        /// Raised when a new token has been created and the store entity updated
        /// </summary>
        public event EventHandler TokenCreated;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateTokenControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates if information about the imported token should be displayed
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowTokenInfo
        {
            get
            {
                return showTokenInfo;
            }
            set
            {
                showTokenInfo = value;

                UpdateStatusDisplay();
            }
        }

        /// <summary>
        /// Used to determine if the token is valid.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAccessTokenValid
        {
            get
            {
                return store != null && !string.IsNullOrWhiteSpace(store.ShopifyAccessToken);
            }
        }

        /// <summary>
        /// Initialize a Shopify store for this control
        /// </summary>
        public void InitializeForStore(ShopifyStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentException("A non Shopify store was passed to osc account settings.");
            }

            this.store = store;
        }

        /// <summary>
        /// Fires when user clicks Grant Access, to allow ShipWorks access to the user's Shopify store.
        /// </summary>
        private void OnCreateToken(object sender, EventArgs e)
        {
            if (store == null)
            {
                throw new InvalidOperationException("The control has not been initialized with a store");
            }

            using (ShopifyCreateTokenWizard wizard = new ShopifyCreateTokenWizard())
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    store.SaveFields("");

                    store.ShopifyShopUrlName = wizard.ShopUrlName;
                    store.ShopifyAccessToken = wizard.ShopAccessToken;

                    try
                    {
                        ShopifyWebClient webClient = new ShopifyWebClient(store);
                        webClient.RetrieveShopInformation();

                        OnTokenCreate();
                    }
                    catch (Exception ex)
                    {
                        store.RollbackFields("");

                        throw WebHelper.TranslateWebException(ex, typeof(ShopifyException));
                    }
                }
            }
        }

        /// <summary>
        /// A token has been created
        /// </summary>
        private void OnTokenCreate()
        {
            UpdateStatusDisplay();

            if (TokenCreated != null)
            {
                TokenCreated(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the display of the information status
        /// </summary>
        public void UpdateStatusDisplay()
        {
            if (IsAccessTokenValid && showTokenInfo)
            {
                labelStatus.Text = string.Format("Authorized to '{0}'", store.ShopifyShopDisplayName);
                imageStatus.Image = Resources.check16;

                labelStatus.Visible = true;
                imageStatus.Visible = true;
            }
            else
            {
                labelStatus.Visible = false;
                imageStatus.Visible = false;
            }
        }
    }
}
