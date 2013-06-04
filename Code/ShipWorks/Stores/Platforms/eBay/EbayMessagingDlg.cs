using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Dialog for gathering details about a message to be sent to an eBay user.
    /// </summary>
    public partial class EbayMessagingDlg : Form
    {
        // The type of eBay message to send
        public EbaySendMessageType MessageType { get; set; }

        // The subject of the message to send
        public string Subject { get; set; }

        // The message contents
        public string Message { get; set; }
        
        // Whether or not to have eBay send a copy to the seller (SW user)
        public bool CopyMe { get; set; }

        // Order Ids provided to the dialog to send messages for
        List<long> orderIds;

        /// <summary>
        /// Returns the ID of the selected order item, or 0 if All
        /// </summary>
        long selectedItemId;
        public long SelectedItemID
        {
            get { return selectedItemId; }
        }
            
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayMessagingDlg(List<long> orderIds)
        {
            InitializeComponent();

            this.orderIds = orderIds;
        }

        /// <summary>
        /// Loading, populate the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // add the Message Type
            EnumHelper.BindComboBox<EbaySendMessageType>(messageTypeComboBox, t => t == EbaySendMessageType.CustomizedSubject ||
                                                                           t == EbaySendMessageType.General ||
                                                                           t == EbaySendMessageType.Payment ||
                                                                           t == EbaySendMessageType.Shipping);
            // default to General
            messageTypeComboBox.SelectedValue = EbaySendMessageType.General;

            // load the item combobox
            itemSelectionControl.LoadOrders(orderIds);
        }
       
        /// <summary>
        /// Send was clicked
        /// </summary>
        private void OnSendClick(object sender, EventArgs e)
        {
            // copy UI values to their properties
            MessageType = (EbaySendMessageType)messageTypeComboBox.SelectedValue;
            Subject = subjectTextBox.Text;
            Message = messageTextBox.Text;
            CopyMe = copyMeCheckBox.Checked;
            selectedItemId = itemSelectionControl.SelectedKey;

            // signal we've accepted
            DialogResult = DialogResult.OK;
        }
    }
}
