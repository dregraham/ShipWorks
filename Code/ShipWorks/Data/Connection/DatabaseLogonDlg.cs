using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using System.Data.SqlClient;
using Interapptive.Shared;
using ShipWorks.Data.Administration;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Window for logging in to the SQL Server database
    /// </summary>
    partial class DatabaseLogonDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseLogonDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Should not be able to get here if session is not valid
            if (SqlSession.Current == null)
            {
                throw new InvalidOperationException("Attempt to log on to SQL Server without valid SqlSession.");
            }

            sqlServer.Text = SqlSession.Current.Configuration.ServerInstance;
            database.Text = SqlSession.Current.Configuration.DatabaseName;

            if (SqlSession.Current.Configuration.WindowsAuth)
            {
                windowsAuth.Checked = true;
                ActiveControl = ok;
            }
            else
            {
                sqlServerAuth.Checked = true;
                username.Text = SqlSession.Current.Configuration.Username;

                if (SqlSession.Current.Configuration.RememberPassword)
                {
                    password.Text = SqlSession.Current.Configuration.Password;
                    ActiveControl = ok;
                }
                else
                {
                    ActiveControl = password;
                }

                remember.Checked = SqlSession.Current.Configuration.RememberPassword;
            }
        }

        /// <summary>
        /// Changing the authentication type for sql server
        /// </summary>
        private void OnChangeSqlAuthType(object sender, System.EventArgs e)
        {
            username.Enabled = sqlServerAuth.Checked;
            password.Enabled = sqlServerAuth.Checked;
            remember.Enabled = sqlServerAuth.Checked;
        }

        /// <summary>
        /// Attempt to login using the specified credentials
        /// </summary>
        private void OnConnect(object sender, System.EventArgs e)
        {
            SqlSession session = new SqlSession();
            session.Configuration.ServerInstance = sqlServer.Text;
            session.Configuration.DatabaseName = database.Text;

            // Set them to the new settings
            session.Configuration.Username = username.Text;
            session.Configuration.Password = password.Text;
            session.Configuration.RememberPassword = remember.Checked;
            session.Configuration.WindowsAuth = windowsAuth.Checked;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                session.TestConnection();

                if (!session.CheckPermissions(SqlSessionPermissionSet.Standard, this))
                {
                    return;
                }

                // Worked
                session.SaveAsCurrent();

                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this,
                    "ShipWorks could not login to SQL Server.\n\n" +
                    "Detail: " + ex.Message);
            }
        }
    }
}