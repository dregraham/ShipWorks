using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    /// <summary>
    /// Editor for configuring the EbayMessageBuyerTask
    /// </summary>
    public partial class EbayMessageBuyerTaskEditor : ActionTaskEditor
    {
        // the task this editor instance configures
        EbayMessageBuyerTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayMessageBuyerTaskEditor(EbayMessageBuyerTask task)
        {
            InitializeComponent();

            this.task = task;

            // add the Message Type
            EnumHelper.BindComboBox<EbaySendMessageType>(messageTypeComboBox, t => t == EbaySendMessageType.CustomizedSubject ||
                                                                           t == EbaySendMessageType.General ||
                                                                           t == EbaySendMessageType.Payment ||
                                                                           t == EbaySendMessageType.Shipping);
        }

        /// <summary>
        /// Load the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // configure the UI
            messageTypeComboBox.SelectedValue = task.MessageType;
            subjectTextBox.Text = task.Subject;
            messageTextBox.Text = task.Body;
            copyMeCheckBox.Checked = task.CopyMe;

            // default to shipping
            messageTypeComboBox.SelectedValue = EbaySendMessageType.Shipping;

            messageTextBox.TextChanged += new EventHandler(OnMessageTextChanged);
            subjectTextBox.TextChanged += new EventHandler(OnSubjectTextChanged);
        }

        /// <summary>
        /// Message text has been changed, propogate it to the task
        /// </summary>
        void OnMessageTextChanged(object sender, EventArgs e)
        {
            task.Body = messageTextBox.Text;
        }

        /// <summary>
        /// Subject has been changed, propogate it to the task
        /// </summary>
        private void OnSubjectTextChanged(object sender, EventArgs e)
        {
            task.Subject = subjectTextBox.Text;
        }

        /// <summary>
        /// Copy Me has been toggled, propogate the setting back to the task
        /// </summary>
        private void OnCopyCheckedChanged(object sender, EventArgs e)
        {
            task.CopyMe = copyMeCheckBox.Checked;
        }

        /// <summary>
        /// Propagate the selected MessageType to the underlying task
        /// </summary>
        private void OnMessageTypeChanged(object sender, EventArgs e)
        {
            task.MessageType = (EbaySendMessageType)messageTypeComboBox.SelectedValue;
        }
    }
}
