using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Templates.Management;
using ShipWorks.Templates.Distribution;
using ShipWorks.Templates;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// Window that is shown when installing a default print rule for a shipment type and the Template needed is missing
    /// </summary>
    public partial class PrintRuleInstallMissingTemplateDlg : Form
    {
        long templateID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintRuleInstallMissingTemplateDlg(string fullName)
        {
            InitializeComponent();

            labelName.Text = fullName;
        }

        /// <summary>
        /// The TemplateID of the chosen template
        /// </summary>
        public long TemplateID
        {
            get { return templateID; }
        }

        /// <summary>
        /// Choose an existing template
        /// </summary>
        private void OnChooseTemplate(object sender, EventArgs e)
        {
            using (ChooseTemplateDlg dlg = new ChooseTemplateDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    templateID = dlg.SelectedTemplateID;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// Recreate the original template that was shipped with ShipWorks
        /// </summary>
        private void OnRecreateTemplate(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                templateID = BuiltinTemplates.InstallTemplate(labelName.Text, TemplateManager.Tree.CreateEditableClone()).TemplateID;

                adapter.Commit();
            }

            TemplateManager.CheckForChangesNeeded();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Can't close if they haven't already selected
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (templateID == 0)
            {
                e.Cancel = true;
            }
        }
    }
}
