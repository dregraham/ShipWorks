using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Class to see if CLR needs to be enabled
    /// </summary>
    public partial class NeedEnableClr : Form
    {
        SqlSession sqlSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeedEnableClr(SqlSession sqlSession)
        {
            InitializeComponent();

            this.sqlSession = sqlSession;
        }

        /// <summary>
        /// Enable CLR in SQL Server
        /// </summary>
        private void OnEnableClr(object sender, EventArgs e)
        {
            try
            {
                using (DbConnection con = sqlSession.OpenConnection())
                {
                    SqlUtility.EnableClr(con);
                }

                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}