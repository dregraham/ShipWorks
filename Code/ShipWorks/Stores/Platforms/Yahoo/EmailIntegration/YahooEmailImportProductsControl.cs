using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// User control for importing inventory from a yahoo product
    /// </summary>
    public partial class YahooEmailImportProductsControl : UserControl
    {
        long storeID = 0;

        bool importComplete = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailImportProductsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control for what store the inventory import will be for
        /// </summary>
        public void InitializeForStore(long storeID)
        {
            this.storeID = storeID;
        }

        /// <summary>
        /// Indicates if an import occurred at least once
        /// </summary>
        public bool ImportComplete
        {
            get { return importComplete; }
        }

        /// <summary>
        /// Import inventory
        /// </summary>
        private void OnImportInventory(object sender, EventArgs e)
        {
            if (storeID == 0)
            {
                throw new InvalidOperationException("The control has not been initialized for a Yahoo store.");
            }

            Cursor.Current = Cursors.WaitCursor;

            ProgressItem progressItem = new ProgressItem("Import");
            ProgressProvider progressProvider = new ProgressProvider();
            progressProvider.ProgressItems.Add(progressItem);

            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Import Catalog";
            progressDlg.Description = "ShipWorks is importing the catalog.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));

            Dictionary<string, object> state = new Dictionary<string, object>();
            state["delayer"] = delayer;
            state["url"] = catalogUrl.Text;
            state["progressItem"] = progressItem;

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(BackgroundImportInventory, "importing inventory"), state);
        }

        /// <summary>
        /// Import the inventory in the background
        /// </summary>
        private void BackgroundImportInventory(object state)
        {
            Dictionary<string, object> stateMap = (Dictionary<string, object>) state;

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) stateMap["delayer"];
            ProgressItem progressItem = (ProgressItem) stateMap["progressItem"];
            string url = (string) stateMap["url"];

            Exception error = null;

            try
            {
                YahooEmailProductManager.ImportProductCatalog(progressItem, storeID, url);
            }
            catch (YahooException ex)
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
                importComplete = true;

                MessageHelper.ShowInformation(this, "The product catalog was successfully imported.");
            }
        }
    }
}
