using System;
using System.Linq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.UI.Controls.StoreSettings
{
    /// <summary>
    /// Base control for setting download modified days back
    /// </summary>
    public abstract partial class DownloadModifiedDaysBackControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected DownloadModifiedDaysBackControl()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public abstract int MaxDaysBack { get; }

        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public abstract int LoadDaysBack(StoreEntity store);

        /// <summary>
        /// Save the days back to the store entity
        /// </summary>
        public abstract void SaveDaysBack(StoreEntity store, int daysBack);

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ResetList();
            daysBack.SelectedItem = LoadDaysBack(store);

            infoLabel.Text = "When this option is not 0, ShipWorks will go back and check every order within the " +
                             $"timeframe specified. This takes time but, due to {EnumHelper.GetDescription(store.StoreTypeCode)} limitations, not doing so may lead " +
                             "to missed modifications or skipped orders.";
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            try
            {
                SaveDaysBack(store, (int) daysBack.SelectedItem);
                return true;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Reset the list of numbers with the current max
        /// </summary>
        private void ResetList()
        {
            daysBack.Items.Clear();

            foreach (int number in Enumerable.Range(0, MaxDaysBack + 1))
            {
                daysBack.Items.Add(number);
            }
        }
    }
}
