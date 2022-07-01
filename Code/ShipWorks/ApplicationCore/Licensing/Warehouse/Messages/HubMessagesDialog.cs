using System.Collections.Generic;
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
        }

        /// <summary>
        /// Set the messages to display
        /// </summary>
        public void SetMessages(IEnumerable<MessageDTO> messages)
        {
            this.messageControl.SetMessages(messages);
        }
    }
}
