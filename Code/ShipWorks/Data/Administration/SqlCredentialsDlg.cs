using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Window for a user to input SQL credentials
    /// </summary>
    public partial class SqlCredentialsDlg : Form
    {
        SqlSession session;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlCredentialsDlg(SqlSession session)
        {
            InitializeComponent();

            this.session = session;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (session.Configuration.WindowsAuth)
            {
                windowsAuth.Checked = true;
            }
            else
            {
                sqlServerAuth.Checked = true;
                username.Text = session.Configuration.Username;
                password.Text = session.Configuration.Password;
            }

            ActiveControl = ok;
        }

        /// <summary>
        /// Changing the authentication type for sql server
        /// </summary>
        private void OnChangeSqlAuthType(object sender, System.EventArgs e)
        {
            username.Enabled = sqlServerAuth.Checked;
            password.Enabled = sqlServerAuth.Checked;
        }

        /// <summary>
        /// Save the UI settings back to the configuration
        /// </summary>
        private void OnOK(object sender, System.EventArgs e)
        {
            SqlSession clone = new SqlSession(session);
            clone.Configuration.Username = username.Text;
            clone.Configuration.Password = password.Text;
            clone.Configuration.WindowsAuth = windowsAuth.Checked;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clone.TestConnection();

                // Worked
                session.Configuration.CopyFrom(clone.Configuration);

                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this,
                    "ShipWorks could not connect with the given credentials.\n\n" +
                    "Detail: " + ex.Message);
            }
        }
    }
}
