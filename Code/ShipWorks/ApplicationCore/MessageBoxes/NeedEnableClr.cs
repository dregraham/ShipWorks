using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using System.Data.SqlClient;
using ShipWorks.Data.Administration;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    public partial class NeedEnableClr : Form
    {
        SqlSession sqlSession;

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
                using (SqlConnection con = sqlSession.OpenConnection())
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