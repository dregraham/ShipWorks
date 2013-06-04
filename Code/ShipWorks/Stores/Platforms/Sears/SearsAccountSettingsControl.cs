using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Account settings for sears.com
    /// </summary>
    public partial class SearsAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearsAccountSettingsControl()
        {
            InitializeComponent();
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

            email.Text = searsStore.Email;
            password.Text = SecureText.Decrypt(searsStore.Password, searsStore.Email);
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

            searsStore.Email = email.Text;
            searsStore.Password = SecureText.Encrypt(password.Text, email.Text);

            // see if we need to test the settings because they changed in some way
            if (ConnectionVerificationNeeded(searsStore))
            {
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
            else
            {
                // Nothing changed
                return true;
            }
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(SearsStoreEntity searsStore)
        {
            return (searsStore.Fields[(int) SearsStoreFieldIndex.Email].IsChanged ||
                    searsStore.Fields[(int) SearsStoreFieldIndex.Password].IsChanged);
        }

    }
}
