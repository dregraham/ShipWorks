using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Actions
{
    /// <summary>
    /// UI for the Magento Shipment Upload task
    /// </summary>
    public partial class MagentoShipmentUploadTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        MagentoShipmentUploadTask task = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoShipmentUploadTaskEditor(MagentoShipmentUploadTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            MagentoStoreEntity store = StoreManager.GetStore(task.StoreID) as MagentoStoreEntity;

            if (store?.MagentoVersion == (int)MagentoVersion.MagentoTwo)
            {
                sendEmailCheckBox.Hide();
            }

            this.task = task;
            tokenBox.Text = task.Comment;
            sendEmailCheckBox.Checked = task.MagentoSendEmail;

            tokenBox.TextChanged += new EventHandler(OnTokenChanged);
        }

        /// <summary>
        /// Comments changed
        /// </summary>
        void OnTokenChanged(object sender, EventArgs e)
        {
            task.Comment = tokenBox.Text;
        }

        /// <summary>
        /// Toggled whether or not to have magento send emails
        /// </summary>
        private void OnSendEmailChecked(object sender, EventArgs e)
        {
            task.MagentoSendEmail = sendEmailCheckBox.Checked;
        }
    }
}
