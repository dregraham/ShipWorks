using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// CA-specific store settings
    /// </summary>
    [ToolboxItem(true)]
    public partial class ChannelAdvisorStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreSettingsControl()
        {
            InitializeComponent();

            // prepare the dropdown
            criteriaComboBox.DisplayMember = "Text";
            criteriaComboBox.ValueMember = "Criteria";

            criteriaComboBox.DataSource = new List<object> {
                new { Text = "Payment has cleared, not yet shipped", Criteria = ChannelAdvisorDownloadCriteria.PaidNotShipped},
                new { Text = "Payment has cleared, any shipping status", Criteria = ChannelAdvisorDownloadCriteria.Paid},
                new { Text = "Any Payment status, any shipping status", Criteria = ChannelAdvisorDownloadCriteria.All},
                new { Text = "Any payment status, not yet shipped", Criteria = ChannelAdvisorDownloadCriteria.NotShipped}
            };
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }

            criteriaComboBox.SelectedValue = (ChannelAdvisorDownloadCriteria)caStore.DownloadCriteria;
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }

            ChannelAdvisorDownloadCriteria selected = criteriaComboBox.SelectedValue == null ? ChannelAdvisorDownloadCriteria.All : (ChannelAdvisorDownloadCriteria)criteriaComboBox.SelectedValue;
            caStore.DownloadCriteria = (short)selected;

            return true;
        }

        /// <summary>
        /// Allows turning off the section header.  This is hear b\c the StoreWizard reuses this control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowHeader
        {
            get { return sectionHeader.Visible; }
            set { sectionHeader.Visible = value; }
        }
    }
}
