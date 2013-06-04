using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// UserControl for importing amazon inventory
    /// </summary>
    public partial class AmazonImportInventoryControl : UserControl
    {
        long amazonStoreID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonImportInventoryControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialized the control for what store the inventory import will be for
        /// </summary>
        public void InitializeForStore(long amazonStoreID)
        {
            this.amazonStoreID = amazonStoreID;
        }

        /// <summary>
        /// Import inventory
        /// </summary>
        private void OnImportInventory(object sender, EventArgs e)
        {
            if (amazonStoreID == 0)
            {
                throw new InvalidOperationException("The control has not been initialized for an Amazon store.");
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Text Files (*.txt)|*.txt";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ProgressItem progressItem = new ProgressItem("Import");
                    ProgressProvider progressProvider = new ProgressProvider();
                    progressProvider.ProgressItems.Add(progressItem);

                    ProgressDlg progressDlg = new ProgressDlg(progressProvider);
                    progressDlg.Title = "Import Inventory";
                    progressDlg.Description = "ShipWorks is importing inventory.";

                    ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);
                    delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));

                    Dictionary<string, object> state = new Dictionary<string, object>();
                    state["delayer"] = delayer;
                    state["filename"] = dlg.FileName;
                    state["progressItem"] = progressItem;

                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(BackgroundImportInventory, "importing inventory"), state);
                }
            }
        }

        /// <summary>
        /// Import the inventory in the background
        /// </summary>
        private void BackgroundImportInventory(object state)
        {
            Dictionary<string, object> stateMap = (Dictionary<string, object>) state;

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) stateMap["delayer"];
            ProgressItem progressItem = (ProgressItem) stateMap["progressItem"];
            string filename = (string) stateMap["filename"];

            Exception error = null;

            try
            {
                AmazonAsin.ImportInventory(progressItem, amazonStoreID, filename);
            }
            catch (AmazonException ex)
            {
                error = ex;
            }

            BeginInvoke(new MethodInvoker<ProgressDisplayDelayer, Exception>(BackgroundImportInventoryComplete), delayer, error);
        }

        /// <summary>
        /// The inventory import has completed
        /// </summary>
        private void BackgroundImportInventoryComplete(ProgressDisplayDelayer delayer, Exception error)
        {
            delayer.NotifyComplete();

            // Check for error conditions
            if (error != null)
            {
                MessageHelper.ShowError(this, error.Message);
            }
            else
            {
                MessageHelper.ShowInformation(this, "The inventory was successfully imported.");
            }
        }
    }
}
