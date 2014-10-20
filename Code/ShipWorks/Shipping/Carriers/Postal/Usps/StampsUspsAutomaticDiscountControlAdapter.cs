using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// An implementation of an IUspsAutomaticDiscountControlAdapter to be ussed when converting/
    /// signing up for a Stamps.com account from a Stamps.com shipment via the UspsAutomaticDiscountControl.
    /// </summary>
    public class StampsUspsAutomaticDiscountControlAdapter : IUspsAutomaticDiscountControlAdapter
    {
        private readonly ShippingSettingsEntity settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsUspsAutomaticDiscountControlAdapter"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public StampsUspsAutomaticDiscountControlAdapter(ShippingSettingsEntity settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Gets or sets a value indicating whether USPS automatic expeditedis being used.
        /// </summary>
        public bool UsingUspsAutomaticExpedited
        {
            get { return settings.StampsUspsAutomaticExpedited; }
            set { settings.StampsUspsAutomaticExpedited = value; }
        }

        /// <summary>
        /// Gets or sets the USPS automatic expedited account.
        /// </summary>
        public long UspsAutomaticExpeditedAccount
        {
            get { return settings.StampsUspsAutomaticExpeditedAccount; }
            set { settings.StampsUspsAutomaticExpeditedAccount = value; }
        }
    }
}
