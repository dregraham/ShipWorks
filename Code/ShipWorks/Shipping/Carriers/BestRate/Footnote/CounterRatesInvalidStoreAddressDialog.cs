using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Dialog that allows the store address to be modified
    /// </summary>
    public partial class CounterRatesInvalidStoreAddressDialog : Form
    {
        private readonly StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store that will be edited</param>
        public CounterRatesInvalidStoreAddressDialog(StoreEntity store)
        {
            this.store = store;
            InitializeComponent();

            storeAddressControl.LoadStore(store);
        }

        /// <summary>
        /// The user wants to save changes
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            storeAddressControl.SaveToEntity(store);
            StoreManager.SaveStore(store);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The user wants to cancel
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
