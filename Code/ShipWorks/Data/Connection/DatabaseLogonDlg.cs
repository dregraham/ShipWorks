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
            }
            else
            {
                sqlServerAuth.Checked = true;
                username.Text = SqlSession.Current.Configuration.Username;
                password.Text = SqlSession.Current.Configuration.Password;
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
        /// Attempt to login using the specified credentials
        /// </summary>
        private void OnConnect(object sender, System.EventArgs e)
        {
            var session = SaveToNewSqlSession();

            if (!ValidateSession(session))
            {
                return;
            }

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

        /// <summary>
        /// Saves to new SQL session (in memory)
        /// </summary>
        private SqlSession SaveToNewSqlSession()
        {
            SqlSession session = new SqlSession();
            session.Configuration.ServerInstance = sqlServer.Text;
            session.Configuration.DatabaseName = database.Text;

            // Set them to the new settings
            session.Configuration.WindowsAuth = windowsAuth.Checked;

            if (sqlServerAuth.Checked)
            {
                session.Configuration.Username = username.Text;
                session.Configuration.Password = password.Text;
            }
            return session;
        }

        /// <summary>
        /// Validates the session.
        /// </summary>
        private bool ValidateSession(SqlSession session)
        {
            if (!session.Configuration.WindowsAuth)
            {
                if (string.IsNullOrEmpty(session.Configuration.Username))
                {
                    MessageHelper.ShowError(this, $"A Validaiton Error has occured. {Environment.NewLine}{Environment.NewLine} Username is required");
                    return false;
                }
            }

            return true;
        }
    }
}