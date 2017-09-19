using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Dialog for leaving eBay Feedback
    /// </summary>
    public partial class LeaveFeedbackDlg : Form
    {
        // collection of order keys passed in, representing those to have feedback left
        List<long> orderIds;

        /// <summary>
        /// Constructor
        /// </summary>
        public LeaveFeedbackDlg(IEnumerable<long> orderIds)
        {
            InitializeComponent();

            this.orderIds = orderIds.ToList();
        }

        /// <summary>
        /// Details for leaving feedback
        /// </summary>
        public EbayFeedbackDetails Details { get; private set; }

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
            Details = new EbayFeedbackDetails
            {
                Comments = commentsTextBox.Text,
                FeedbackType = CommentTypeCodeType.Positive,
                SelectedItemID = itemSelectionControl.SelectedKey
            };

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
