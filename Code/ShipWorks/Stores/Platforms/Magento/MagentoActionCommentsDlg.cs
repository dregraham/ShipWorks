using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Window for allowing the user to enter a custom comment for taking an action on a Magento order
    /// </summary>
    public partial class MagentoActionCommentsDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoActionCommentsDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            action.DisplayMember = "Text";
            action.ValueMember = "Action";
            action.DataSource = new List<object> {
                new { Text = "Cancel", Action = "cancel" },
                new { Text = "Complete", Action = "complete" },
                new { Text = "Hold", Action = "hold" } 
            };

            action.SelectedIndex = 0;
        }

        /// <summary>
        /// The code the user has selected
        /// </summary>
        public string Action
        {
            get
            {
                if (action.SelectedIndex >= 0)
                {
                    return (string)action.SelectedValue;
                }

                return "";
            }
        }

        /// <summary>
        /// The user entered comments
        /// </summary>
        public string Comments
        {
            get
            {
                return comment.Text;
            }
        }
    }
}
