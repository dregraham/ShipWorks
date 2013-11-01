using System;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateServiceControl(ShipmentTypeCode shipmentTypeCode)
            : base (shipmentTypeCode)
        {
            InitializeComponent();

            rateControl.ReloadRatesRequired += OnReloadRatesRequired;
        }

        private void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
