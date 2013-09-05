using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Teh UI for configuring/editing the settings of a DownloadOrdersTask.
    /// </summary>
    public partial class DownloadOrdersTaskEditor : ActionTaskEditor
    {
        private readonly DownloadOrdersTask task;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOrdersTaskEditor"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public DownloadOrdersTaskEditor(DownloadOrdersTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            InitializeComponent();
            this.task = task;

            // Don't truncate the display of the panel after loading the stores
            storeCheckBoxPanel.LoadStores();

            // Update the selected stores based on the task settings
            IEnumerable<long> storeIDs = this.task.StoreIDs;
            storeCheckBoxPanel.SelectedStoreIDs = storeIDs;

            Height = storeCheckBoxPanel.Bottom + 5;

            storeCheckBoxPanel.StoreSelectionChanged += OnStoreSelectionChanged;
        }

        /// <summary>
        /// Called when the list of stores has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnStoreSelectionChanged(object sender, System.EventArgs e)
        {
            task.StoreIDs = storeCheckBoxPanel.SelectedStoreIDs;
        }


    }
}
