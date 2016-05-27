using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    public partial class ShippingAccountRequiredForRatingFootnoteControl : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingAccountRequiredForRatingFootnoteControl"/> class.
        /// </summary>
        public ShippingAccountRequiredForRatingFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Link to open the shipping settings to add a shipping account.
        /// </summary>
        private void OnAddShippingAccount(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShippingSettingsDlg shippingSettingsDlg = new ShippingSettingsDlg(lifetimeScope))
                {
                    // Show the shipping settings dialog to guide/nudge the user on to the
                    // account creation process
                    shippingSettingsDlg.ShowDialog(this);

                    // Trigger the rates to refresh
                    RaiseRateCriteriaChanged();
                }
            }
        }
    }
}
