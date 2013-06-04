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

            sqlServer.Text = SqlSession.Current.ServerInstance;
            database.Text = SqlSession.Current.DatabaseName;

            if (SqlSession.Current.WindowsAuth)
            {
                windowsAuth.Checked = true;
                ActiveControl = ok;
            }
            else
            {
                sqlServerAuth.Checked = true;
                username.Text = SqlSession.Current.Username;

                if (SqlSession.Current.RememberPassword)
                {
                    password.Text = SqlSession.Current.Password;
                    ActiveControl = ok;
                }
                else
                {
                    ActiveControl = password;
                }

                remember.Checked = SqlSession.Current.RememberPassword;
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
            session.ServerInstance = sqlServer.Text;
            session.DatabaseName = database.Text;

            // Set them to the new settings
            session.Username = username.Text;
            session.Password = password.Text;
            session.RememberPassword = remember.Checked;
            session.WindowsAuth = windowsAuth.Checked;

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