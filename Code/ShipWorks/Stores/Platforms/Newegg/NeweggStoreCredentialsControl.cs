using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Control for obtaining the Newegg store credentials (seller ID and secret key).
    /// </summary>
    public partial class NeweggStoreCredentialsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggStoreCredentialsControl"/> class.
        /// </summary>
        public NeweggStoreCredentialsControl()            
        {
            InitializeComponent();

            EnumHelper.BindComboBox<NeweggChannelType>(marketplace);
        }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>The seller ID.</value>
        public string SellerId
        {
            get { return sellerId.Text; }
            set { sellerId.Text = value; }
        }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>The secret key.</value>
        public string SecretKey 
        {
            get { return secretKey.Text; }
            set { secretKey.Text = value; }
        }

        /// <summary>
        /// Gets or sets the Marketplace.
        /// </summary>
        /// <value>The secret key.</value>
        public int Marketplace
        {
            get { return (int)marketplace.SelectedValue; }
            set { marketplace.SelectedValue = (NeweggChannelType)value; }
        }
    }
}
