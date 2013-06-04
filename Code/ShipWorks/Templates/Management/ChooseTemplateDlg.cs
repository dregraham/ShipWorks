using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Users;
using ShipWorks.Templates.Controls;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for choosing a template.  Dialog version of the template dropdown box
    /// </summary>
    public partial class ChooseTemplateDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChooseTemplateDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            templateTreeControl.LoadTemplates();
            templateTreeControl.ApplyFolderState(new FolderExpansionState(UserSession.User.Settings.TemplateExpandedFolders));

            ok.Enabled = false;
        }

        /// <summary>
        /// Returns the ID of the selected template.  Only valid if DialogResult = OK
        /// </summary>
        public long SelectedTemplateID
        {
            get { return templateTreeControl.SelectedID; }
        }

        /// <summary>
        /// Selected template node has changed
        /// </summary>
        private void OnSelectedTemplateChanged(object sender, TemplateNodeChangedEventArgs e)
        {
            ok.Enabled = e.NewNode != null && !e.NewNode.IsFolder;
        }

        /// <summary>
        /// OK to select and close
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
