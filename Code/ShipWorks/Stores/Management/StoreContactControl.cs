using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using log4net;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// User control for editing the contact details of a store
    /// </summary>
    partial class StoreContactControl : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(StoreContactControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreContactControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the contact information from the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            email.Text = store.Email;
            phone.Text = store.Phone;
            website.Text = store.Website;
        }

        /// <summary>
        /// Save the contact information for the given store
        /// </summary>
        public void SaveToEntity(StoreEntity store)
        {
            store.Email = email.Text;
            store.Phone = phone.Text;
            store.Website = website.Text;
        }

        /// <summary>
        /// Select the image to use for the store logo
        /// </summary>
        private void OnSelectLogo(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(this);
        }

        /// <summary>
        /// User is trying to select a logo image
        /// </summary>
        private void OnLogoSelectOK(object sender, CancelEventArgs e)
        {
            try
            {
                Image image = Image.FromFile(openFileDialog.FileName);

                pictureBoxLogo.Image = image;
            }
            catch (Exception ex)
            {
                log.Error("Error loading selected image file.", ex);

                MessageHelper.ShowMessage(this, "The selected file could not be opened or is not a valid image file.");

                e.Cancel = true;
            }
        }
    }
}
