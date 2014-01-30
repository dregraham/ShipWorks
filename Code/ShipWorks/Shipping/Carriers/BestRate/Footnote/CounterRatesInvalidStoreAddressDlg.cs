using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Dialog that allows the store address to be modified
    /// </summary>
    public partial class CounterRatesInvalidStoreAddressDlg : Form
    {
        private readonly StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store that will be edited</param>
        public CounterRatesInvalidStoreAddressDlg(StoreEntity store)
        {
            this.store = store;
            InitializeComponent();

            storeAddressControl.LoadStore(store);
        }

        /// <summary>
        /// The user wants to save changes
        /// </summary>
        private void OnOkButtonClick(object sender, EventArgs e)
        {
            storeAddressControl.SaveToEntity(store);
            StoreManager.SaveStore(store);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The user wants to cancel
        /// </summary>
        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
