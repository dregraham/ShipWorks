using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Window for chosing the image for the UserInititated trigger
    /// </summary>
    public partial class UserInitiatedImageChooserDlg : Form
    {
        Image selectedImage = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserInitiatedImageChooserDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The Image the user selected.  Only valid if the DialogResult is OK.
        /// </summary>
        public Image SelectedImage
        {
            get { return selectedImage; }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            foreach (DictionaryEntry entry in UserInitiatedStockImages.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true).OfType<DictionaryEntry>().OrderBy(de => (string) de.Key))
            {
                imageList.Images.Add((Image) entry.Value);
            }

            listView.LargeImageList = imageList;

            for (int i = 0; i < imageList.Images.Count; i++)
            {
                listView.Items.Add("", i);
            }
        }

        /// <summary>
        /// Browse for an image
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// User is trying to select an image file
        /// </summary>
        private void OnOpenFileOK(object sender, CancelEventArgs e)
        {
            try
            {
                selectedImage = Image.FromFile(openFileDialog.FileName);
            }
            catch (Exception)
            {
                MessageHelper.ShowError(this, "The selected file is not a valid image.");

                e.Cancel = true;
            }
        }

        /// <summary>
        /// User has actively chosen an item from the list
        /// </summary>
        private void OnItemActivate(object sender, EventArgs e)
        {
            OnOK(ok, EventArgs.Empty);
        }

        /// <summary>
        /// User selected an item from the list
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageHelper.ShowInformation(this, "Please select an image.");
                return;
            }

            selectedImage = (Image) listView.LargeImageList.Images[listView.SelectedIndices[0]].Clone();

            DialogResult = DialogResult.OK;
        }
    }
}
