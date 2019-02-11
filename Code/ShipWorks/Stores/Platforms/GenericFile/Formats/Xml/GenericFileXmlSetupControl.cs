using System;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    /// <summary>
    /// User control for configuring the XML import type
    /// </summary>
    public partial class GenericFileXmlSetupControl : UserControl
    {
        private bool isVerified;

        // If the user has changed\cleared the XSLT file to use, this will reflect that as the filename or empty string.
        private string pendingXslFile;
        private string originalXslContent;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileXmlSetupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the entity
        /// </summary>
        public void LoadStore(GenericFileStoreEntity generic)
        {
            pendingXslFile = null;
            IsVerified = false;

            // If an XSLT is set
            if (generic.XmlXsltFileName != null)
            {
                xsltPath.Text = Path.GetFileName(generic.XmlXsltFileName) + " (Saved in Database)";
                originalXslContent = generic.XmlXsltContent;
            }
        }

        /// <summary>
        /// Save the data in the control to the given entity. 
        /// </summary>
        public bool SaveToEntity(GenericFileStoreEntity store)
        {
            // See if they changed\cleared the XSLT file
            if (pendingXslFile != null)
            {
                // A file was set
                if (!string.IsNullOrWhiteSpace(pendingXslFile))
                {
                    string xsltContent = ReadXslFile(pendingXslFile);

                    // Null indicates failure
                    if (xsltContent == null)
                    {
                        return false;
                    }

                    store.XmlXsltFileName = pendingXslFile;
                    store.XmlXsltContent = xsltContent;
                }
                // It was cleared
                else
                {
                    store.XmlXsltFileName = null;
                    store.XmlXsltContent = null;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets whether the XML input has been verified to work
        /// </summary>
        public bool IsVerified
        {
            get
            {
                return isVerified;
            }
            set
            {
                isVerified = value;

                panelVerifySuccess.Visible = isVerified;
            }
        }

        /// <summary>
        /// Clear the selected stylesheet
        /// </summary>
        private void OnClearXslt(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(xsltPath.Text))
            {
                IsVerified = false;
            }

            pendingXslFile = "";
            xsltPath.Text = "";
        }

        /// <summary>
        /// Browse for an XSLT file
        /// </summary>
        private void OnXsltBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XSLT Files (*.xsl, *.xslt)|*.xsl;*.xslt";

                // Don't let the window close unless the pick something valid or cancel
                dlg.FileOk += (unused, cancelArgs) =>
                    {
                        cancelArgs.Cancel = !LoadXslTransform(dlg.FileName);
                    };

                // If closed OK, its a valid XSLT file
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // If it changed will have to reverify under the new XSLT
                    if (xsltPath.Text != dlg.FileName)
                    {
                        IsVerified = false;
                    }

                    pendingXslFile = dlg.FileName;
                    xsltPath.Text = dlg.FileName;
                }
            }
        }

        /// <summary>
        /// Load the given XSL file
        /// </summary>
        private bool LoadXslTransform(string xsltFile)
        {
            // Read the contents of the file
            string xslContent = ReadXslFile(xsltFile);
            if (xslContent == null)
            {
                return false;
            }

            try
            {
                // Attempt to load it right now just for a quick verifcation its ok.  It will be loaded again each time its processed.
                GenericFileXmlUtility.LoadXslTransform(xslContent);

                return true;
            }
            catch (GenericFileStoreException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Read the contents of the file.  If the file couldn't be read and an error was displayed, null is returned.
        /// </summary>
        private string ReadXslFile(string xsltFile)
        {
            try
            {
                return File.ReadAllText(xsltFile);
            }
            catch (IOException ex)
            {
                MessageHelper.ShowMessage(this, "ShipWorks could not read the specified file: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageHelper.ShowMessage(this, "ShipWorks could not read the specified file: " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Verify that a user's selected XML file will be loadable by ShipWorks
        /// </summary>
        private void OnVerify(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select Example XML File";
                dlg.Filter = "XML Files (*.xml)|*.xml";

                // Don't let the window close unless the pick something valid or cancel
                dlg.FileOk += (unused, cancelArgs) =>
                    {
                        try
                        {
                            string xslContent = null;

                            // If they've specfied the XSLT to use since loading this store
                            if (pendingXslFile != null)
                            {
                                if (!string.IsNullOrWhiteSpace(pendingXslFile))
                                {
                                    xslContent = ReadXslFile(xsltPath.Text);

                                    // Invalid XSLT file if it returns null
                                    if (xslContent == null)
                                    {
                                        cancelArgs.Cancel = true;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                // Use what's already existant for the store
                                xslContent = originalXslContent;
                            }

                            GenericFileXmlUtility.LoadAndValidateDocument(dlg.FileName, xslContent);
                        }
                        catch (GenericFileStoreException ex)
                        {
                            MessageHelper.ShowError(this, ex.Message);

                            cancelArgs.Cancel = true;
                        }
                    };

                // If closed OK, its a valid file
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    IsVerified = true;
                }
            }
        }

        /// <summary>
        /// Open the ShipWorks page for the generic schema documentation
        /// </summary>
        private void OnLinkSchemaDocumentation(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022460732", this);
        }
    }
}
