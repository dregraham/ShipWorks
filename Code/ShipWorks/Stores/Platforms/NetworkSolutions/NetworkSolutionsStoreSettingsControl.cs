using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// User control for changing download settings for NetworkSolutions
    /// </summary>
    public partial class NetworkSolutionsStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// state object for checkedlistbox
        /// </summary>
        class StatusCheckItem
        {
            public string Name;
            public long Code;

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load configuration from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            NetworkSolutionsStoreEntity nsStore = store as NetworkSolutionsStoreEntity;
            if (nsStore == null)
            {
                throw new InvalidOperationException("A non NetworkSolutions store was passed to the Network Solutions store settings control."); 
            }

            // get the collection of currently chosen codes to be downloaded
            List<string> selectedCodes = nsStore.DownloadOrderStatuses.Split(',').ToList();

            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider(nsStore);
            foreach (long code in codeProvider.CodeValues)
            {
                StatusCheckItem newItem = new StatusCheckItem
                {
                    Name = codeProvider.GetCodeName(code),
                    Code = code
                };

                bool chosen = selectedCodes.Contains(code.ToString());

                statuses.Items.Add(newItem, chosen);
            }
        }

        /// <summary>
        /// Save configuration to the entity.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            NetworkSolutionsStoreEntity nsStore = store as NetworkSolutionsStoreEntity;
            if (nsStore == null)
            {
                throw new InvalidOperationException("A non NetworkSolutions store was passed to the Network Solutions store settings control.");
            }

            if (statuses.CheckedItems.Count == 0)
            {
                MessageHelper.ShowError(this, "At least one order status must be selected for download.");
                return false;
            }

            List<long> chosenStatuses = new List<long>();
            foreach (StatusCheckItem selectedItem in statuses.CheckedItems)
            {
                chosenStatuses.Add(selectedItem.Code);
            }

            // save the selected statuses as CSV
            nsStore.DownloadOrderStatuses = string.Join(",", chosenStatuses.ConvertAll<string>(l => l.ToString()).ToArray());

            return true;
        }
    }
}
