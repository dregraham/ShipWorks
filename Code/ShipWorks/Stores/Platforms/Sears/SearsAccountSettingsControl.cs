using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using System;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Account settings for sears.com
    /// </summary>
    public partial class SearsAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsAccountSettingsControl()
        {
            InitializeComponent();

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                encryptionProvider = scope.Resolve<IEncryptionProviderFactory>().CreateSearsEncryptionProvider();
            }
        }

        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            SearsStoreEntity searsStore = store as SearsStoreEntity;
            if (searsStore == null)
            {
                throw new ArgumentException("A non SearsStore store was passed to SearsStore account settings.");
            }

            email.Text = searsStore.SearsEmail;
            sellerID.Text = searsStore.SellerID;

            try
            {
                secretKey.Text = string.IsNullOrWhiteSpace(searsStore.SecretKey)
                    ? string.Empty
                    : encryptionProvider.Decrypt(searsStore.SecretKey);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(SearsAccountSettingsControl)).Error("Couldn't decrypt Secret Key", ex);

                secretKey.Text = string.Empty;
            }
        }

        /// <summary>
        /// Saves the user selected settings back to the store entity;
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            SearsStoreEntity searsStore = store as SearsStoreEntity;
            if (searsStore == null)
            {
                throw new ArgumentException("A non SearsStore store was passed to SearsStore account settings.");
            }

            if (string.IsNullOrWhiteSpace(secretKey.Text))
            {
                MessageHelper.ShowError(this, "Please enter a Secret Key.");
                return false;
            }

            searsStore.Email = email.Text;
            searsStore.SearsEmail = email.Text;
            searsStore.SellerID = sellerID.Text;
            searsStore.SecretKey = encryptionProvider.Encrypt(secretKey.Text);

            // see if we need to test the settings because they changed in some way
            if (!ConnectionVerificationNeeded(searsStore))
            {
                return true;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SearsWebClient webClient = new SearsWebClient(searsStore);
                webClient.TestConnection();

                return true;
            }
            catch (SearsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return false;
            }
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(SearsStoreEntity searsStore)
        {
            return (searsStore.Fields[(int) SearsStoreFieldIndex.SearsEmail].IsChanged ||
                    searsStore.Fields[(int) SearsStoreFieldIndex.SellerID].IsChanged ||
                    searsStore.Fields[(int) SearsStoreFieldIndex.SecretKey].IsChanged);
        }
    }
}
