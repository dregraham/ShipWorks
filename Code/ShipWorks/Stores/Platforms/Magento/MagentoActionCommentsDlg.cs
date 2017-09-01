using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Window for allowing the user to enter a custom comment for taking an action on a Magento order
    /// </summary>
    public partial class MagentoActionCommentsDlg : Form
    {
        private readonly MagentoVersion version;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoActionCommentsDlg(MagentoVersion version)
        {
            this.version = version;
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            action.DisplayMember = "Text";
            action.ValueMember = "Action";

            List<object> commands = new List<object>
            {
                new {Text = "Cancel", Action = MagentoUploadCommand.Cancel},
                new {Text = "Complete", Action = MagentoUploadCommand.Complete},
                new {Text = "Hold", Action = MagentoUploadCommand.Hold}
            };
            if (version == MagentoVersion.MagentoTwoREST)
            {
                commands.Add(new { Text = "Unhold", Action = MagentoUploadCommand.Unhold });
                commands.Add(new { Text = "Comments only", Action = MagentoUploadCommand.Comments });
            }

            action.DataSource = commands;
            action.SelectedIndex = 0;
        }

        /// <summary>
        /// The code the user has selected
        /// </summary>
        public MagentoUploadCommand Action
        {
            get
            {
                if (action.SelectedIndex >= 0)
                {
                    return (MagentoUploadCommand) action.SelectedValue;
                }

                return MagentoUploadCommand.None;
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
