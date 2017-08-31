using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Dialog for gathering details about a message to be sent to an eBay user.
    /// </summary>
    public partial class EbayMessagingDlg : Form
    {
        // Order Ids provided to the dialog to send messages for
        List<long> orderIds;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayMessagingDlg(IEnumerable<long> orderIds)
        {
            InitializeComponent();

            this.orderIds = orderIds.ToList();
        }

        /// <summary>
        /// Details about a message to be sent to an eBay user
        /// </summary>
        public EbayMessagingDetails Details { get; private set; }

        /// <summary>
        /// Loading, populate the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // add the Message Type
            EnumHelper.BindComboBox<EbaySendMessageType>(messageTypeComboBox,
                t => t == EbaySendMessageType.CustomizedSubject ||
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
            Details = new EbayMessagingDetails
            {
                MessageType = (EbaySendMessageType) messageTypeComboBox.SelectedValue,
                Subject = subjectTextBox.Text,
                Message = messageTextBox.Text,
                CopyMe = copyMeCheckBox.Checked,
                SelectedItemID = itemSelectionControl.SelectedKey
            };

            // signal we've accepted
            DialogResult = DialogResult.OK;
        }
    }
}
