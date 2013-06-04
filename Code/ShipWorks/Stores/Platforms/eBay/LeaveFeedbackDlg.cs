using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Dialog for leaving eBay Feedback
    /// </summary>
    public partial class LeaveFeedbackDlg : Form
    {
        /// <summary>
        /// Type of feedback being left
        /// </summary>
        CommentTypeCodeType feedbackType = CommentTypeCodeType.Positive;
        public CommentTypeCodeType FeedbackType
        {
            get { return feedbackType; }
        }

        /// <summary>
        /// Feedback contents
        /// </summary>
        string comments = "";
        public string Comments
        {
            get { return comments; }
        }

        // collection of order keys passed in, representing those to have feedback left
        List<long> orderIds;

        /// <summary>
        /// If the user selected a single item to leave feedback for, this is the key
        /// </summary>
        long selectedItemId;
        public long SelectedItemID
        {
            get { return selectedItemId; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LeaveFeedbackDlg(List<long> orderIds)
        {
            InitializeComponent();

            this.orderIds = orderIds;
        }

        /// <summary>
        /// Selected to submit feedback
        /// </summary>
        private void OnSubmitClick(object sender, EventArgs e)
        {
            if (commentsTextBox.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "A feedback comment must be entered.");
                return;
            }

            // move values from the UI
            comments = commentsTextBox.Text;
            feedbackType = CommentTypeCodeType.Positive;
            selectedItemId = itemSelectionControl.SelectedKey;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Populate the GUI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // setup the "item" combobox
            itemSelectionControl.LoadOrders(orderIds);
        }
    }
}
