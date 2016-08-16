using System;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using Interapptive.Shared.UI;
using log4net;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Account settings control for Buy.com
    /// </summary>
    public partial class BuyDotComAccountSettingsControl : AccountSettingsControlBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComAccountSettingsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load Settings from store Entity.
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            BuyDotComStoreEntity buyDotComStore = store as BuyDotComStoreEntity;
            if (buyDotComStore == null)
            {
                throw new ArgumentException("A non BuyDotCom store was passed to BuyDotCom account settings.");
            }

            username.Text = buyDotComStore.FtpUsername;
            password.Text = SecureText.Decrypt(buyDotComStore.FtpPassword, buyDotComStore.FtpUsername);
        }

        /// <summary>
        /// Save to Entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            BuyDotComStoreEntity buyDotComStore = store as BuyDotComStoreEntity;

            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter your FTP username and password.");
                return false;
            }

            // Only test connection if something changed
            if (username.Text != buyDotComStore.FtpUsername ||
                password.Text != SecureText.Decrypt(buyDotComStore.FtpPassword, buyDotComStore.FtpUsername))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    FtpAccountEntity account = BuyDotComUtility.GetFtpAccount(username.Text, password.Text);

                    // Tets the credentials
                    FtpUtility.TestDataTransfer(account);

                    // Success - save
                    buyDotComStore.FtpUsername = username.Text;
                    buyDotComStore.FtpPassword = SecureText.Encrypt(password.Text, username.Text);
                }
                catch (FileTransferException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// AddressLink Click Event
        /// </summary>
        private void OnEmailAddressLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenMailTo("mp-integration@buy.com", this);
        }
    }
}