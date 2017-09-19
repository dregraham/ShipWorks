using System;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Dialog for loading a GenericModuleResponse from a file, validating it,
    /// and making it available to the caller.
    /// </summary>
    public partial class GenericStoreResponseLoadDlg : Form
    {
        // The web client for the store
        IGenericStoreWebClient client = null;

        // the loaded response
        GenericModuleResponse loadedResponse = null;

        // the file that was loaded
        string loadedFileName = "";

        /// <summary>
        /// Gets the filename used to load the response
        /// </summary>
        public string LoadedFileName
        {
            get { return loadedFileName; }
        }

        /// <summary>
        /// Gets the mock response, loaded from the user-selected file
        /// </summary>
        public GenericModuleResponse LoadedResponse
        {
            get { return loadedResponse; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreResponseLoadDlg(IGenericStoreWebClient client)
        {
            InitializeComponent();

            this.client = client;
        }

        /// <summary>
        /// Import clicked
        /// </summary>
        private void OnImportClick(object sender, EventArgs e)
        {
            if (importFile.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please select an Order XML file to import.");
                return;
            }

            try
            {
                // Load the response from file, mocking a call with action getorders
                loadedResponse = client.ResponseFromFile(importFile.Text.Trim(), "getorders");

                // remember the file used
                loadedFileName = importFile.Text;

                // done
                DialogResult = DialogResult.OK;
            }
            catch (FileNotFoundException ex)
            {
                MessageHelper.ShowError(this, "Unable to locate the specified file: " + ex.FileName);
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, "There was an error loading this file.  Is it a module response?:\n\n" + ex.Message);
            }
        }
    }
}
