using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// An implementation of an IUspsAutomaticDiscountControlAdapter to be ussed when converting/
    /// signing up for a USPS account from an Endicia shipment via the UspsAutomaticDiscountControl.
    /// </summary>
    public class EndiciaUspsAutomaticDiscountControlAdapter : IUspsAutomaticDiscountControlAdapter
    {
        private readonly ShippingSettingsEntity settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaUspsAutomaticDiscountControlAdapter"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public EndiciaUspsAutomaticDiscountControlAdapter(ShippingSettingsEntity settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Gets or sets a value indicating whether USPS automatic expeditedis being used.
        /// </summary>
        public bool UsingUspsAutomaticExpedited
        {
            get { return settings.EndiciaUspsAutomaticExpedited; }
            set { settings.EndiciaUspsAutomaticExpedited = value; }
        }

        /// <summary>
        /// Gets or sets the USPS automatic expedited account.
        /// </summary>
        public long UspsAutomaticExpeditedAccount
        {
            get { return settings.EndiciaUspsAutomaticExpeditedAccount; }
            set { settings.EndiciaUspsAutomaticExpeditedAccount = value; }
        }
    }
}
