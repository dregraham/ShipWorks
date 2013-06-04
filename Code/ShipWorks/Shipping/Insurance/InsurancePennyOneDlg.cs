using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Window for displaying benefits of penny one insurance
    /// </summary>
    public partial class InsurancePennyOneDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsurancePennyOneDlg(string carrier, bool allowEnablePennyOne)
        {
            InitializeComponent();

            labelHeaderText.Text = string.Format(labelHeaderText.Text, carrier);
            labelDeclaredValueInfo.Text = string.Format(labelDeclaredValueInfo.Text, carrier);

            if (!allowEnablePennyOne)
            {
                pennyOne.Visible = false;
                kryptonBorderEdge.Visible = false;
                labelProtect.Visible = false;

                Height -= (panel.Top - labelProtect.Top);
            }
        }

        /// <summary>
        /// Get's or sets whether the PennyOne box is enabled
        /// </summary>
        public bool PennyOne
        {
            get { return pennyOne.Checked; }
            set { pennyOne.Checked = value; }
        }
    }
}
