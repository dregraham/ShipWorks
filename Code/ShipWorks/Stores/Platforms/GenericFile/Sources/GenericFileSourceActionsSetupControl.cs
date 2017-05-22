using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.IO;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Reusable control to help setup the post-import success and error actions
    /// </summary>
    public partial class GenericFileSourceActionsSetupControl : UserControl
    {
        public event GenericFileSourceFolderBrowseEventHandler BrowseForSuccessFolder;
        public event GenericFileSourceFolderBrowseEventHandler BrowseForErrorFolder;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceActionsSetupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control
        /// </summary>
        public void Initialize(IEnumerable<object> successOptions, IEnumerable<object> errorOptions)
        {
            // Load the success combo
            comboSuccessAction.DisplayMember = "Display";
            comboSuccessAction.ValueMember = "Value";
            comboSuccessAction.DataSource = successOptions.ToList();

            // Load the error combo
            comboErrorAction.DisplayMember = "Display";
            comboErrorAction.ValueMember = "Value";
            comboErrorAction.DataSource = errorOptions.ToList();
        }

        /// <summary>
        /// Load the settings from the given store into the UI
        /// </summary>
        public void LoadStore(GenericFileStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Success
            comboSuccessAction.SelectedValue = (GenericFileSuccessAction) store.SuccessAction;
            successFolder.Text = store.SuccessMoveFolder;
            
            // Error
            comboErrorAction.SelectedValue = (GenericFileErrorAction) store.ErrorAction;
            errorFolder.Text = store.ErrorMoveFolder;
        }

        /// <summary>
        /// Save the UI settings to the given store.  Returns false if there was a problem and an error was displayed.
        /// </summary>
        public bool SaveToEntity(GenericFileStoreEntity store, string sourceFolder)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            GenericFileSuccessAction successAction = (GenericFileSuccessAction) comboSuccessAction.SelectedValue;
            GenericFileErrorAction errorAction = (GenericFileErrorAction) comboErrorAction.SelectedValue;

            if (successAction == GenericFileSuccessAction.Move)
            {
                if (!CheckValidFolder(successFolder.Text, "folder into which files should be moved after successfully importing all orders"))
                {
                    return false;
                }

                if (sourceFolder == successFolder.Text)
                {
                    MessageHelper.ShowError(this, "The success move folder must be different than the import folder");
                    return false;
                }
            }

            if (errorAction == GenericFileErrorAction.Move)
            {
                //if (!CheckValidFolder(errorFolder.Text, "error move folder"))
                if (!CheckValidFolder(errorFolder.Text, "folder into which files should be moved after an error occurs while importing orders"))
                {
                    return false;
                }

                if (sourceFolder == errorFolder.Text)
                {
                    MessageHelper.ShowError(this, "The error move folder must be different than the import folder");
                    return false;
                }
            }

            store.SuccessAction = (int) successAction;
            store.SuccessMoveFolder = successFolder.Text;

            store.ErrorAction = (int) errorAction;
            store.ErrorMoveFolder = errorFolder.Text;

            return true;
        }


        /// <summary>
        /// Changing the success action option
        /// </summary>
        private void OnChangeSuccessAction(object sender, EventArgs e)
        {
            // If for whatever reason an invalid value was selected, select a valid one
            if (comboSuccessAction.SelectedIndex == -1)
            {
                if (comboSuccessAction.Items.Count > 0)
                {
                    comboSuccessAction.SelectedIndex = 0;
                }

                return;
            }
                
            GenericFileSuccessAction action = (GenericFileSuccessAction) comboSuccessAction.SelectedValue;

            bool showFolder = (action == GenericFileSuccessAction.Move);

            panelSuccessFolder.Visible = showFolder;
            panelError.Top = showFolder ? panelSuccessFolder.Bottom + 1 : comboSuccessAction.Bottom + 5;

            Height = panelError.Bottom + 5;
        }

        /// <summary>
        /// Changing the error action option
        /// </summary>
        private void OnChangeErrorAction(object sender, EventArgs e)
        {
            // If for whatever reason an invalid value was selected, select a valid one
            if (comboErrorAction.SelectedIndex == -1)
            {
                if (comboErrorAction.Items.Count > 0)
                {
                    comboErrorAction.SelectedIndex = 0;
                }

                return;
            }

            GenericFileErrorAction action = (GenericFileErrorAction) comboErrorAction.SelectedValue;

            bool showFolder = (action == GenericFileErrorAction.Move);

            panelErrorFolder.Visible = showFolder;
            panelError.Height = 2 + (showFolder ? panelErrorFolder.Bottom : comboErrorAction.Bottom);

            Height = panelError.Bottom + 5;
        }

        /// <summary>
        /// Browse for the success folder
        /// </summary>
        private void OnBrowseSuccessFolder(object sender, EventArgs e)
        {
            if (BrowseForSuccessFolder != null)
            {
                var args = new GenericFileSourceFolderBrowseEventArgs(successFolder.Text);

                BrowseForSuccessFolder(this, args);
                successFolder.Text = args.Folder;
            }
        }

        /// <summary>
        /// Browse for the error folder
        /// </summary>
        private void OnBrowseErrorFolder(object sender, EventArgs e)
        {
            if (BrowseForErrorFolder != null)
            {
                var args = new GenericFileSourceFolderBrowseEventArgs(errorFolder.Text);

                BrowseForErrorFolder(this, args);
                errorFolder.Text = args.Folder;
            }
        }

        /// <summary>
        /// Check the given path to ensure that it's a valid folder
        /// </summary>
        public bool CheckValidFolder(string path, string description)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageHelper.ShowInformation(this, string.Format("Please specify the {0}.", description));
                return false;
            }

            return true;
        }
    }
}
