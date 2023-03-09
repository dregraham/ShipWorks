using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.Messages
{
    public partial class HubMessagesDialog : Form
    {
        public HubMessagesDialog()
        {
            InitializeComponent();
            messageControl.ParentDialog = this;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// Set the messages to display
        /// </summary>
        public void SetMessages(IEnumerable<MessageDTO> messages)
        {
            this.messageControl.SetMessages(messages);
        }

        /// <summary>
        /// Prevent closing through any means until the user has acknowledged the messages
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (this.messageControl.Acknowledgement.Visibility == Visibility.Visible && (!this.messageControl.Acknowledgement.IsChecked ?? true))
            {
                e.Cancel = true;
            }
        }
    }
}
