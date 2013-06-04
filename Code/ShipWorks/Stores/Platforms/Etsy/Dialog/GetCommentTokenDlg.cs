using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Etsy.Dialog
{
    /// <summary>
    /// Prompt for user to enter comment token.
    /// </summary>
    public partial class GetCommentTokenDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetCommentTokenDlg()
        {
            InitializeComponent();
            tokenBox.Text = "{//ServiceUsed} - {//TrackingNumber}";
        }

        /// <summary>
        /// The Comment Token entered by user.
        /// </summary>
        public string Comment
        {
            get
            {
                return tokenBox.Text;
            }
        }
    }
}