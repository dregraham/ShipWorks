using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for controling which computers can be downloaded form
    /// </summary>
    public partial class ComputerDownloadPolicyDlg : Form
    {
        ComputerDownloadPolicy downloadPolicy;

        /// <summary>
        /// Constructor
        /// </summary>
        public ComputerDownloadPolicyDlg(ComputerDownloadPolicy policy, string storeName)
        {
            InitializeComponent();

            this.Text = string.Format("'{0}' Download Policy", storeName);
            this.downloadPolicy = policy;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            checkBoxDefault.Checked = downloadPolicy.DefaultToYes;

            int top = 0;
            foreach (ComputerEntity computer in ComputerManager.Computers)
            {
                Panel panel = new Panel();
                panel.Location = new Point(0, top);
                panel.Size = new Size(panelComputers.Width, 28);
                panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panel.Tag = computer;

                Label label = new Label();
                label.Location = new Point(0, 7);
                label.Text = computer.Name + ":";
                label.AutoSize = true;
                panel.Controls.Add(label);

                ComputerDownloadAllowedComboBox combo = new ComputerDownloadAllowedComboBox();
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.Location = new Point(202, 4);
                combo.LoadChoices(downloadPolicy.DefaultToYes);
                combo.Width = 100;
                panel.Controls.Add(combo);

                panelComputers.Controls.Add(panel);

                top = panel.Bottom;
                combo.SelectedValue = downloadPolicy.GetComputerAllowed(computer.ComputerID);
            }
        }

        /// <summary>
        /// Changing the setting of the default policy
        /// </summary>
        private void OnChangeDefault(object sender, EventArgs e)
        {
            foreach (Panel panel in panelComputers.Controls)
            {
                ((ComputerDownloadAllowedComboBox) panel.Controls[1]).LoadChoices(checkBoxDefault.Checked);
            }
        }

        /// <summary>
        /// User is OKing hte changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            downloadPolicy.DefaultToYes = checkBoxDefault.Checked;

            foreach (Panel panel in panelComputers.Controls)
            {
                ComputerEntity computer = (ComputerEntity) panel.Tag;
                ComputerDownloadAllowed allowed = (ComputerDownloadAllowed) ((ComputerDownloadAllowedComboBox) panel.Controls[1]).SelectedValue;

                downloadPolicy.SetComputerAllowed(computer.ComputerID, allowed);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
