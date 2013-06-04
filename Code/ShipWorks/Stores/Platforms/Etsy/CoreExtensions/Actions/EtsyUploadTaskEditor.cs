using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Etsy Shipment Upload Action Editor
    /// </summary>
    public partial class EtsyUploadTaskEditor : ActionTaskEditor
    {
        EtsyUploadTask task;
        bool initialized = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyUploadTaskEditor(EtsyUploadTask etsyUploadTask)
        {
            InitializeComponent();
            if (etsyUploadTask == null)
            {
                throw new ArgumentNullException("etsyUploadTask");
            }

            task = etsyUploadTask;
            tokenBox.Text = etsyUploadTask.Comment;
            shippedCheckBox.Checked = etsyUploadTask.MarkAsShipped;
            paidCheckBox.Checked = etsyUploadTask.MarkAsPaid;
            setComment.Checked = etsyUploadTask.WithComment;

            // Listen for comment changes
            tokenBox.TextChanged += new EventHandler(OnTokenChanged);

            initialized = true;
            StateChanged();
        }

        /// <summary>
        /// Change the associated task comment to the entered text 
        /// </summary>
        void OnTokenChanged(object sender, EventArgs e)
        {
            StateChanged();
        }

        /// <summary>
        /// Handles ShippedCheckBoxCheckedChanged
        /// </summary>
        private void OnShippedCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            StateChanged();
        }

        /// <summary>
        /// Handles OnPaidCheckBoxCheckedChanged
        /// </summary>
        private void OnPaidCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            StateChanged();
        }

        /// <summary>
        /// Handles OnWithCommentCheckedChanged
        /// </summary>
        private void OnWithCommentCheckedChanged(object sender, EventArgs e)
        {
            StateChanged();
        }

        /// <summary>
        /// Handles enabling/disabling controls and setting task values when checkbox states change.
        /// </summary>
        private void StateChanged()
        {
            if (initialized)
            {
                tokenBox.Enabled = setComment.Checked && shippedCheckBox.Checked;
                setComment.Enabled = shippedCheckBox.Checked;

                task.Comment = tokenBox.Enabled ? tokenBox.Text : string.Empty;
                task.WithComment = setComment.Enabled ? setComment.Checked : false;

                task.MarkAsShipped = shippedCheckBox.Checked;
                task.MarkAsPaid = paidCheckBox.Checked;
            }
        }
    }
}