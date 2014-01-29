using System;
using ShipWorks.Shipping.Editing;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    public partial class CounterRatesInvalidStoreAddressFootnoteControl : RateFootnoteControl
    {
        private readonly FootnoteParameters parameters;

        public CounterRatesInvalidStoreAddressFootnoteControl(FootnoteParameters parameters)
        {
            this.parameters = parameters;
            InitializeComponent();
        }

        private void OnShowAddress(object sender, EventArgs e)
        {
            // TODO: implement store popup
            // StoreType storeType = parameters.GetStoreTypeAction();
            
            
        }
    }
}
